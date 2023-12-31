using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class PlayerStateMachine : MonoBehaviour
{
    //Components
    Rigidbody2D rb;
    public Rigidbody2D Rb => rb;
    Animator anim;
    public Animator Anim => anim;
    SpriteRenderer sr;
    public SpriteRenderer Sr => sr;


    //Groundcheck stuff
    Transform groundCheck;
    [SerializeField] LayerMask isGroundLayer;
    [SerializeField] float groundCheckRadius = 0.02f;
    [SerializeField] bool isGrounded = false;
    public bool IsGrounded { get => isGrounded; }


    //Move Variables
    Coroutine speedChange = null;
    [SerializeField] float speed = 10.0f;
    float moveX;
    float inputY;
    public Vector2 moveInput { get => new Vector2(moveX, inputY); }

    //attack variables
    bool attackWindow = true;
    public bool AttackWindow { get => attackWindow; set => attackWindow = value; }

    int attackNumber = 0;
    public int AttackNumber { get => attackNumber; set => attackNumber = value; }
    bool attackPressed = false;
    public bool AttackPressed => attackPressed;


    [SerializeField] float slamForce = 0;
    public float SlamForce => slamForce;

    //Jump Variables
    [SerializeField] float jumpForce = 500.0f;
    public float JumpForce => jumpForce; 
    int jumpCount = 0;
    public int JumpCount { get => jumpCount; set => jumpCount = value; }
    [SerializeField] int maxJumps = 2;
    public int MaxJumps => maxJumps;


    //button presses
    bool jumpPressed = false;
    public bool JumpPressed => jumpPressed;

    bool dashPressed = false;
    public bool DashPressed => dashPressed;

    bool jetpackPressed;
    public bool JetpackPressed => jetpackPressed;

    //Dash Variables
    [SerializeField] float dashDistance = 40;
    public float DashDistance => dashDistance;
    
    int dashCounter = 0;
    public int DashCounter { get => dashCounter; set => dashCounter = value; }

    [SerializeField] int dashMaxCounter = 5;
    public int DashMaxCounter => dashMaxCounter;
    
    int dashCooldown = 5;
    public int DashCooldown => dashCooldown;

    //Jetpack Variables
    int jetpackFuel = 100;
    public int JetpackFuel 
    {
        get => jetpackFuel;
        set
        {
            jetpackFuel = value;
            jetpackFuelTEXT.text = jetpackFuel.ToString();
        } 
    }
    
    //Store current state and the factory that makes the states
    PlayerBaseState currentState;
    PlayerStateFactory states;

    public TMP_Text jetpackFuelTEXT;
    
    public PlayerBaseState GetCurrentState()
    {
        return currentState;
    }
    public void SetCurrentState(PlayerBaseState value)
    {
        currentState = value;
    }

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        states = new PlayerStateFactory(this);
        currentState = states.Fall();
        currentState.EnterState();

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
            obj.transform.localPosition = new Vector3(0, 0, 0);
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
        InputManager.Instance.OnPlayerDash += Dash;
        InputManager.Instance.OnPlayerJetPack += Jetpack;
    }

    private void OnDestroy()
    {
        InputManager.Instance.OnPlayerMove -= Move;
        InputManager.Instance.OnPlayerMoveCanceled -= MoveCanceled;
        InputManager.Instance.OnPlayerJump -= Jump;
        InputManager.Instance.OnPlayerCrouch -= Crouch;
        InputManager.Instance.OnPlayerCrouchCanceled -= CrouchCanceled;
        InputManager.Instance.OnPlayerAttack -= Attack;
        InputManager.Instance.OnPlayerDash -= Dash;
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

    //set our jump input
    void Jump(bool isPressed)
    {
        jumpPressed = isPressed;
    }

    //set our dash input
    void Dash(bool isPressed)
    {
        dashPressed = isPressed;
    }

    //set our attack input
    void Attack(bool isPressed)
    {
        attackPressed = isPressed;
    }

    //set our jetpack input
    void Jetpack(bool isPressed)
    {
        jetpackPressed = isPressed;
    }

    // Update is called once per frame
    void Update()
    {
        jetpackFuelTEXT.text = jetpackFuel.ToString();
      

        //isGrounded still needs to be checked every tick
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, isGroundLayer);

        //our states update function is called here to tie it to the state machines update function
        currentState.UpdateState();

        if (isGrounded && jetpackFuel < 100)
        {
            jetpackFuel++;
            //Debug.Log(jetpackFuel);
        }
    }

    void FixedUpdate()
    {
        //if we are running/jumping/falling/jetpack = we can move left or right - otherwise we do not move left or right
        rb.velocity = (currentState == states.Run() || currentState == states.Jump() || currentState == states.Fall() || currentState == states.Jetpack()) ? new Vector2(moveX, rb.velocity.y) : new Vector2(0, rb.velocity.y);
    }

    //Slam change stuff
    public void SlamSpeedChange()
    {
        if (speedChange == null)
            speedChange = StartCoroutine(SpeedChange());
    }

    IEnumerator SpeedChange()
    {
        speed *= 2;
        yield return new WaitForSeconds(1.0f);
        speed /= 2;
        speedChange = null;
    }

    //ANIMATION EVENTS THAT CANNOT BE REMOVED FROM THE STATE MACHINE
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
}
