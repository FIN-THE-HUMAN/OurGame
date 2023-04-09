using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temporary : MonoBehaviour
{
    [SerializeField] private float _lifeTime = 1f;
    void Start()
    {
        Destroy(gameObject, _lifeTime);
    }

}
