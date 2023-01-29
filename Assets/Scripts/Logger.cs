using UnityEngine;

public class Logger : MonoBehaviour
{
    public void LogPlayerDamaged(int damage)
    {
        Debug.Log("PlayerDamaged damage = " + damage);
    }

    public void LogPlayerHealth(int maxHealth, int health)
    {
        Debug.Log("Player Health = " + health);
    }

    public void LogPlayerDeath()
    {
        Debug.Log("Player Death");
    }

}
