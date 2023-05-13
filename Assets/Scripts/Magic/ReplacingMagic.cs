using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ReplacingMagic : MonoBehaviour
{
    [SerializeField] private GameObject _replacer;
    [SerializeField] private float _radius;

    private void OnCollisionEnter(Collision collision)
    {

        if (!PlayerUnility.IsCollidedPlayer(collision))
        {
            var colliders = Physics.OverlapSphere(transform.position, _radius);
            IEnumerable<GameObject> objects = colliders.Select(c => c.gameObject).Distinct();

            foreach (var c in objects)
            {
                if (c.TryGetComponent(out Damagable damagable) && c.TryGetComponent(out Health health) && !c.CompareTag("Player"))
                {
                    Debug.Log(c.gameObject.name);
                    Instantiate(_replacer, damagable.transform.position, Quaternion.identity);
                    health.OnDied.Invoke(health.gameObject);
                    Destroy(damagable.gameObject);
                }
            }

            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}
