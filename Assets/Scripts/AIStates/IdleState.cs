using UnityEngine;
using static EnemyAI;

public class IdleState : AIState
{
    public override void OnStateExit(EnemyAI enemy)
    {

    }

    public override void OnStateStart(EnemyAI enemy)
    {
        //Debug.Log("IdleState.OnStateStart");
        enemy.StartIdle();
    }

    public override void OnStateUpdate(EnemyAI enemy)
    {
        //if (enemy.TryFindPlayer())
        //    enemy.SetState(EnemyState.Chase);
        //Debug.Log("IdleState.OnStateUpdate");
        if (enemy.CanSeePlayer() && enemy.CanReachPlayer())
        {
            //Debug.Log("CanSeePlayer & CanReachPlayer");
            enemy.SetState(EnemyState.Chase);
        }
    }
}
