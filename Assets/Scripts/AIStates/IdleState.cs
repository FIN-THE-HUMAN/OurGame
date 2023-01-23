using static EnemyAI;

public class IdleState : AIState
{
    public IdleState(EnemyAI e) : base(e)
    {

    }

    public override void OnStateExit()
    {

    }

    public override void OnStateStart()
    {

    }

    public override void OnStateUpdate()
    {
        if (_enemy.TryFindPlayer())
            _enemy.SetState(EnemyState.Chase);
    }
}
