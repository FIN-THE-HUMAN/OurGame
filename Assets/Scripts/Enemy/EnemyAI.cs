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
    private float _attackCooldown = 1f;
    [SerializeField] private float _hitCooldown = 1f;
    [SerializeField] private EnemyAIStateSystem _enemyAIStateSystem;

    private EnemyState _currentState;
    private float _attackTimer;
    private NavMeshAgent _navMeshAgent;

    private bool _isAttacking;
    private float _attackTime;

    private float visDist = 10.0f; //Distance of vision
    private float visAngle = 90.0f; //Angle of the cone vision
    private float meleeDist = 2f; //Distance from which the enemy will attack the player
    private float _rotationTime = 1;

    public bool IsMoving { get; private set; }
    public float HitCooldown => _hitCooldown;
    public bool IsAttacking => _isAttacking;
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

    public IEnumerator QuickLookAtPlayer(float speed)
    {
        float localRotationTime = 1;
        while (localRotationTime > 0)
        {
            Vector3 targetDirection = _player.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * speed/* * time*/);
            localRotationTime -= Time.deltaTime * speed;
            yield return null;
        }
        yield return null;
    }

    public IEnumerator LookPlayer(float time, float speedRot)
    {
        float localRotationTime = time;
        while (localRotationTime > 0)
        {
            Vector3 direction = _player.transform.position - transform.position;
            float angle = Vector3.Angle(direction, transform.forward);
            direction.y = 0;

            if (direction != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * speedRot);
            localRotationTime -= Time.deltaTime * speedRot;
            yield return new WaitForSeconds(Time.deltaTime);
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

    public void TryAttackWithCooldown()
    {
        if (Time.time > _attackTimer)
        {
            Attack();
            _isAttacking = true;
            StartCoroutine(ResetCooldown());
        }
    }

    IEnumerator ResetCooldown()
    {
        yield return new WaitForSeconds(_attackCooldown);
        _attackTimer = Time.time + _attackCooldown;
        _isAttacking = false;
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

    public void FollowPlayer()
    {
        _navMeshAgent.SetDestination(_player.position);
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
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, visDist);
    }
}
