using UnityEngine;

public class SeekState : AIState
{
    public override void OnStateExit(EnemyAI enemy)
    {

    }

    public override void OnStateStart(EnemyAI enemy)
    {
        enemy.LastTargetPosition = enemy.GetClosestReachablePosition(enemy.LastTargetPosition.Value);
    }

    public override void OnStateUpdate(EnemyAI enemy)
    {
        if(enemy.SeeTarget())
        {
            if (enemy.CanReachTarget())
            {
                enemy.SetState(EnemyAI.EnemyState.Chase);
            }
            else
                //enemy.SetState(EnemyAI.EnemyState.KeepEyeContacting);
                enemy.ReturnToUsualState();

        }

        if (enemy.LastTargetPosition.HasValue)
        {
            if (enemy.Reached(enemy.LastTargetPosition.Value) /*Vector3.Distance(enemy.transform.position, enemy.LastTargetPosition.Value) < enemy.StandartReachDistance*/)
            {
                //провести немного времени в поисках игрока
                Debug.Log("Противник достиг последнего места где он видел игрока");
                enemy.LastTargetPosition = null;
                enemy.ReturnToUsualState();
            }
            else
            {
                if (enemy.CanReachPosition(enemy.LastTargetPosition.Value))
                {
                    //Если поиск слишком долгий то возможно мы застряли и нужно перейти к обычному состоянию
                    enemy.SetDestination(enemy.LastTargetPosition.Value);

                    //enemy.LastTargetPosition = PositionToGroundPosition(enemy.LastTargetPosition);

                    enemy.LastTargetPosition = enemy.GetClosestReachablePosition(enemy.LastTargetPosition.Value);



                }
                else
                {
                    Debug.Log("Противник НЕ МОЖЕТ достич последнего места где он видел игрока");
                    enemy.ReturnToUsualState();

                }


            }
        }
        else
        {
            Debug.Log("Противник НЕ ИМЕЕТ координат последнего места где он видел игрока");
            enemy.ReturnToUsualState();
        }
    }
}
