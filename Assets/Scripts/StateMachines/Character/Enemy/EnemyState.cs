public abstract class EnemyState
{
    protected EnemyStateMachine EnemyStateMachine { get; }

    public EnemyState(EnemyStateMachine enemyStateMachine)
    {
        EnemyStateMachine = enemyStateMachine;
    }

    public abstract void Update();
    public abstract void FixedUpdate();
    public abstract void Exit();
}