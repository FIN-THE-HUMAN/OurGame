using System.Collections;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

#if UNITY_EDITOR
[ExecuteInEditMode]
#endif
public class AnimationBoneWanderCorrection : MonoBehaviour
{
    private Animator animator;
    private bool ragdollStatus = false;
    [SerializeField, Tooltip("All the ragdoll rigidbodies must be set to kinematic and Interpolation turned off while in the animation cycle. Otherwise, the animations will break. This script handles the ragdoll activation. Ragdolls cannot be disabled without breaking the animations. \n \n To activate the ragdoll use: AnimationBoneWanderCorrection.EnableRagdoll() \n \n This list contains all ragdoll bones in the character. To automatically fill this value, set the list count to 0.")]
    private Rigidbody[] boneRigidbodies = null;
    [Tooltip("Rigidbody interpolation mode after the ragdoll is enabled. Setting the interpolation while in the animation cycle will break the animations.")]
    public RigidbodyInterpolation rigidbodyInterpolation = RigidbodyInterpolation.None;

#if UNITY_EDITOR
    private void OnValidate()
    {
        //Finds Rigidbodies
        if (boneRigidbodies == null || boneRigidbodies.Length == 0)
        {
            boneRigidbodies = gameObject.GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody rigidbody in boneRigidbodies)
            {
                rigidbody.isKinematic = true;
                rigidbody.interpolation = RigidbodyInterpolation.None;
                EditorUtility.SetDirty(rigidbody);
            }
        }
        //Finds Animator
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }
#endif

    private void Awake()
    {
        //Finds Animator
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    //Enables the ragdoll
    public void EnableRagdoll()
    {
        foreach (Rigidbody rigidbody in boneRigidbodies)
        {
            rigidbody.isKinematic = false;
            rigidbody.interpolation = rigidbodyInterpolation;
        }
        animator.enabled = false;
        ragdollStatus=true;
    }
    //Attempts to disable the ragdoll. This will cause serious animation problems.
    public void DisableRagdoll()
    {
        for (int i = 0; i < boneRigidbodies.Length; i++)
        {
            boneRigidbodies[i].isKinematic = true;
            boneRigidbodies[i].interpolation = RigidbodyInterpolation.None;
        }
        animator.enabled = true;
        ragdollStatus = false;
    }
    //Returns true if the ragdoll is active
    public bool GetRagdollStatus()
    {
        return ragdollStatus;
    }
}
