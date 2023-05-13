using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Collider))]
public class PoisonedBomb : MonoBehaviour
{
    [SerializeField] private float _radius;

    private void OnCollisionEnter(Collision collision)
    {

        if (!PlayerUnility.IsCollidedPlayer(collision))
        {
            var colliders = Physics.OverlapSphere(transform.position, _radius);
            IEnumerable<GameObject> objects = colliders.Select(c => c.gameObject).Distinct();

            foreach (var c in objects)
            {
                if(c.TryGetComponent(out PoisonEffect poisonEffect))
                {
                    poisonEffect.AddTime(PoisonEffect.AddPoisonTime);
                }
                else
                {
                    c.gameObject.AddComponent<PoisonEffect>();
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
