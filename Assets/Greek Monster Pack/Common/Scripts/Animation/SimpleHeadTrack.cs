using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleHeadTrack : MonoBehaviour
{
    [Tooltip("Is enabled? Use this value to smoothly transition between having a target and not having a target.")]
    public bool isEnabled = true;
    private Animator animator;
    [Tooltip("Look at target. Setting this value to null will set the weight to 0. This will not result in a smooth transition. Instead use the isEnabled value if possible.")]
    public Transform target;

    [Tooltip("Target Height Correction")]
    public float heightOffset;
    private float activeWeight = 0;
    [HideInInspector] //Calculated Head Weight
    public float targetWeight = 1;
    [Tooltip("This value is used to softly limit the heads maximum side to side movement.")]
    public float softCapFactor = 130;
    [Tooltip("Speed at which the head moves to look at a target.")]
    public float adjustmentSpeed = 5;

    private void OnAnimatorIK(int layerIndex)
    {
        //Check reqs
        if (animator == null)
        {
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                return;
            }
        }

        if (target != null)
        {
            animator.SetLookAtPosition(target.position + (Vector3.up * heightOffset));
        }
        else
        {
            activeWeight = 0;
            animator.SetLookAtWeight(activeWeight);
            return;
        }
        //Calculates and set head weight
        if (isEnabled)
        {
            if (softCapFactor != 0)
            {

                Vector3 forwardPos = animator.GetBoneTransform(HumanBodyBones.UpperChest).forward;
                forwardPos.y = 0;
                Vector3 targetPosition = target.position - animator.GetBoneTransform(HumanBodyBones.Head).position;
                targetPosition.y = 0;
                float angle = Vector3.Angle(forwardPos, targetPosition);
                targetWeight = Mathf.Clamp01(1 - (angle / softCapFactor));
            }
            else
            {
                targetWeight = 1;
            }
        }
        else
        {
            targetWeight = 0;
        }
        activeWeight = Mathf.Lerp(activeWeight, targetWeight, adjustmentSpeed * Time.deltaTime);
        animator.SetLookAtWeight(activeWeight);
    }
}
