using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class CollisionActivator : MonoBehaviour
{
    public UnityEvent<GameObject> OnCollied;

    private void OnCollisionEnter(Collision collision)
    {

        if (!PlayerUnility.IsCollidedPlayer(collision))
        {
            Debug.Log(collision.gameObject.name);
            OnCollied.Invoke(gameObject);
        }
        else
        {
            Debug.Log("Collied with player");
        }
    }
}
