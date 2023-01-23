using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class MeleeWeapon : MonoBehaviour
{
    [SerializeField] private float _cooldown = 0.5f;
    [SerializeField] private int _damage = 100;
    private bool _canAttack = true;
    private bool _isAttacking = false;
    private List<Damagable> _JustAttackedDamagables;

    public bool IsAttacking => _isAttacking;
    public UnityEvent Attaced;

    private void Start()
    {
        _JustAttackedDamagables = new List<Damagable>(10);
    }

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
        _JustAttackedDamagables.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        var damagable = other.gameObject.GetComponent<Damagable>();
        if (damagable != null && IsAttacking && !_JustAttackedDamagables.Contains(damagable))
        {
            damagable.Damage(_damage);
            _JustAttackedDamagables.Add(damagable);
        }

    }
}
