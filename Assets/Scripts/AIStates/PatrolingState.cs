using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static EnemyAI;

public class PatrolingState : AIState
{
    private float _standartReachDistance = 1;

    public override void OnStateExit(EnemyAI enemy)
    {

    }

    public override void OnStateStart(EnemyAI enemy)
    {
        enemy.SpeedToWalkSpeed();
        enemy.StartWalking();
    }

    public override void OnStateUpdate(EnemyAI enemy)
    {
        if (enemy.SeeTarget() && enemy.CanReachTarget())
        {
            enemy.SetState(EnemyState.Chase);
        }
        else
        {
            if (!enemy.IsWalking)
            {
                enemy.StartWalking();
            }

            enemy.SetDestination(enemy.PatrolingPath.TempTargetPoint);

            if (Vector3.Distance(enemy.transform.position, enemy.PatrolingPath.TempTargetPoint) < _standartReachDistance)
            {
                enemy.PatrolingPath.Next();
            }
        }
    }
}
