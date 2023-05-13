using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healing : MonoBehaviour
{
    [SerializeField] private int _heal = 5;
    [SerializeField] private float _rate = 0.1f;
    [SerializeField] private float _time = 3;

    private Health _targetHealth;
    //private Damager _damager = Damager.Poison;

    void Start()
    {
        _targetHealth = GetComponent<Health>();
        StartCoroutine(TimerCorutine());
        InvokeRepeating(nameof(HealTarget), _rate - Time.deltaTime, _rate);
    }

    public void AddTime(float time)
    {
        _time += time;
    }

    private void HealTarget()
    {
        _targetHealth.Heal(_heal);
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
