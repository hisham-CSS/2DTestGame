public abstract class EnemyBaseState
{
    protected EnemyStateMachine ctx;
    protected EnemyStateFactory factory;

    public EnemyBaseState(EnemyStateMachine currentContext, EnemyStateFactory playerStateFactory)
    {
        ctx = currentContext;
        factory = playerStateFactory;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void CheckSwitchState();


    protected void SwitchState(EnemyBaseState newState)
    {
        ExitState();
        newState.EnterState();

        ctx.SetCurrentState(newState);
    }
}