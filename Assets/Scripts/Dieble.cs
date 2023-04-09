using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dieble : MonoBehaviour
{
    public void DieAfterSomeTime(float t)
    {
        Destroy(gameObject, t);
    }
}
