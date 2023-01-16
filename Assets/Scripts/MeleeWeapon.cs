using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class MeleeWeapon : MonoBehaviour
{
    [SerializeField] private float _cooldown = 0.5f;
    private bool _canAttack = true;
    private bool _isAttacking = false;

    public bool IsAttacking => _isAttacking;
    public UnityEvent Attaced;

    public void OnAttack()
    {
        if (_canAttack)
        {
            _canAttack = false;
            _isAttacking = true;
            StartCoroutine(ResetCooldown());
            Attaced.Invoke();
        }
    }

    IEnumerator ResetCooldown()
    {
        yield return new WaitForSeconds(_cooldown);
        _canAttack = true;
        _isAttacking = false;
    }
}
