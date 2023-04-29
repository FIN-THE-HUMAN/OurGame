using UnityEngine;
using static EnemyAI;

public class PatrolingState : AIState
{
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
<<<<<<< Updated upstream
<<<<<<< Updated upstream
        if (enemy.SeeTarget())
=======
        if (enemy.PlayerInVisionDistanceRadious()/*enemy.SeeTarget()*/)
>>>>>>> Stashed changes
=======
        if (enemy.PlayerInVisionDistanceRadious()/*enemy.SeeTarget()*/)
>>>>>>> Stashed changes
        {
            if (enemy.CanReachTarget())
            {
                enemy.SetState(EnemyState.Chase);
            }
            else
            {
                //enemy.SetState(EnemyState.KeepEyeContacting);
                //enemy.ReturnToUsualState();
<<<<<<< Updated upstream
<<<<<<< Updated upstream
=======
=======
>>>>>>> Stashed changes

                if (!enemy.IsWalking)
                {
                    enemy.StartWalking();
                }

                enemy.SetDestination(enemy.PatrolingPath.GetTempTargetPoint());

                if (enemy.Reached(enemy.PatrolingPath.GetTempTargetPoint()) /*Vector3.Distance(enemy.transform.position, enemy.PatrolingPath.GetTempTargetPoint()) < enemy.StandartReachDistance*/)
                {
                    enemy.PatrolingPath.Next();
                }
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
            }

        }
        else
        {
            if (!enemy.IsWalking)
            {
                enemy.StartWalking();
            }

            enemy.SetDestination(enemy.PatrolingPath.GetTempTargetPoint());

            if (enemy.Reached(enemy.PatrolingPath.GetTempTargetPoint()) /*Vector3.Distance(enemy.transform.position, enemy.PatrolingPath.GetTempTargetPoint()) < enemy.StandartReachDistance*/)
            {
                enemy.PatrolingPath.Next();
            }
        }
    }
}
