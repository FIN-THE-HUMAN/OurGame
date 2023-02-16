using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using NaughtyAttributes;
using System.Linq;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    public enum EnemyState
    {
        Idle, Chase, Attack, Hit, Patroling
    }

    [SerializeField] private Transform _eyePoint;
    [SerializeField] private float _hitCooldown = 1f;
    [SerializeField] private float _meleeDistance = 2f; //Distance from which the enemy will attack the player
    [SerializeField] private Damagable.DamagableType _targetType;
    [SerializeField] private float _visionDistance = 25; //Distance of vision
    [Range(0, 360)]
    [SerializeField] private float _visionAngle = 180; //Angle of the cone vision
    [SerializeField] private bool _mustPatrol;
    [ShowIf(nameof(_mustPatrol))]
    [SerializeField] private PatrolingPath _patrolingPath;
    private Transform _target;
    private EnemyAIStateSystem _enemyAIStateSystem;
    private EnemyState _currentState;
    private NavMeshAgent _navMeshAgent;
    private bool _isAttacking;
    private float _rotationTime = 1;


    public const float WALK_SPEED = 3;
    public const float RUN_SPEED = 8;

    public Vector3? LastTargetPosition = null;
    public bool IsWalking { get; private set; }
    public bool IsRunning { get; private set; } 
    public float HitCooldown => _hitCooldown;
    public bool IsAttacking => _isAttacking;
    public bool MustPatrol => _mustPatrol;
    public Transform Target => _target;
    public PatrolingPath PatrolingPath => _patrolingPath;
    public Damagable.DamagableType TargetType => _targetType;
    public float VisionDistance => _visionDistance;
    public float VisionAngle => _visionAngle;

    public UnityEvent OnAttack;
    public UnityEvent OnMovingStart;
    public UnityEvent OnRunningStart;
    public UnityEvent OnMovingStop;
    public UnityEvent OnIdleStart;

    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = WALK_SPEED;

        _target = FindObjectOfType<CharacterController>().transform;
        _enemyAIStateSystem = FindObjectOfType<EnemyAIStateSystem>();

        if (_mustPatrol)
        {
            if (_patrolingPath == null) throw new InvalidOperationException("if Enemy Set (MustPatrol = true) then PatrolingPath must not be null");
            if (_patrolingPath.Points == null) throw new InvalidOperationException("if Enemy Set (MustPatrol = true) then PatrolingPath.Points must not be null");
            if (_patrolingPath.Points.Count < 2) throw new InvalidOperationException("if Enemy Set (MustPatrol = true) then PatrolingPath must collect minimum 2 points");
            _currentState = EnemyState.Patroling;
        }
        else
        {
            _currentState = EnemyState.Idle;
        }

    }

    private void Update()
    {
        UpdateCurrentState();
    }

    public void SpeedToWalkSpeed()
    {
        _navMeshAgent.speed = WALK_SPEED;
    }

    public void SpeedToRunSpeed()
    {
        _navMeshAgent.speed = RUN_SPEED;
    }

    public void Attack()
    {
        StartCoroutine(QuickLookAtPlayer());
        OnAttack.Invoke();
    }

    public IEnumerator QuickLookAtPlayer(float speed)
    {
        float localRotationTime = 1;
        while (localRotationTime > 0)
        {
            Vector3 targetDirection = _target.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * speed/* * time*/);
            localRotationTime -= Time.deltaTime * speed;
            yield return null;
        }
        yield return null;
    }

    public IEnumerator LookPlayer(float time)
    {
        float localRotationTime = time;
        while (localRotationTime > 0)
        {
            Vector3 targetDirection = _target.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime);
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

    public void StartWalking()
    {
        IsWalking = true;
        IsRunning = false;
        OnMovingStart.Invoke();
    }

    public void StopMoving()
    {
        IsWalking = false;
        IsRunning = false;
        OnMovingStop.Invoke();

    }

    public void StartRunning()
    {
        IsWalking = false;
        IsRunning = true;
        OnRunningStart.Invoke();
    }

    public void AttackAfterWeaponCooldown()
    {
        if (!_isAttacking)
        {
            Attack();
            _isAttacking = true;
        }
    }

    public void WaitForWeaponCooldown()
    {
        _isAttacking = false;
    }

    public void StartIdle()
    {
        _navMeshAgent.SetDestination(transform.position);
        IsWalking = false;

        StopMoving();
        OnIdleStart.Invoke();
    }

    public bool CanReachTarget()
    {
        _navMeshAgent.SetDestination(_target.position);
        if (_navMeshAgent.pathStatus != NavMeshPathStatus.PathPartial)
        {
            _navMeshAgent.SetDestination(transform.position);
            return true;
        }
        _navMeshAgent.SetDestination(transform.position);
        return false;
    }

    /// <summary>
    /// Неправилный метод, необходимо осуществлять правильную проверку возможности достижения игрока
    /// </summary>
    /// <param name="position"></param>
    public void TrySetDestination(Vector3 position)
    {
        if (_navMeshAgent.pathStatus != NavMeshPathStatus.PathPartial)
        {
            _navMeshAgent.SetDestination(position);
        }
    }

    public void FollowTarget()
    {
        _navMeshAgent.SetDestination(_target.position);
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
        _enemyAIStateSystem?.State[_currentState].OnStateUpdate(this);
    }

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

    public bool SeeTarget()
    {
        //Vector3 direction = _target.position - transform.position;
        //float angle = Vector3.Angle(direction, transform.forward);

        //if (direction.magnitude < _visionDistance && angle < _visionAngle)
        //{
        //    return true;
        //}
        //return false;

        //return TrySeeTarget();
        return FieldOfViewCheck();
    }

    public bool canSeePlayer;
    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool TrySeeTarget()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, _visionDistance, targetMask);

        if (rangeChecks.Length != 0 && _target == null)
        {
            var damagables = rangeChecks.Where(e => e.GetComponent<Damagable>());
            if (damagables.Count() <= 0)
            {
                _target = null;
                return false;
            }
            //Поменять на выбор не первого, а ближайшего
            var potentialTargets = damagables.Where(d => d.GetComponent<Damagable>().Type == TargetType);
            if (potentialTargets.Count() <= 0)
            {
                _target = null;
                return false;
            }
            _target = potentialTargets.FirstOrDefault().transform;

            Vector3 directionToTarget = (_target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < _visionAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, _target.position);

                if (/*!_navMeshAgent.Raycast(target.position, out NavMeshHit hit)*/ !Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    //LastTargetPosition = _target.position;
                    canSeePlayer = true;
                }
                else
                {
                    _target = null;
                    canSeePlayer = false;
                }

            }
            else
            {
                _target = null;
                canSeePlayer = false;
            }

        }
        else if (canSeePlayer)
        {
            _target = null;
            canSeePlayer = false;
        }

        return canSeePlayer;
    }

    public void EyeFollow(Transform target)
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(_target.transform.position - target.position), Time.deltaTime);
    }

    public void GoToNearestPoint(Transform target)
    {
        NavMeshHit hit;
        _navMeshAgent.FindClosestEdge(out hit);
        _navMeshAgent.SetDestination(hit.position);
    }

    private bool FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, _visionDistance, targetMask);

        if (rangeChecks.Length != 0)
        {
            var damagables = rangeChecks.Where(e => e.GetComponent<Damagable>());
            if (damagables.Count() <= 0) return false;
            //Поменять на выбор не первого, а ближайшего
            var potentialTargets = damagables.Where(d => d.GetComponent<Damagable>().Type == TargetType);
            if (potentialTargets.Count() <= 0) return false;
            var target = potentialTargets.FirstOrDefault().transform;

            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < _visionAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (/*!_navMeshAgent.Raycast(target.position, out NavMeshHit hit)*/ !Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                    canSeePlayer = true;
                else
                    canSeePlayer = false;
            }
            else
                canSeePlayer = false;
        }
        else if (canSeePlayer)
            canSeePlayer = false;
        return canSeePlayer;
    }

    public bool CanAttackTarget()
    {
        Vector3 direction = _target.position - transform.position;
        if (direction.magnitude < _meleeDistance)
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
        Gizmos.DrawWireSphere(transform.position, _meleeDistance);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, _visionDistance);
    }
}
