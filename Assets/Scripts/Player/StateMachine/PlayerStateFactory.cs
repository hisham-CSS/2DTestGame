public class PlayerStateFactory
{
    readonly PlayerStateMachine context;
    PlayerJumpState jumpState;
    PlayerFallingState fallState;
    PlayerRunState runState;
    PlayerSlamState slamState;
    PlayerIdleState idleState;
    PlayerCrouchState crouchState;
    PlayerAttackState atttackState;

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
    public PlayerBaseState Fall()
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
