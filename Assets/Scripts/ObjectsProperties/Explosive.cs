using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Explosive : MonoBehaviour
{
    [SerializeField] protected ParticleSystem _particles;
    [SerializeField] protected float _radious = 10f;
    [SerializeField] protected float _explosionForce = 10f;
    [SerializeField] protected int _damage = 500;

    protected bool _isExploded;

    public bool IsExploded => _isExploded;
    public UnityEvent<Explosive> OnExplode;

    public void Explode()
    {
        var colliders = Physics.OverlapSphere(transform.position, _radious);
        IEnumerable<GameObject> objects = colliders.Select(c => c.gameObject).Distinct();
        foreach(var c in objects)
        {
            Rigidbody rigidbody;
            if(c.TryGetComponent(out rigidbody))
            {
                rigidbody.AddExplosionForce(_explosionForce, transform.position, _radious, 1, ForceMode.Impulse);
            }

            Damagable damagable;
            if (c.TryGetComponent(out damagable))
            {
                damagable.Damage(_damage);
            }
        }
        if(_particles) Instantiate(_particles, transform.position, transform.rotation);
        _isExploded = true;
        OnExplode.Invoke(this);
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _radious);
    }
}
