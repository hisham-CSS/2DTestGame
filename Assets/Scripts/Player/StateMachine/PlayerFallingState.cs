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
            ctx.JumpCount = 0;
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
        }
    }

    public override void EnterState()
    {
        ctx.Anim.Play(fallingClip);
    }

    public override void ExitState()
    {
    }

    public override void InitalizeSubState()
    {
    }

    public override void UpdateState()
    {
        CheckSwitchState();
    }
}
