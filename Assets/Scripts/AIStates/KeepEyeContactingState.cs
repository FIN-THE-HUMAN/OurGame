using UnityEngine;
using static EnemyAI;

public class KeepEyeContactingState : AIState
{
    public override void OnStateExit(EnemyAI enemy)
    {

    }

    public override void OnStateStart(EnemyAI enemy)
    {

    }

    public override void OnStateUpdate(EnemyAI enemy)
    {
        if (enemy.SeeTarget())
        {
            if (enemy.CanReachTarget())
            {
                enemy.SetState(EnemyState.Chase);
            }
            else
            {
                enemy.LastTargetPosition = enemy.Target.position;
                enemy.EyeFollow(enemy.Target.position);
                enemy.GoToNearestPoint(enemy.Target);
            }
        }
        else
        {
            Debug.Log("Противник НЕ ВИДИТ игрока, находящегося вне зоны его досигаемости");
            enemy.ReturnToUsualState();
        }
    }
}
