using UnityEngine;

public class PlayerUnility
{
    public static bool IsCollidedPlayer(Collision collision)
    {
        return collision.gameObject.GetComponent<CharacterController>();
    }
}
