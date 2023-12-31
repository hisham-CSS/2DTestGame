using UnityEngine;
public class PlayerAttackState : PlayerBaseState
{
    string attackClip = "Attack1";
    public PlayerAttackState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) { }
    public override void CheckSwitchState()
    {
        AnimatorStateInfo stateInfo = ctx.Anim.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.normalizedTime > 1)
        {
            AnimatorClipInfo[] clipInfo = ctx.Anim.GetCurrentAnimatorClipInfo(0);
            if (clipInfo[0].clip.name.Contains(ctx.AttackNumber.ToString()))
            {
                ctx.AttackNumber = 0;
                ctx.AttackWindow = true;
                attackClip = "Attack1";
                SwitchState(factory.Idle());
            }
            else
            {
                //2 or 3 to the end of the attackClip string
                attackClip = "Attack" + ctx.AttackNumber.ToString();
                ctx.Anim.Play(attackClip);
            }
        }
    }

    public override void EnterState()
    {
        ctx.Anim.Play(attackClip);
        ctx.AttackNumber++;
        ctx.AttackWindow = false;
    }

    public override void ExitState()
    {
       
    }

    public override void UpdateState()
    {
        CheckSwitchState();

        if (ctx.AttackWindow && ctx.AttackPressed)
        {
            AnimatorClipInfo[] clipInfo = ctx.Anim.GetCurrentAnimatorClipInfo(0);

            //if we are in attack1, we need to move to the second attack
            if (clipInfo[0].clip.name == "Attack1")
                ctx.AttackNumber = 2;

            //if we are in attack2, we need to move into the third attack
            if (clipInfo[0].clip.name == "Attack2")
                ctx.AttackNumber = 3;

            ctx.AttackWindow = false;
        }
    }

}
