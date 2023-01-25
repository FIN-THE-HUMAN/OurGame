using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
//[RequireComponent(typeof(EnemyAI))]
public class Damagable : MonoBehaviour
{
    public enum DamagableType
    {
        Enemy,
        Player
    }

    [SerializeField] DamagableType _type = DamagableType.Enemy;

    public DamagableType Type => _type;
    //private EnemyAI _enemy;
    public UnityEvent<int> OnDamaged;

    private void Start()
    {
        //_enemy = GetComponent<EnemyAI>();
    }

    public void Damage(int damage)
    {
        //_enemy.SetState(EnemyAI.EnemyState.Hit);
        OnDamaged.Invoke(damage);
    }
}
