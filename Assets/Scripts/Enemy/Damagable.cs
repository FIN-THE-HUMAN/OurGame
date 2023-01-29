using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]

public class Damagable : MonoBehaviour
{
    public enum DamagableType
    {
        Enemy,
        Player
    }

    [SerializeField] DamagableType _type = DamagableType.Enemy;

    public DamagableType Type => _type;
    public UnityEvent<int> OnDamaged;

    public void Damage(int damage)
    {
        OnDamaged.Invoke(damage);
    }
}
