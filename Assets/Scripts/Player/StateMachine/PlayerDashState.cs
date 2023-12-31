using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerBaseState
{
    readonly string dashClip = "Dash";

    public PlayerDashState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) { }
    public override void CheckSwitchState()
    {
        AnimatorStateInfo stateInfo = ctx.Anim.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.normalizedTime > 1)
        {
            SwitchState(factory.Idle());
            return;
        }

        if (ctx.DashPressed)
            HandleDash();
        
    }

    public override void EnterState()
    {
        HandleDash();
    }

    public override void ExitState()
    {
        ctx.DashCounter = 0;
    }

    public override void UpdateState()
    {
        CheckSwitchState();
    }

    void HandleDash()
    {
        ctx.DashCounter++;

        if (ctx.DashCounter >= ctx.DashMaxCounter) return;

        float dir = ctx.Sr.flipX ? -1 : 1;

        ctx.Rb.velocity = new Vector2(0, 0);

        ctx.Rb.AddForce(new Vector2(ctx.DashDistance * dir, 0));

        ctx.Anim.Play(dashClip, -1, 0.0f);
    }
}
