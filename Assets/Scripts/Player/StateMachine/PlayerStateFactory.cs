public class PlayerStateFactory
{
    readonly PlayerStateMachine context;

    public PlayerJumpState jumpState;
    public PlayerFallingState fallState;
    public PlayerRunState runState;
    public PlayerSlamState slamState;
    public PlayerIdleState idleState;
    public PlayerCrouchState crouchState;
    public PlayerAttackState atttackState;

    public PlayerStateFactory(PlayerStateMachine currentContext)
    {
        context = currentContext;
        fallState = new PlayerFallingState(context, this);
        jumpState = new PlayerJumpState(context, this);
        runState = new PlayerRunState(context, this);
        slamState = new PlayerSlamState(context, this);
        idleState = new PlayerIdleState(context, this);
        crouchState = new PlayerCrouchState(context, this);
        atttackState = new PlayerAttackState(context, this);
    }
    public PlayerBaseState Falling()
    {
        return fallState;
    }

    public PlayerBaseState Jump()
    {
        return jumpState;
    }

    public PlayerBaseState Idle()
    {
        return idleState;
    }

    public PlayerBaseState Run()
    {
        return runState;
    }

    public PlayerBaseState Slam()
    {
        return slamState;
    }

    public PlayerBaseState Attack()
    {
        return atttackState;
    }

    public PlayerBaseState Crouch()
    {
        return crouchState;
    }
}
