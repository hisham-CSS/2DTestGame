//Factory pattern for creating and returning all playerstates.  This is a non monobehaviour class that is used directly by the player state machine.
public class PlayerStateFactory
{
    //reference to a state machine
    readonly PlayerStateMachine context;
    
    //player states
    PlayerJumpState jumpState;
    PlayerFallingState fallState;
    PlayerRunState runState;
    PlayerSlamState slamState;
    PlayerIdleState idleState;
    PlayerCrouchState crouchState;
    PlayerAttackState atttackState;
    PlayerDashState dashState;
    PlayerJetpackState jetpackState;

    //Construtor which requires the statemachine to provide a context
    public PlayerStateFactory(PlayerStateMachine currentContext)
    {
        context = currentContext;

        //Creation of the concrete states
        fallState = new PlayerFallingState(context, this);
        jumpState = new PlayerJumpState(context, this);
        runState = new PlayerRunState(context, this);
        slamState = new PlayerSlamState(context, this);
        idleState = new PlayerIdleState(context, this);
        crouchState = new PlayerCrouchState(context, this);
        atttackState = new PlayerAttackState(context, this);
        dashState = new PlayerDashState(context, this);
        jetpackState = new PlayerJetpackState(context, this);
    }

    //Concrete State Getters - these are used by the playerstatemachine and other states to set our states
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

    public PlayerDashState Dash()
    {
        return dashState;
    }

    public PlayerJetpackState Jetpack()
    {
        return jetpackState;
    }
}
