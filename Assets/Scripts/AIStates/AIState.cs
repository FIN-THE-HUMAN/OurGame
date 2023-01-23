public abstract class AIState
{
    protected EnemyAI _enemy;

    public AIState(EnemyAI enemy)
    {
        _enemy = enemy;
    }

    public abstract void OnStateStart();

    public abstract void OnStateUpdate();

    public abstract void OnStateExit();
}
