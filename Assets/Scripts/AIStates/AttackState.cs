using UnityEngine;
using static EnemyAI;

public class AttackState : AIState
{
    public override void OnStateExit(EnemyAI enemy)
    {

    }

    public override void OnStateStart(EnemyAI enemy)
    {
        //Debug.Log("AttackState.OnStateStart");
    }

    public override void OnStateUpdate(EnemyAI enemy)
    {
        //Debug.Log("AttackState.OnStateUpdate");
        // атаковать с кулдауном
        if (enemy.CanAttackPlayer())
        {
            //Debug.Log("AttackState. enemy.CanAttackPlayer()");
            enemy.TryAttackWithCooldown();
        }
        else
        {
            //Debug.Log("AttackState. not enemy.CanAttackPlayer()");
            enemy.SetState(EnemyState.Chase);
            // догонять игрока
        }
    }
}
