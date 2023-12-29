using System;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class InputManager : MonoBehaviour
{
    public static InputManager Instance => instance;

    private static InputManager instance;

    public event Action<Vector2> OnPlayerMove;
    public event Action<Vector2> OnPlayerMoveCanceled;

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
    }

    private void OnDisable()
    {
        input.Disable();
        input.Player.Move.performed -= Move;
        input.Player.Move.canceled -= Move;
    }
    // Start is called before the first frame update
    void Start()
    {
        //
        //input.Player.Debug.performed += MoveScene;
    }

    void Move(InputAction.CallbackContext ctx)
    {
        Vector2 moveValue = ctx.ReadValue<Vector2>();
        if (ctx.performed && Mathf.Abs(moveValue.x) > 0) OnPlayerMove?.Invoke(moveValue);
        if (ctx.canceled) OnPlayerMoveCanceled?.Invoke(moveValue);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
