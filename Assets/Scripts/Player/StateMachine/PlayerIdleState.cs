using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    readonly string idleClip = "Idle";
    public PlayerIdleState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) { }
    public override void CheckSwitchState()
    {
        if (ctx.AttackPressed)
        {
            SwitchState(factory.Attack());
            return;
        }

        if (ctx.JumpPressed)
        {
            SwitchState(factory.Jump());
            return;
        }

        if (ctx.moveInput.y == -1)
        {
            SwitchState(factory.Crouch());
            return;
        }

        if (ctx.DashPressed && !ctx.DashCooldown)
        {
            SwitchState(factory.Dash());
            return;
        }

        if (ctx.JetpackPressed)
        {
            SwitchState(factory.Jetpack());
            return;
        }


        if (Mathf.Abs(ctx.moveInput.x) > 0)
        {
            SwitchState(factory.Run());
        }
    }

    public override void EnterState()
    {
        ctx.JumpCount = 0;
        ctx.Anim.Play(idleClip);
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        CheckSwitchState();
    }
}
