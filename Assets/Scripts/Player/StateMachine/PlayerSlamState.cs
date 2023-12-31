using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlamState : PlayerBaseState
{
    public PlayerSlamState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) { }
    public override void CheckSwitchState()
    {
        if (ctx.IsGrounded)
        {
            SwitchState(factory.Idle());
        }
    }

    public override void EnterState()
    {
        ctx.Rb.AddForce(Vector2.down * ctx.SlamForce);
    }

    public override void ExitState()
    {
        ctx.JumpCount = 0;
    }

    public override void UpdateState()
    {
        CheckSwitchState();
    }
}
