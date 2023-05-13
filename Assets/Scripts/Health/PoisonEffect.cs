using Cinemachine.Utility;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class PoisonEffect : MonoBehaviour
{
    public const float AddPoisonTime = 3;

    [SerializeField] private int _damage = 5;
    [SerializeField] private float _rate = 0.1f;
    [SerializeField] private float _time = 3;

    private Health _targetHealth;
    //private Damager _damager = Damager.Poison;

    void Start()
    {
        _targetHealth = GetComponent<Health>();
        StartCoroutine(TimerCorutine());
        InvokeRepeating(nameof(DamageTarget), _rate - Time.deltaTime, _rate);
    }

    public void AddTime(float time)
    {
        _time += time;
    }

    //public void StartPoison()
    //{
    //    this.enabled = true;
    //}

    private void DamageTarget()
    {
        _targetHealth.Damage(_damage);
    }

    IEnumerator TimerCorutine()
    {
        while (_time > 0)
        {
            _time -= Time.deltaTime;
            yield return null;
        }

        Destroy(this);
    }

}
