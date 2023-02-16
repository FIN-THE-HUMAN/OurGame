using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

public class Health : MonoBehaviour
{
    [SerializeField] private int MaxHealth = 300;

    [ProgressBar("Health", nameof(MaxHealth), EColor.Green)]
    [SerializeField] private int _healthValue;

    public UnityEvent<int, int> OnHealthChanged;
    public UnityEvent OnDamaged;
    public UnityEvent OnHealed;
    public UnityEvent<GameObject> OnDied;

    private void Start()
    {
        _healthValue = MaxHealth;
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
                OnDied.Invoke(gameObject);
            }

            HealthChanged();
            OnDamaged.Invoke();
        }
    }
}
