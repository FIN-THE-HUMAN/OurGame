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
<<<<<<< Updated upstream
<<<<<<< Updated upstream
        if (enemy.SeeTarget())
        {
            if (enemy.CanReachTarget())
            {
                enemy.SetState(EnemyState.Chase);
            }
            else
                //enemy.SetState(EnemyState.KeepEyeContacting);
                enemy.ReturnToUsualState();
=======
        if (enemy.PlayerInVisionDistanceRadious() /*enemy.SeeTarget()*/)
        {
=======
        if (enemy.PlayerInVisionDistanceRadious() /*enemy.SeeTarget()*/)
        {
>>>>>>> Stashed changes
            //Debug.Log("PlayerInVisionDistanceRadious");
            if (enemy.CanReachTarget())
            {
                //Debug.Log("CanReachTarget");
                enemy.SetState(EnemyState.Chase);
            }
            //else
                //enemy.SetState(EnemyState.KeepEyeContacting);
                //enemy.ReturnToUsualState();
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes

        }
    }
}
