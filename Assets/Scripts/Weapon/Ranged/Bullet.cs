using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class Bullet : Explosive
{
    public UnityEvent<Bullet> Collide;

    private void Start()
    {
        Destroy(gameObject, 5);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Collide.Invoke(this);
        if(!PlayerUnility.IsCollidedPlayer(collision) && !_isExploded)
        {
            Explode();
        }
    }
}
