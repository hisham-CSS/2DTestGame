using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchState : PlayerBaseState
{
    readonly string crouchClip = "Crouch";
    public PlayerCrouchState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) { }
    public override void CheckSwitchState()
    {
        if (ctx.moveInput.y >= 0)
        {
            SwitchState(factory.Idle());
        }

    }

    public override void EnterState()
    {
        ctx.Anim.Play(crouchClip);
    }

    public override void ExitState()
    {
       
    }

    public override void UpdateState()
    {
        CheckSwitchState();
    }

}
