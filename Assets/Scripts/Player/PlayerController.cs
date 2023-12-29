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

    //Animation Clips
    readonly string idleClip = "Idle";
    readonly string runClip = "Run";
    readonly string fallingClip = "Fall";
    readonly string jumpClip = "Jump";
    readonly string crouchClip = "Crouch";


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
    }

    private void OnDestroy()
    {
        InputManager.Instance.OnPlayerMove -= Move;
        InputManager.Instance.OnPlayerMoveCanceled -= MoveCanceled;
        InputManager.Instance.OnPlayerJump -= Jump;
        InputManager.Instance.OnPlayerCrouch -= Crouch;
        InputManager.Instance.OnPlayerCrouchCanceled -= CrouchCanceled;
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
