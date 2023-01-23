using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    public enum EnemyState
    {
        Idle, Patrol, Chase, Attack, Hit
    }

    [SerializeField] private Transform _player;
    [SerializeField] private float _attackCooldown = 1f;

    private EnemyState _currentState;
    private Dictionary<EnemyState, AIState> _states;

    private float _attackTimer;
    private NavMeshAgent _navMeshAgent;
    private bool isMoving;
    private bool isAttacking;
    private float _attackTime;

    private float visDist = 10.0f; //Distance of vision
    private float visAngle = 90.0f; //Angle of the cone vision
    private float meleeDist = 2f; //Distance from which the enemy will attack the player
    private float _rotationTime = 1;

    public Transform Player => _player;
    public UnityEvent OnAttack;
    public UnityEvent OnMovingStart;
    public UnityEvent OnMovingStop;

    public UnityEvent OnIdleStart;

    private void Awake()
    {
        _states = new Dictionary<EnemyState, AIState> {
            { EnemyState.Idle, new IdleState(this) },
            { EnemyState.Chase, new ChaseState(this) },
            { EnemyState.Attack, new AttackState(this) }
        };
    }

    public void Attack()
    {
        StartCoroutine(QuickLookAtPlayer());
        OnAttack.Invoke();
    }

    private IEnumerator QuickLookAtPlayer()
    {
        float localRotationTime = _rotationTime;
        while (localRotationTime > 0)
        {
            Vector3 targetDirection = _player.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * _rotationTime);
            localRotationTime -= Time.deltaTime;
            yield return null;
        }
        yield return null;
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
        // ќбновлени€ состо€ни€ противника
        //UpdateCurrentState();


        if (!PathComplete() && !isMoving)
            StartMoving();

        if (_navMeshAgent.pathStatus != NavMeshPathStatus.PathPartial)
        {
            _navMeshAgent.SetDestination(_player.position);
        }
        else
        {
            if (isMoving)
            {
                _navMeshAgent.SetDestination(transform.position);
                _navMeshAgent.Stop();
                isMoving = false;

                StopMoving();
                OnIdleStart.Invoke();

            }

        }

        if (PathComplete() && isMoving)
            StopMoving();

        if (CanAttackPlayer())
        {
            if(Time.time > _attackTimer)
            {
                Attack();
                _attackTimer = Time.time + _attackCooldown;
            }
        }

        //if(_navMeshAgent.pathStatus == NavMeshPathStatus.PathInvalid)
        //{
        //    Debug.Log("NavMeshPathStatus.PathInvalid");
        //    StopMoving();
        //}

        //if (_navMeshAgent.pathStatus == NavMeshPathStatus.PathPartial)
        //{
        //    Debug.Log("NavMeshPathStatus.PathPartial");
        //    StopMoving();
        //}
    }

    public bool TryFindPlayer()
    {
        if (Vector3.Distance(transform.position, _player.position) < visDist)
        {
            return true;
        }
        return false;
    }

    public bool CanReachPosition()
    {
        if (_navMeshAgent.pathStatus != NavMeshPathStatus.PathPartial)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Ќеправилный метод, необходимо осуществл€ть правильную проверку возможности достижени€ игрока
    /// </summary>
    /// <param name="position"></param>
    public void TrySetDestination(Vector3 position)
    {
        if (_navMeshAgent.pathStatus != NavMeshPathStatus.PathPartial)
        {
            _navMeshAgent.SetDestination(position);
        }
    }

    public void SetDestination(Vector3 target)
    {
        _navMeshAgent.SetDestination(target);
    }

    public void SetState(EnemyState state)
    {
        _currentState = state;
        _states[_currentState].OnStateStart();
    }

    private void UpdateCurrentState()
    {
        _states[_currentState].OnStateUpdate();
    }

    //public bool CanAttackPlayer()
    //{
    //    if (Vector3.Distance(transform.position, _player.position) < meleeDist)
    //    {
    //        return true;
    //    }
    //    return false;
    //}

    private bool PathComplete()
    {
        if (_navMeshAgent.pathStatus == NavMeshPathStatus.PathPartial) return true;

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

    public bool CanSeePlayer()
    {
        Vector3 direction = _player.position - transform.position;
        float angle = Vector3.Angle(direction, transform.forward);

        if (direction.magnitude < visDist && angle < visAngle)
        {
            return true;
        }
        return false;
    }

    public bool CanAttackPlayer()
    {
        Vector3 direction = _player.position - transform.position;
        if (direction.magnitude < meleeDist)
        {
            return true;
        }
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _navMeshAgent == null ? 2 : _navMeshAgent.stoppingDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeDist);
    }
}
