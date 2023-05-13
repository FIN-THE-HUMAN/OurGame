using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DamageField : MonoBehaviour
{
    [SerializeField] private int _damage = 5;

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out Health health))
        {
            health.Damage(_damage);
        }
    }
}
