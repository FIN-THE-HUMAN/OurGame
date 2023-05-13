using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dismemberment : MonoBehaviour
{
    [SerializeField] private Transform _position;
    [SerializeField] private GameObject _replacement;

    [Button]
    public void Dismember()
    {        
        if (TryGetComponent(out Collider collider))
        {
            Destroy(collider);
        }

        Instantiate(_replacement, _position.position, Quaternion.identity);

        transform.localScale = Vector3.zero;
    }
}
