public class EnemyStateFactory
{
    readonly EnemyStateMachine context;

    //player states
    EnemyHitState hitState;
    EnemyDeathState deathState;
    EnemyPatrolState patrolState;
    EnemyShieldState shieldState;
    EnemyAttackState attackState;
}
