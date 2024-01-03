using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJetpackState : PlayerBaseState
{
    public PlayerJetpackState (PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) { }

    public override void CheckSwitchState()
    {
        //if we release jetpack button exit state
        if (!ctx.JetpackPressed || ctx.JetpackFuel <= 0)
        {           
            SwitchState(factory.Fall());
            return;   
        }

        if (ctx.DashPressed && !ctx.DashCooldown)
        {
            SwitchState(factory.Dash());
        }
    }

    public override void EnterState()
    {
        //play a jetpack animation
        ctx.Anim.Play("Idle");
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        CheckSwitchState();
        ctx.Rb.velocity = new Vector2(ctx.Rb.velocity.x, 2);
        ctx.JetpackFuel--;
        if (ctx.JetpackFuel < 0)
            ctx.JetpackFuel = 0;
    }
}
