using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    //Player States
    public enum PlayerState
    {
        Idle,
        Run,
        Crouch,
        Attack,
        Jump,
        Falling,
    }
    PlayerState myState;

    //Components
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator anim;

    //Groundcheck stuff
    [SerializeField] LayerMask isGroundLayer;
    [SerializeField] float groundCheckRadius = 0.02f;
    bool isGrounded = false;
    Transform groundCheck;

    //Variables
    [SerializeField] float speed = 10.0f;
    [SerializeField] float jumpForce = 500.0f;
    float moveX;
    float inputY;
    bool attackWindow = true;
    int attackNumber = 0;

    //Animation Clips
    readonly string idleClip = "Idle";
    readonly string runClip = "Run";
    readonly string fallingClip = "Fall";
    readonly string jumpClip = "Jump";
    readonly string crouchClip = "Crouch";
    string attackClip = "Attack1";



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        if (speed <= 0)
        {
            speed = 10.0f;
            Debug.Log("Speed has been set to a default value " + speed.ToString());
        }

        if (jumpForce <= 0)
        {
            jumpForce = 400;
            Debug.Log("JumpForce has been set to a default value " + jumpForce.ToString());
        }

        //create groundcheck object if null and set it to be childed to the player object
        if (groundCheck == null)
        {
            GameObject obj = new GameObject();
            obj.name = "GroundCheck";
            obj.transform.parent = transform;
            obj.transform.localPosition = new Vector3(0,0,0);
            groundCheck = obj.transform;
        }

        if (groundCheckRadius <= 0)
        {
            groundCheckRadius = 0.02f;
            Debug.Log("Ground Check Radius has been set to a default value " + groundCheckRadius.ToString());
        }

        InputManager.Instance.OnPlayerMove += Move;
        InputManager.Instance.OnPlayerMoveCanceled += MoveCanceled;
        InputManager.Instance.OnPlayerJump += Jump;
        InputManager.Instance.OnPlayerCrouch += Crouch;
        InputManager.Instance.OnPlayerCrouchCanceled += CrouchCanceled;
        InputManager.Instance.OnPlayerAttack += Attack;
    }

    private void OnDestroy()
    {
        InputManager.Instance.OnPlayerMove -= Move;
        InputManager.Instance.OnPlayerMoveCanceled -= MoveCanceled;
        InputManager.Instance.OnPlayerJump -= Jump;
        InputManager.Instance.OnPlayerCrouch -= Crouch;
        InputManager.Instance.OnPlayerCrouchCanceled -= CrouchCanceled;
        InputManager.Instance.OnPlayerAttack -= Attack;
    }
    
    //set our crouching input
    void Crouch(float crouchInput)
    {
        inputY = crouchInput;
    }
    void CrouchCanceled(float crouchInput)
    {
        inputY = crouchInput;
    }

    //set our horizontal movement input
    void Move(float moveInput)
    {
        moveX = moveInput * speed;
        Debug.Log(moveX);
        sr.flipX = moveInput > 0 ? false : true;
    }
    void MoveCanceled(float moveInput)
    {
        moveX = moveInput;
    }

    void Jump()
    {
        //can't jump if we aren't grounded
        if (!isGrounded) return;

        rb.AddForce(Vector2.up * jumpForce);
    }

    void Attack()
    {
        //currently unable to attack if in air
        if (!isGrounded) return;

        if (attackWindow)
        {
            AnimatorClipInfo[] clipInfo = anim.GetCurrentAnimatorClipInfo(0);

            //if we are not currently in an attack - this means we need to move to the first attack
            if (!clipInfo[0].clip.name.Contains("Attack"))
                attackNumber = 1;

            //if we are in attack1, we need to move to the second attack
            if (clipInfo[0].clip.name == "Attack1")
                attackNumber = 2;

            //if we are in attack2, we need to move into the third attack
            if (clipInfo[0].clip.name == "Attack2")
                attackNumber = 3;

            attackWindow = false;
        }
    }

    //Animation event on first frame of the attack
    public void AttackStart()
    {
        AnimatorClipInfo[] clipInfo = anim.GetCurrentAnimatorClipInfo(0);
        float dir = sr.flipX ? -1 : 1;

        if (clipInfo[0].clip.name == "Attack1")
            rb.AddForce(new Vector2(300 * dir, 0));

        if (clipInfo[0].clip.name == "Attack2")
            rb.AddForce(new Vector2(400 * dir, 0));

        if (clipInfo[0].clip.name == "Attack3")
            rb.AddForce(new Vector2(500 * dir, 0));
    }

    //animation event to trigger hitboxes and the attack window to enter the next attack
    public void ResetAttackWindow()
    {
        //TODO turn on the appropriate collider for our attack depending on the attack animation we are in

        attackWindow = true;
    }
    
    //animation event to check to see what happens when we end our attack
    public void AttackEnd()
    {
        AnimatorClipInfo[] clipInfo = anim.GetCurrentAnimatorClipInfo(0);
        if (clipInfo[0].clip.name.Contains(attackNumber.ToString()))
        {
            attackNumber = 0;
            attackWindow = true;
            attackClip = "Attack1";
        }
        else
        {
            //2 or 3 to the end of the attackClip string
            attackClip = "Attack" + attackNumber.ToString();
        }

    }

    //Logic for the player and switching between states will happen here.
    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, isGroundLayer);

        //if we aren't grounded - we are either falling or jumping
        if (!isGrounded) myState = rb.velocity.y > 0 ? PlayerState.Jump : PlayerState.Falling;
        
        //if we are grounded - we are either idle or running
        if (isGrounded) myState = Mathf.Abs(moveX) > 0 ? PlayerState.Run : PlayerState.Idle;

        //if we press down - we will instantly enter crouch as long as we aren't jumping
        if (inputY == -1 && isGrounded) myState = PlayerState.Crouch;

        //If we press the attack button and we are able to enter into an attack - we will do so here
        if (attackNumber > 0) myState = PlayerState.Attack;

        CheckAnimations();
    }

    //logic for player movement.
    void FixedUpdate()
    {
        //if we are running/jumping/falling = we can move left or right - otherwise we do not move left or right
        rb.velocity = (myState == PlayerState.Run || myState == PlayerState.Jump || myState == PlayerState.Falling) ? new Vector2(moveX, rb.velocity.y) : new Vector2(0, rb.velocity.y);
    }

    //Check to see which animation to play.
    void CheckAnimations()
    {
        switch (myState)
        {
            case PlayerState.Idle:
                if (anim.GetCurrentAnimatorClipInfo(0)[0].clip.name != idleClip)
                    anim.Play(idleClip);
                break;
            
            case PlayerState.Run:
                if (anim.GetCurrentAnimatorClipInfo(0)[0].clip.name != runClip)
                    anim.Play(runClip);
                break;
            
            case PlayerState.Crouch:
                if (anim.GetCurrentAnimatorClipInfo(0)[0].clip.name != crouchClip)
                    anim.Play(crouchClip);
                break;
            
            case PlayerState.Attack:
                Debug.Log(attackClip);
                if (anim.GetCurrentAnimatorClipInfo(0)[0].clip.name != attackClip)
                    anim.Play(attackClip);
                break;
            
            case PlayerState.Falling:
                if (anim.GetCurrentAnimatorClipInfo(0)[0].clip.name != fallingClip)
                    anim.Play(fallingClip);
                break;
            
            case PlayerState.Jump:
                if (anim.GetCurrentAnimatorClipInfo(0)[0].clip.name != jumpClip)
                    anim.Play(jumpClip);
                break;
        }
    }
}
