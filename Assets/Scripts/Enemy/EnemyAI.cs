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
    [SerializeField] private float _hitCooldown = 1f;
    [SerializeField] private EnemyAIStateSystem _enemyAIStateSystem;

    private EnemyState _currentState;


    private float _attackTimer;
    private NavMeshAgent _navMeshAgent;

    private bool isAttacking;
    private float _attackTime;

    private float visDist = 10.0f; //Distance of vision
    private float visAngle = 90.0f; //Angle of the cone vision
    private float meleeDist = 2f; //Distance from which the enemy will attack the player
    private float _rotationTime = 1;

    public bool IsMoving { get; private set; }
    public float HitCooldown => _hitCooldown;
    public Transform Player => _player;
    public UnityEvent OnAttack;
    public UnityEvent OnMovingStart;
    public UnityEvent OnMovingStop;

    public UnityEvent OnIdleStart;

    private void Awake()
    {

    }

    public void Attack()
    {
        StartCoroutine(QuickLookAtPlayer());
        OnAttack.Invoke();
    }

    public IEnumerator QuickLookAtPlayer(float time)
    {
        float localRotationTime = time;
        while (localRotationTime > 0)
        {
            Vector3 targetDirection = _player.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime/* * time*/);
            localRotationTime -= Time.deltaTime;
            yield return null;
        }
        yield return null;
    }

    public IEnumerator QuickLookAtPlayer()
    {
        return QuickLookAtPlayer(_rotationTime);
    }

    public IEnumerator GradualTurn(Vector3 endDirection, float speed)
    {
        Vector3 startDirection = transform.forward;

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * speed;
            transform.forward = Vector3.Lerp(startDirection, endDirection, t);
            yield return null;
        }
    }

    public void StartMoving()
    {
        IsMoving = true;
        OnMovingStart.Invoke();
    }

    public void StopMoving()
    {
        IsMoving = false;
        OnMovingStop.Invoke();
    }

    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _currentState = EnemyState.Idle;
    }

    private void Update()
    {
        UpdateCurrentState();
    }

    /*void Update()
    {
        // ќбновлени€ состо€ни€ противника
        //UpdateCurrentState();


        if (!PathComplete() && !IsMoving)
            StartMoving();

        if (_navMeshAgent.pathStatus != NavMeshPathStatus.PathPartial)
        {
            _navMeshAgent.SetDestination(_player.position);
        }
        else
        {
            if (IsMoving)
            {
                _navMeshAgent.SetDestination(transform.position);
                _navMeshAgent.Stop();
                IsMoving = false;

                StopMoving();
                OnIdleStart.Invoke();

            }

        }

        if (PathComplete() && IsMoving)
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
    }*/

    public void TryAttackWithCooldown()
    {
        if (Time.time > _attackTimer)
        {
            Attack();
            _attackTimer = Time.time + _attackCooldown;
        }
    }

    public bool TryFindPlayer()
    {
        if (Vector3.Distance(transform.position, _player.position) < visDist)
        {
            return true;
        }
        return false;
    }

    public void StartIdle()
    {
        _navMeshAgent.SetDestination(transform.position);
        //_navMeshAgent.Stop();
        IsMoving = false;

        StopMoving();
        OnIdleStart.Invoke();
    }

    public bool CanReachPlayer()
    {
        _navMeshAgent.SetDestination(_player.position);
        if (_navMeshAgent.pathStatus != NavMeshPathStatus.PathPartial)
        {
            _navMeshAgent.SetDestination(transform.position);
            return true;
        }
        _navMeshAgent.SetDestination(transform.position);
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

    public void FollowPlayer()
    {
        _navMeshAgent.SetDestination(_player.position);
        Debug.Log("_player.position = " + _player.position);
        Debug.Log("IsMoving = " + IsMoving);
    }

    public void SetDestination(Vector3 target)
    {
        _navMeshAgent.SetDestination(target);
    }

    public void SetState(EnemyState state)
    {
        _enemyAIStateSystem.State[_currentState].OnStateExit(this);
        _currentState = state;
        _enemyAIStateSystem.State[_currentState].OnStateStart(this);
    }

    public void SetStateHit()
    {
        SetState(EnemyState.Hit);
    }

    private void UpdateCurrentState()
    {
        //Debug.Log(_enemyAIStateSystem);
        //Debug.Log(_enemyAIStateSystem.State[_currentState]);
        _enemyAIStateSystem?.State[_currentState].OnStateUpdate(this);
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
        //See player radious
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, visDist);
    }
}
