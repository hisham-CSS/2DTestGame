using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    readonly string runClip = "Run";
    public PlayerRunState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) { }
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

        if(ctx.moveInput.x == 0)
        {
            SwitchState(factory.Idle());
        }
    }

    public override void EnterState()
    {
        ctx.Anim.Play(runClip);
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
