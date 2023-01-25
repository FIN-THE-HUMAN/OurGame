public abstract class AIState
{
    public abstract void OnStateStart(EnemyAI enemy);

    public abstract void OnStateUpdate(EnemyAI enemy);

    public abstract void OnStateExit(EnemyAI enemy);
}
