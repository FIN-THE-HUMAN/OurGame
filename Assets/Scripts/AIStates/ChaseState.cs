using UnityEngine;
using static EnemyAI;

public class ChaseState : AIState
{
    public override void OnStateExit(EnemyAI enemy)
    {

    }

    public override void OnStateStart(EnemyAI enemy)
    {
        //Debug.Log("ChaseState.OnStateStart");
        enemy.StartMoving();
    }

    public override void OnStateUpdate(EnemyAI enemy)
    {
        //Debug.Log("ChaseState.OnStateUpdate");
        if (enemy.CanAttackPlayer())
        {
            //Debug.Log("ChaseState.CanAttackPlayer");
            enemy.StopMoving();
            enemy.SetState(EnemyState.Attack);
        }
        else
        {
            //Debug.Log("ChaseState. not CanAttackPlayer");
            //Возможно ли достичь игрока и видим ли мы его
            if (enemy.CanSeePlayer() && enemy.CanReachPlayer())
            {
                //Debug.Log("ChaseState. enemy.CanSeePlayer() && enemy.CanReachPlayer()");
                if (!enemy.IsMoving)
                {
                    enemy.StartMoving();
                }

                // Нормальный метод преследовтьния игрока
                //_enemy.TrySetDestination(_enemy.Player.position);
                enemy.FollowPlayer();
            }
            else
            {
                Debug.Log("ChaseState. not enemy.CanSeePlayer() && enemy.CanReachPlayer()");
                //Если игрока нельзя достичь, то надо чилить и искать его
                enemy.StopMoving();
                enemy.SetState(EnemyState.Idle);
            }
        }
    }
}
