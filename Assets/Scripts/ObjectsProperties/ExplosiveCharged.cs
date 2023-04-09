using UnityEngine;

public class ExplosiveCharged : Explosive
{
    public bool Charged;

    private void OnCollisionEnter(Collision collision)
    {
        if (Charged && !PlayerUnility.IsCollidedPlayer(collision) && !_isExploded)
        {
            Explode();
        }
    }
}
