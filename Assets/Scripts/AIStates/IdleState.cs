using static EnemyAI;

public class IdleState : AIState
{
    public override void OnStateExit(EnemyAI enemy)
    {

    }

    public override void OnStateStart(EnemyAI enemy)
    {
        enemy.StartIdle();
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
                //enemy.SetState(EnemyState.KeepEyeContacting);
                enemy.ReturnToUsualState();

        }
    }
}
