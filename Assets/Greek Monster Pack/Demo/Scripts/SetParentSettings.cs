using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

//Used to override simple set parent settings
public class SetParentSettings : MonoBehaviour
{
    public string id = "";
    public Vector3 offset = Vector3.zero;
    public Quaternion rotation = Quaternion.identity;
    public float scale = 0.44f;
}
