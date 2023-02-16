using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject _cameraHolder;
    [SerializeField] private GameObject _enemyMinotaurPrefab;
    [SerializeField] private int _enemyCount;
    [SerializeField] private float _cooldown;
    [SerializeField] private bool _canSpawn = true;

    private void Update()
    {
        if (_canSpawn && _enemyCount > 0)
        {
            Instantiate(_enemyMinotaurPrefab);
            _canSpawn = false;
            _enemyCount--;
            StartCoroutine(ResetCooldown(_cooldown));
        }
    }

    IEnumerator ResetCooldown(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        _canSpawn = true;
    }

}
