using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    readonly string jumpClip = "Jump";
    readonly string doubleJumpClip = "DoubleJump";

    public PlayerJumpState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) { }
    public override void CheckSwitchState()
    {
        if (ctx.Rb.velocity.y < 0)
        {
            SwitchState(factory.Fall());
            return;
        }

        if (ctx.moveInput.y == -1)
        {
            SwitchState(factory.Slam());
            return;
        }

        if (ctx.DashPressed)
        {
            SwitchState(factory.Dash());
            return;
        }

        if (ctx.JetpackPressed)
        {
            SwitchState(factory.Jetpack());
            return;
        }
    }

    public override void EnterState()
    {
        ctx.JumpCount++;
        //Debug.Log(ctx.JumpCount);
        if (ctx.JumpCount == 1)
            ctx.Anim.Play(jumpClip);
        if (ctx.JumpCount > 1)
            ctx.Anim.Play(doubleJumpClip);

        HandleJump();
    }

    void HandleJump()
    {
        ctx.Rb.velocity = new Vector2(ctx.Rb.velocity.x, 0);
        ctx.Rb.AddForce(Vector2.up * ctx.JumpForce);
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        CheckSwitchState();
    }
}
