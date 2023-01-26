using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logger : MonoBehaviour
{
    public void LogPlayerDamaged(int damage)
    {
        Debug.Log("PlayerDamaged damage = " + damage);
    }
}
