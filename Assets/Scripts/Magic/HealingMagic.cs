using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class HealingMagic : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private KeyCode Heal = KeyCode.Mouse1;
    [SerializeField] private float _cooldown = 5f;
    private float lastFireTime;

    public UnityEvent OnHealed;

    private void Update()
    {
        if (Input.GetKey(Heal))
        {
            if (Time.time > lastFireTime + _cooldown)
            {
                lastFireTime = Time.time;
                AddHealingEffect(_health);
                OnHealed.Invoke();
//                Shot();
            }
        }
    }

    public void AddHealingEffect(Health health)
    {
        health.gameObject.AddComponent<Healing>();
    }
}
