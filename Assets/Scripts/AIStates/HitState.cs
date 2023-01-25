using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyAI;

public class HitState : AIState
{
    public override void OnStateExit(EnemyAI enemy)
    {

    }

    public override void OnStateStart(EnemyAI enemy)
    {
        Debug.Log("HitState.OnStateStart");
        enemy.StopMoving();
        enemy.StartCoroutine(HitCooldown(enemy));
    }

    public override void OnStateUpdate(EnemyAI enemy)
    {
       // Debug.Log("HitState.OnStateUpdate");
        // атаковать с кулдауном

    }

    private IEnumerator HitCooldown(EnemyAI enemy)
    {
        yield return new WaitForSeconds(enemy.HitCooldown);

        if (enemy.CanAttackPlayer())
        {
            Debug.Log("CanAttackPlayer");
            //enemy.StartCoroutine(enemy.GradualTurn(enemy.Player.position, 2000f));
            //enemy.StartCoroutine(enemy.QuickLookAtPlayer(0.3f));
            enemy.transform.LookAt(enemy.Player);
            enemy.SetState(EnemyState.Attack);
        }
        else if (enemy.CanSeePlayer() && enemy.CanReachPlayer())
        {
            enemy.SetState(EnemyState.Chase);
        }
        else
        {
            Debug.Log("else");
            enemy.transform.LookAt(enemy.Player);
            //enemy.StartCoroutine(enemy.QuickLookAtPlayer(0.3f));
            enemy.SetState(EnemyState.Idle);
        }
    }
}
