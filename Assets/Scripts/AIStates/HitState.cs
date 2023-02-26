using System.Collections;
using UnityEngine;
using static EnemyAI;

public class HitState : AIState
{
    public override void OnStateExit(EnemyAI enemy)
    {

    }

    public override void OnStateStart(EnemyAI enemy)
    {
        enemy.StopMoving();
        enemy.StartCoroutine(HitCooldown(enemy));
    }

    public override void OnStateUpdate(EnemyAI enemy)
    {

    }

    private IEnumerator HitCooldown(EnemyAI enemy)
    {
        yield return new WaitForSeconds(enemy.HitCooldown);

        if (enemy.CanAttackTarget())
        {
            //enemy.transform.LookAt(enemy.Player);
            enemy.StartCoroutine(enemy.QuickLookAtPlayer(2));
            enemy.SetState(EnemyState.Attack);
        }
        else if (enemy.SeeTarget())
        {
            if (enemy.CanReachTarget())
            {
                enemy.SetState(EnemyState.Chase);
            }
            else
                //enemy.SetState(EnemyState.KeepEyeContacting);
                enemy.ReturnToUsualState();

        }
        else
        {
            //enemy.transform.LookAt(enemy.Player);
            enemy.StartCoroutine(enemy.QuickLookAtPlayer(2));
            enemy.SetState(EnemyState.Idle);
        }
    }
}
