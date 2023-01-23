using static EnemyAI;

public class AttackState : AIState
{
    public AttackState(EnemyAI e) : base(e)
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
        if (_enemy.CanAttackPlayer())
        {
            // атаковать с кулдауном
        }
        else
        {
            _enemy.SetState(EnemyState.Chase);
            // догонять игрока
        }
    }
}
