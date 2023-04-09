using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyAI _enemy;
    [SerializeField] private float _cooldown;
    [SerializeField] private bool _canSpawn = true;
    [SerializeField] private bool _mustPatrol;
    [SerializeField] PatrolingPath _patrolingPath;

    public UnityEvent<EnemyAI> OnSpawned;
    public UnityEvent OnSpawnEnded;

    private void Update()
    {
        if (_canSpawn)
        {
            var enemy = Instantiate(_enemy, transform.position, transform.rotation);

            _enemy.MustPatrol = _mustPatrol;
            /*if (enemy.MustPatrol) */_enemy.PatrolingPath = _patrolingPath;
            _enemy.SetUsualState();

            _canSpawn = false;
            OnSpawned.Invoke(enemy);
            StartCoroutine(ResetCooldown(_cooldown));
        }
    }

    IEnumerator ResetCooldown(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        _canSpawn = true;
    }

    //private void Update()
    //{
    //    if (_canSpawn && index < EnemyCombinations.Count)
    //    {
    //        if (index2 < EnemyCombinations[index].Count)
    //        {
    //            var enemy = Instantiate(EnemyCombinations[index].EnemyPatrolingPathPair.Enemy);
    //            enemy.PatrolingPath = EnemyCombinations[index].EnemyPatrolingPathPair.PatrolingPath;
    //            if (enemy.MustPatrol) enemy.SetState(EnemyAI.EnemyState.Patroling);
    //            else enemy.SetState(EnemyAI.EnemyState.Idle);
    //            OnSpawned.Invoke(enemy);
    //            _canSpawn = false;
    //            index2++;
    //            Debug.Log(index2);
    //            StartCoroutine(ResetCooldown(_cooldown));
    //        }
    //        else
    //        {
    //            index++;
    //        }

    //        Debug.Log(index);
    //        index2 = 0;
    //    }

    //    if (index >= EnemyCombinations.Count) OnSpawnEnded.Invoke();
    //}

    //IEnumerator ResetCooldown(float cooldown)
    //{
    //    yield return new WaitForSeconds(cooldown);
    //    _canSpawn = true;
    //}

}
