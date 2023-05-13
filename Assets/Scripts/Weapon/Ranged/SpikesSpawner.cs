using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class SpikesSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _spikes;

    private void OnCollisionEnter(Collision collision)
    {
        if (!PlayerUnility.IsCollidedPlayer(collision))
        {
            Instantiate(_spikes, transform.position, Quaternion.Euler(90, 0, 0));
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Collied with player");
        }

    }
}
