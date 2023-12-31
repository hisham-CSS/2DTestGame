public abstract class PlayerBaseState
{
    protected PlayerStateMachine ctx;
    protected PlayerStateFactory factory;

    public PlayerBaseState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    {
        ctx = currentContext;
        factory = playerStateFactory;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void CheckSwitchState();
    public abstract void InitalizeSubState();

    void UpdateStates() { }
    
    protected void SwitchState(PlayerBaseState newState) {
        ExitState();
        newState.EnterState();

        ctx.SetCurrentState(newState);
    }

    protected void SetSuperState() { }
    protected void SetSubState() { }
}
