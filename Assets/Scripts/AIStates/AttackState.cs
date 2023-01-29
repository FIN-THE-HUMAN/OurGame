using static EnemyAI;

public class AttackState : AIState
{
    public override void OnStateExit(EnemyAI enemy)
    {

    }

    public override void OnStateStart(EnemyAI enemy)
    {

    }

    public override void OnStateUpdate(EnemyAI enemy)
    {
        if (enemy.CanAttackPlayer())
        {
            enemy.AttackAfterWeaponCooldown();
        }
        else if(!enemy.IsAttacking)
        {
            enemy.SetState(EnemyState.Chase);
        }
    }
}
