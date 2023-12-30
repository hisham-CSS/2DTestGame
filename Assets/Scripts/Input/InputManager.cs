using System;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class InputManager : MonoBehaviour
{
    public static InputManager Instance => instance;

    private static InputManager instance;

    public event Action<float> OnPlayerMove;
    public event Action<float> OnPlayerCrouch;
    public event Action<float> OnPlayerMoveCanceled;
    public event Action<float> OnPlayerCrouchCanceled;
    public event Action OnPlayerAttack;
    public event Action OnPlayerJump;
    public event Action OnPlayerDash;
    PlayerControls input;
    

    void Awake()
    {   
        if (Instance != null)
        {
            Destroy(gameObject);
        }

        instance = this;
        DontDestroyOnLoad(this);
        input = new PlayerControls();
    }

    private void OnEnable()
    {
        input.Enable();
        input.Player.Move.performed += Move;
        input.Player.Move.canceled += Move;
        input.Player.Jump.performed += ctx => Jump();
        input.Player.Attack.performed += ctx => Attack();
        input.Player.Dash.performed += ctx => Dash();
    }

    private void OnDisable()
    {
        input.Disable();
        input.Player.Move.performed -= Move;
        input.Player.Move.canceled -= Move;
        input.Player.Jump.performed -= ctx => Jump();
        input.Player.Attack.performed -= ctx => Attack();
        input.Player.Dash.performed -= ctx => Dash();
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    void Jump()
    {
        OnPlayerJump?.Invoke();
    }
    
    void Attack()
    {
        OnPlayerAttack?.Invoke();
    }
    
    void Dash()
    {
        OnPlayerDash?.Invoke();
    }



    void Move(InputAction.CallbackContext ctx)
    {
        Vector2 moveValue = ctx.ReadValue<Vector2>();

        if (ctx.performed)
        {
            if (Mathf.Abs(moveValue.x) > 0) OnPlayerMove?.Invoke(moveValue.x);
            if (moveValue.y == -1) OnPlayerCrouch?.Invoke(moveValue.y);
        }
        
        if (moveValue.x == 0) OnPlayerMoveCanceled?.Invoke(moveValue.x);
        if (moveValue.y == 0) OnPlayerCrouchCanceled?.Invoke(moveValue.y);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
