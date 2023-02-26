using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class EnemyPatrolingPathPair 
{
    public EnemyAI Enemy;
    public PatrolingPath PatrolingPath;
}

[System.Serializable]
public class EnemyCombination
{
    public EnemyPatrolingPathPair EnemyPatrolingPathPair;
    public int Count;
}


public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject _cameraHolder;
    [SerializeField] private float _cooldown;
    [SerializeField] private bool _canSpawn = true;

    [SerializeField] private List<EnemyCombination> EnemyCombinations;


    private int index = 0;
    private int index2 = 0;

    public UnityEvent<EnemyAI> OnSpawned;
    public UnityEvent OnSpawnEnded;

    private void Update()
    {
        if (_canSpawn && index < EnemyCombinations.Count)
        {
            if (index2 < EnemyCombinations[index].Count)
            {
                var enemy = Instantiate(EnemyCombinations[index].EnemyPatrolingPathPair.Enemy);
                enemy.PatrolingPath = EnemyCombinations[index].EnemyPatrolingPathPair.PatrolingPath;
                if (enemy.MustPatrol) enemy.SetState(EnemyAI.EnemyState.Patroling);
                else enemy.SetState(EnemyAI.EnemyState.Idle);
                OnSpawned.Invoke(enemy);
                _canSpawn = false;
                index2++;
                Debug.Log(index2);
                StartCoroutine(ResetCooldown(_cooldown));
            }
            else
            {
                index++;
            }

            Debug.Log(index);
            index2 = 0;
        }

        if (index >= EnemyCombinations.Count) OnSpawnEnded.Invoke();
    }

    IEnumerator ResetCooldown(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        _canSpawn = true;
    }

}
