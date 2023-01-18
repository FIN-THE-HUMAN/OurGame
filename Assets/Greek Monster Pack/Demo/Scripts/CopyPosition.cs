using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyPosition : MonoBehaviour
{
    [Tooltip("Position to be duplicated")]
    public Transform followedTransform;

    // Update is called once per frame
    void Update()
    {
        transform.position = followedTransform.position;
    }
}
