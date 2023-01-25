using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyAI;

public class EnemyAIStateSystem : MonoBehaviour
{
    public Dictionary<EnemyState, AIState> State { get; private set; }

    private void Awake()
    {
        State = new Dictionary<EnemyState, AIState> {
            { EnemyState.Idle, new IdleState() },
            { EnemyState.Chase, new ChaseState() },
            { EnemyState.Attack, new AttackState() },
            { EnemyState.Hit, new HitState()}
        };
    }
}
