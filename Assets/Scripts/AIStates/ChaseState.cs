using UnityEngine;
using static EnemyAI;

public class ChaseState : AIState
{
    public override void OnStateExit(EnemyAI enemy)
    {
        Debug.Log("ChaseState.OnStateExit");
    }

    public override void OnStateStart(EnemyAI enemy)
    {
<<<<<<< Updated upstream

        Debug.Log("ChaseState.OnStateStart");
=======
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
        {    
            //Возможно ли достичь игрока и видим ли мы его
            if (enemy.SeeTarget())
=======
        {
            //Возможно ли достичь игрока и видим ли мы его
            if (enemy.PlayerInVisionDistanceRadious() /*enemy.SeeTarget()*/)
>>>>>>> Stashed changes
            {
                if (enemy.CanReachTarget())
                {
                    if (enemy.Target != null)
                    {
                        enemy.LastTargetPosition = enemy.Target.position;
                        //Debug.Log("enemy.LastTargetPosition = " + enemy.LastTargetPosition);
                        enemy.FollowTarget();
                    }
                }
                else
                {
                    /*enemy.SetState(EnemyState.KeepEyeContacting);*/
                    enemy.ReturnToUsualState();
                }
                //if (!enemy.IsRunning)
                //{
                //    //enemy.StartMoving();
                //    enemy.StartRunning();
                //}

                // Нормальный метод преследовтьния игрока
                //_enemy.TrySetDestination(_enemy.Player.position);


<<<<<<< Updated upstream
            }
            else if (enemy.LastTargetPosition.HasValue)
            {
                Debug.Log("enemy.SetState(EnemyState.Seek);");
                enemy.SetState(EnemyState.Seek);
=======
>>>>>>> Stashed changes
            }
            //else if (enemy.LastTargetPosition.HasValue)
            //{

            //    enemy.SetState(EnemyState.Seek);
            //}
            else
            {
                //Debug.Log("ChaseState. not enemy.CanSeePlayer() && enemy.CanReachPlayer()");
                //Если игрока нельзя достичь, то надо чилить и искать его. Или патрулировать местность
<<<<<<< Updated upstream
                Debug.Log("else");
                if (enemy.MustPatrol)
                {
                    enemy.SetState(EnemyState.Patroling);
                }
                else
                {
                    enemy.StopMoving();
                    enemy.SetState(EnemyState.Idle);
                }
=======

                enemy.ReturnToUsualState();
>>>>>>> Stashed changes

            }

        }
    }
}

