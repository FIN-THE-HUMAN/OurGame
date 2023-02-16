using UnityEngine;
using static EnemyAI;

public class ChaseState : AIState
{
    private float _standartReachDistance = 1;
    public override void OnStateExit(EnemyAI enemy)
    {

    }

    public override void OnStateStart(EnemyAI enemy)
    {
        //Debug.Log("ChaseState.OnStateStart");
        enemy.SpeedToRunSpeed();
        //enemy.StartMoving();
        enemy.StartRunning();
    }

    public override void OnStateUpdate(EnemyAI enemy)
    {
        if (enemy.CanAttackTarget())
        {
            enemy.StopMoving();
            enemy.SetState(EnemyState.Attack);
        }
        else
        {
            /*if (enemy.SeeTarget())
            {
                enemy.LastTargetPosition = enemy.Target.position;
                enemy.EyeFollow(enemy.Target);
                if (enemy.CanReachTarget())
                {
                    enemy.FollowTarget();
                }
                else
                {
                    enemy.GoToNearestPoint(enemy.Target);
                }
            }
            else if (enemy.LastTargetPosition != null && Vector3.Distance(enemy.transform.position, enemy.LastTargetPosition.Value) > _standartReachDistance)
            {
                enemy.SetDestination(enemy.LastTargetPosition.Value);
            }
            else
            {
                enemy.LastTargetPosition = null;

                if (enemy.MustPatrol)
                {
                    enemy.SetState(EnemyState.Patroling);
                }
                else
                {
                    enemy.StopMoving();
                    enemy.SetState(EnemyState.Idle);
                }
            }*/
            
            //Возможно ли достичь игрока и видим ли мы его
            if (enemy.SeeTarget() && enemy.CanReachTarget())
            {

                if (!enemy.IsRunning)
                {
                    //enemy.StartMoving();
                    enemy.StartRunning();
                }

                // Нормальный метод преследовтьния игрока
                //_enemy.TrySetDestination(_enemy.Player.position);
                enemy.FollowTarget();
            }
            else
            {
                //Debug.Log("ChaseState. not enemy.CanSeePlayer() && enemy.CanReachPlayer()");
                //Если игрока нельзя достичь, то надо чилить и искать его. Или патрулировать местность
                if (enemy.MustPatrol)
                {
                    enemy.SetState(EnemyState.Patroling);
                }
                else
                {
                    enemy.StopMoving();
                    enemy.SetState(EnemyState.Idle);
                }

            }

        }
    }
}

