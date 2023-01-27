using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private int MaxHealth = 300;
    [SerializeField] private int _healthValue;

    public UnityEvent<int, int> OnHealthChanged;
    public UnityEvent OnDamaged;
    public UnityEvent OnHealed;
    public UnityEvent<GameObject> OnDied;

    private void Start()
    {
        HealthChanged();
    }

    private void HealthChanged()
    {
        OnHealthChanged.Invoke(MaxHealth, _healthValue);
    }

    public void Damage(int damage)
    {
        int resultHealth = _healthValue - damage;
        if (_healthValue > 0)
        {
            if (resultHealth > 0) _healthValue = resultHealth;
            else
            {
                _healthValue = 0;
                OnDied.Invoke(gameObject); /*Destroy(gameObject);*/
            }

            HealthChanged();
            OnDamaged.Invoke();
        }
    }
}
