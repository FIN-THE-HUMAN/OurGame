using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBinder : MonoBehaviour
{
    [SerializeField] GameObject _camera;
    private void Update()
    {
        Vector3 pos = new Vector3(_camera.transform.position.x, transform.position.y, _camera.transform.position.z);
        transform.LookAt(pos);
    }
}
