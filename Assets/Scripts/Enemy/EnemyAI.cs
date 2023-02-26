using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using System.Linq;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    public enum EnemyState
    {
        Idle, Chase, Attack, Hit, Patroling, JustSpawned, Seek, KeepEyeContacting
    }

    [SerializeField] private Transform _eyePoint;
    [SerializeField] private float _hitCooldown = 1f;
    [SerializeField] private float _meleeDistance = 2f; //Distance from which the enemy will attack the player
    [SerializeField] private Damagable.DamagableType _targetType;
    [SerializeField] private float _visionDistance = 25; //Distance of vision
    [Range(0, 360)]
    [SerializeField] private float _visionAngle = 180; //Angle of the cone vision
    [SerializeField] private bool _mustPatrol;
    [SerializeField] private float _standartReachDistance = 2;

    private Transform _target;
    private EnemyAIStateSystem _enemyAIStateSystem;
    private NavMeshAgent _navMeshAgent;
    private bool _isAttacking;
    private float _rotationTime = 1;

    public const float WALK_SPEED = 3;
    public const float RUN_SPEED = 8;

    public EnemyState _currentState;
    public Vector3? LastTargetPosition = null;
    public bool IsWalking { get; private set; }
    public bool IsRunning { get; private set; } 
    public float HitCooldown => _hitCooldown;
    public bool IsAttacking => _isAttacking;
    public bool MustPatrol => _mustPatrol;
    public Transform Target => _target;
    public PatrolingPath PatrolingPath;
    public Damagable.DamagableType TargetType => _targetType;
    public float VisionDistance => _visionDistance;
    public float VisionAngle => _visionAngle;

    public float StandartReachDistance => _standartReachDistance;
    public Vector3 Center => transform.position.AddY(_navMeshAgent.height / 2);

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

        if (_currentState == EnemyState.JustSpawned) return;

        if (_mustPatrol)
        {
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

    public bool Reached(Vector3 position)
    {
        return Vector3.Distance(transform.position, position) < _standartReachDistance || 
            Vector3.Distance(Center, position) < _standartReachDistance;
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
        Debug.Log("StartRunning"); 
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
        return CanReachPosition(_target.position);
    }

    public bool CanReachPosition(Vector3 position)
    {
        Vector3 targetPos = position;
        NavMeshPath path = new NavMeshPath();
        _navMeshAgent.CalculatePath(targetPos, path);
        return (path.status == NavMeshPathStatus.PathComplete);

    }

    public void FollowTarget()
    {
        SetDestination(_target.position);
    }

    public void SetDestination(Vector3 target)
    {
        _navMeshAgent.SetDestination(target);
    }

    public void SetState(EnemyState state)
    {
        _enemyAIStateSystem?.State[_currentState]?.OnStateExit(this);
        _currentState = state;
        _enemyAIStateSystem?.State[_currentState]?.OnStateStart(this);
    }

    public void SetStateHit()
    {
        SetState(EnemyState.Hit);
    }

    private void UpdateCurrentState()
    {
        _enemyAIStateSystem?.State[_currentState].OnStateUpdate(this);
    }

    public Vector3 GetClosestReachablePosition(Vector3 position)
    {
        NavMeshHit hit;
        if(NavMesh.SamplePosition(position, out hit, _standartReachDistance, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return position;
    }

    public bool SeeTarget()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, _visionDistance, targetMask);

        if (rangeChecks.Length != 0)
        {
            var damagables = rangeChecks.Where(e => e.GetComponent<Damagable>());
            if (damagables.Count() <= 0)
            {
                Debug.Log("1");
                return false;
            }
            //Поменять на выбор не первого, а ближайшего
            var potentialTargets = damagables.Where(d => d.GetComponent<Damagable>().Type == TargetType);
            if (potentialTargets.Count() <= 0)
            {
                Debug.Log("2");
                return false;
            }
            var target = potentialTargets.FirstOrDefault().transform;

            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < _visionAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(_eyePoint.transform.position, target.position);

                if (!Physics.Raycast(_eyePoint.transform.position, directionToTarget, distanceToTarget, obstructionMask))
                    canSeePlayer = true;
                else
                {
                    Debug.Log("3");
                    canSeePlayer = false;
                }
            }
            else
            {
                Debug.Log("4");
                canSeePlayer = false;
            }
        }
        else if (canSeePlayer)
        {
            Debug.Log("5");
            canSeePlayer = false;
        }
        if(!canSeePlayer) Debug.Log("6");
        return canSeePlayer;
    }

    public bool canSeePlayer;
    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public void EyeFollow(Vector3 target)
    {
        transform.LookAt(target.SetY(transform.position.y)); 
    }

    public void GoToNearestPoint(Transform target)
    {
        NavMeshHit hit;
        _navMeshAgent.FindClosestEdge(out hit);
        _navMeshAgent.SetDestination(hit.position);
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

    public void ReturnToUsualState()
    {
        if (MustPatrol)
        {
            SetState(EnemyState.Patroling);
        }
        else
        {
            StopMoving();
            SetState(EnemyState.Idle);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _navMeshAgent == null ? 2 : _navMeshAgent.stoppingDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _meleeDistance);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, _visionDistance);
        if(_currentState == EnemyState.Seek)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, _standartReachDistance);
            Gizmos.DrawWireSphere(Center, _standartReachDistance);
        }
    }
}
