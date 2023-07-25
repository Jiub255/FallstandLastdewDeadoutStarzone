public abstract class EnemyState
{
    protected EnemyStateMachine _enemyStateMachine;

    public EnemyState(EnemyStateMachine enemyStateMachine)
    {
        _enemyStateMachine = enemyStateMachine;
    }

    public abstract void Update();
    public abstract void FixedUpdate();
    public abstract void Exit();
}