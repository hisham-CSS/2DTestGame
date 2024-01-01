using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallingState : PlayerBaseState
{
    readonly string fallingClip = "Fall";

    public PlayerFallingState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) { }
    public override void CheckSwitchState()
    {
        if (ctx.IsGrounded)
        {
            SwitchState(factory.Idle());
            return;
        }

        if (ctx.JumpPressed)
        {
            if (ctx.JumpCount >= ctx.MaxJumps) return;
            SwitchState(factory.Jump());
            return;
        }

        if (ctx.moveInput.y == -1)
        {
            SwitchState(factory.Slam());
            return;
        }

        if (ctx.DashPressed && !ctx.DashCooldown)
        {
            SwitchState(factory.Dash());
        }
    }

    public override void EnterState()
    {
        ctx.Anim.Play(fallingClip);
    }

    public override void ExitState()
    {
    }

    public override void UpdateState()
    {
        CheckSwitchState();
    }
}
