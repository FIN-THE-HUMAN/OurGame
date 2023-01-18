using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
[ExecuteInEditMode]
#endif
public class ArmatureExtension : MonoBehaviour
{
    //Reference to armature bones that are not longer a child and will need to be destoryed on finalization
    public List<GameObject> bones = new List<GameObject>();
    //Keeps the object safe from finalization destruction
    public bool isUsing = false;

    //On finalization, destroys bone object and self
    public void DestroySelf(bool activeProtection = true)
    {
        if (activeProtection && isUsing) return;
        for (int i = 0; i < bones.Count; i++)
        {
            DestroyImmediate(bones[i]);
        }
        DestroyImmediate(gameObject);
    }
}
