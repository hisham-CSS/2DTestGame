using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class PlayerMovement : MonoBehaviour
{
    //Components
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator anim;

    //Variables
    [SerializeField] float speed = 10.0f;
    bool isMoving;
    Vector2 finalMoveInput;

    //Animation Clips
    string idleClip = "Idle";
    string runClip = "Run";

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

        InputManager.Instance.OnPlayerMove += Move;
        InputManager.Instance.OnPlayerMoveCanceled += MoveCanceled;
    }

    void Move(Vector2 moveInput)
    {
        //stop movement while in air currently
        if (rb.velocity.y < 0) return;

        isMoving = true;
        finalMoveInput = new Vector2(moveInput.x * speed, rb.velocity.y);
        sr.flipX = moveInput.x > 0 ? false : true;
    }
    
    void MoveCanceled(Vector2 moveInput)
    {
        isMoving = false;
        rb.velocity = new Vector2(0, rb.velocity.y);
        anim.Play(idleClip);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isMoving)
        {
            anim.Play(runClip);
            rb.velocity = finalMoveInput;
        }
    }
}
