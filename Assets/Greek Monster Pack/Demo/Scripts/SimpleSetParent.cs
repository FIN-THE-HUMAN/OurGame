using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Used to set weapon positions
public class SimpleSetParent : MonoBehaviour
{
    public Animator animator;
    public HumanBodyBones bone;
    public string id = "";
    public Vector3 offset = Vector3.zero;
    public Quaternion rotation = Quaternion.identity;
    public float scale = 0.44f;

    // Start is called before the first frame update
    void Start()
    {
        SetAsParent();
    }

    private void Update()
    {
        SetAsParent();
    }

    void SetAsParent()
    {
        if (animator == null)
        {
            transform.parent = null;
            return;
        }
        if (transform.parent != animator.GetBoneTransform(bone))
        transform.SetParent(animator.GetBoneTransform(bone));
        SetParentSettings[] parentSettings = transform.parent.GetComponents<SetParentSettings>();
        if (parentSettings.Length > 0)
        {
            foreach (SetParentSettings settings in parentSettings)
            {
                if (settings.id == id)
                {
                    transform.localPosition = settings.offset / 1000;
                    transform.localRotation = settings.rotation;
                    transform.localScale = Vector3.one * settings.scale;
                    return;
                }
            }
        }

        transform.localPosition = offset/1000;
        transform.localRotation = rotation;
        transform.localScale = Vector3.one * scale;
    }
}
