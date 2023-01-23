using static EnemyAI;

public class ChaseState : AIState
{
    public ChaseState(EnemyAI e) : base(e)
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
            _enemy.SetState(EnemyState.Attack);
        }
        else
        {
            //Возможно ли достичь игрока
            if (_enemy.CanReachPosition())
            {
                // Нормальный метод преследовтьния игрока
                //_enemy.TrySetDestination(_enemy.Player.position);
            }
            else
            {
                //Если игрока нельзя достичь, то надо чилить и искать его
                _enemy.SetState(EnemyState.Idle);
            }
        }
    }
}
