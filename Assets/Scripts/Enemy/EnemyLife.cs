using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class EnemyLife : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var weapon = other.gameObject.GetComponent<MeleeWeapon>();
        if (weapon != null && weapon.IsAttacking)
        {
            Die();
        }

    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
