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
                //�������� ������� ������� � ������� ������
                Debug.Log("��������� ������ ���������� ����� ��� �� ����� ������");
                enemy.LastTargetPosition = null;
                enemy.ReturnToUsualState();
            }
            else
            {
                if (enemy.CanReachPosition(enemy.LastTargetPosition.Value))
                {
                    //���� ����� ������� ������ �� �������� �� �������� � ����� ������� � �������� ���������
                    enemy.SetDestination(enemy.LastTargetPosition.Value);

                    //enemy.LastTargetPosition = PositionToGroundPosition(enemy.LastTargetPosition);

                    enemy.LastTargetPosition = enemy.GetClosestReachablePosition(enemy.LastTargetPosition.Value);



                }
                else
                {
                    Debug.Log("��������� �� ����� ������ ���������� ����� ��� �� ����� ������");
                    enemy.ReturnToUsualState();

                }


            }
        }
        else
        {
            Debug.Log("��������� �� ����� ��������� ���������� ����� ��� �� ����� ������");
            enemy.ReturnToUsualState();
        }
    }
}
