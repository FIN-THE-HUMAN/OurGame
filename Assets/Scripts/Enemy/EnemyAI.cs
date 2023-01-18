using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform _player;
    private NavMeshAgent _navMeshAgent;
    private bool isMoving;

    public UnityEvent OnAttack;
    public UnityEvent OnMovingStart;
    public UnityEvent OnMovingStop;

    public void Attack()
    {
        OnAttack.Invoke();
    }

    public void StartMoving()
    {
        isMoving = true;
        OnMovingStart.Invoke();
    }

    public void StopMoving()
    {
        isMoving = false;
        OnMovingStop.Invoke();
    }

    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if(!PathComplete() && !isMoving)
            StartMoving();
        
        _navMeshAgent.SetDestination(_player.position);

        if (PathComplete() && isMoving)
            StopMoving();
    }

    private bool PathComplete()
    {
        if (!_navMeshAgent.pathPending)
        {
            if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
            {
                if (_navMeshAgent.hasPath || _navMeshAgent.velocity.sqrMagnitude == 0)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _navMeshAgent == null ? 2 : _navMeshAgent.stoppingDistance);
    }
}
