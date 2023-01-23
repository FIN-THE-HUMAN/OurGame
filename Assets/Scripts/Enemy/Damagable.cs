using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class Damagable : MonoBehaviour
{
    public UnityEvent<int> OnDamaged;

    public void Damage(int damage)
    {
        OnDamaged.Invoke(damage);
    }
}
