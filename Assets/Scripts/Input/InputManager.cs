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
    public event Action<bool> OnPlayerAttack;
    public event Action<bool> OnPlayerJump;
    public event Action<bool> OnPlayerDash;
    public event Action<bool> OnPlayerJetPack;
    
    
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

        input.Player.Jump.started += Jump;
        input.Player.Jump.performed += Jump;
        input.Player.Jump.canceled += Jump;

        input.Player.Attack.started += Attack;
        input.Player.Attack.performed += Attack;
        input.Player.Attack.canceled += Attack;

        input.Player.Dash.started += Dash;
        input.Player.Dash.performed += Dash;
        input.Player.Dash.canceled += Dash;

        input.Player.Jetpack.performed += JetPack;
        input.Player.Jetpack.canceled += JetPack;

    }

    private void OnDisable()
    {
        input.Disable();
        input.Player.Move.performed -= Move;
        input.Player.Move.canceled -= Move;

        input.Player.Jump.started -= Jump;
        input.Player.Jump.performed -= Jump;
        input.Player.Jump.canceled -= Jump;

        input.Player.Attack.started -= Attack;
        input.Player.Attack.performed -= Attack;
        input.Player.Attack.canceled -= Attack;

        input.Player.Dash.started -= Dash;
        input.Player.Dash.performed -= Dash;
        input.Player.Dash.canceled -= Dash;

        input.Player.Jetpack.performed -= JetPack;
        input.Player.Jetpack.canceled -= JetPack;
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    void Jump(InputAction.CallbackContext ctx)
    {
        OnPlayerJump?.Invoke(ctx.ReadValueAsButton());
    }
    
    void Attack(InputAction.CallbackContext ctx)
    {
        OnPlayerAttack?.Invoke(ctx.ReadValueAsButton());
    }
    
    void Dash(InputAction.CallbackContext ctx)
    {
        OnPlayerDash?.Invoke(ctx.ReadValueAsButton());
    }

    void JetPack(InputAction.CallbackContext ctx)
    {
        OnPlayerJetPack?.Invoke(ctx.ReadValueAsButton());
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
