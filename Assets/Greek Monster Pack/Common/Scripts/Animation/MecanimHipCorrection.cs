using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class MecanimHipCorrection : MonoBehaviour
{
    [Header("Required Transforms"), Tooltip("Character animator")]
    public Animator animator;
    [Tooltip("Character hip bone transform")]
    public Transform hips;
    [Header("Settings:"), Tooltip("Layer index of the animator used for mecanim animations")]
    public int animatorMecanimLayer = 1;
    //Checks for new animation states
    private string lastAniClip;
    //Index of the animation data pulled from the profile
    private int activeRotationIndex = -1;

    [Tooltip("Mecanim Animation profile used to determine if the mecanim animation needs to be adjusted.")]
    public MecanimHipCorrectionProfile profile;
    [Tooltip("If the animator is in a transition, should the next state's information be used instead of the current state.")]
    public bool useNextAnimationInTransitions = true;
    //Adjusted speed the spine will rotate
    private float currentSpeed = 10;
    //Prevents rotation while angle differences are larger than the profile requires
    private bool freezeValues = false;
    [Tooltip("If multiple mecanim animations are playing in the same layer this value should be set to determine which animation should be used for hip correction. Setting this value to -1 will disable this setting.")]
    public int forceToIndex = -1;
    [Tooltip("Rotation calculation dampening for the character's hip bone. Changing this value may reduce jarring character movements.")]
    public float dampening = 1f;
    //Calculated hip rotation difference
    private Quaternion rotDifference = Quaternion.identity;
    //Last calculated hip rotation difference
    private Quaternion lastCalcRot;
    private void Awake()
    {
        //Sets the current speed
        currentSpeed = profile.defaultAdjustmentSpeed;
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (!Application.isPlaying) return;
        if (profile == null) return;
        //Checks if the current animation playing has changed
        CheckForNewAnimation();
        //Calculates spine compensation
        GetRotationalDifference();
    }

    //Checks if the current animation playing has changed
    void CheckForNewAnimation()
    {
        //Determines active state based on transition values
        AnimatorClipInfo[] clipInfo;
        if (useNextAnimationInTransitions && animator.IsInTransition(animatorMecanimLayer))
        {
            if (animator.GetNextAnimatorStateInfo(animatorMecanimLayer).IsName(lastAniClip)) return;
            clipInfo = animator.GetNextAnimatorClipInfo(animatorMecanimLayer);
        }
        else
        {
            if (animator.GetCurrentAnimatorStateInfo(animatorMecanimLayer).IsName(lastAniClip)) return;
            clipInfo = animator.GetCurrentAnimatorClipInfo(animatorMecanimLayer);
        }
        //Checks for null animation state
        if (clipInfo.Length == 0 && forceToIndex == -1)
        {
            activeRotationIndex = -1;
            lastAniClip = "";
            return;
        }
        //Freezes values if multiple animations are playing and force to index has not been set
        if (clipInfo.Length > 1 && forceToIndex == -1)
        {
            freezeValues = true;
        }

        if (lastAniClip == "") currentSpeed = 0;

        //Trys to get animation data from profile
        if (forceToIndex == -1)
        {
            lastAniClip = clipInfo[0].clip.name;
        }
        else
        {
            lastAniClip = clipInfo[forceToIndex].clip.name;
        }
        for (int i = 0; i < profile.animationRecordData.Count; i++)
        {
            if (profile.animationRecordData[i].clipRecord.animationClipName == lastAniClip)
            {
                activeRotationIndex = i;
                return;
            }
        }
        activeRotationIndex = -1;
    }

    //Calculates spine compensation
    void GetRotationalDifference()
    {
        //Returns and zeros speed if frozen
        if (freezeValues)
        {
            freezeValues = false;
            currentSpeed = 0;
            AdjustRotation();
            return;
        }
        //Checks if no mecanim animation is playing
        if (activeRotationIndex < 0 || activeRotationIndex >= profile.animationRecordData.Count)
        {
            AdjustRotation(Quaternion.Inverse(animator.GetBoneTransform(HumanBodyBones.Spine).localRotation));
            return;
        }

        //Gets animation state time and builds quaternion from profile
        float aniTime = Mathf.Clamp01(animator.GetCurrentAnimatorStateInfo(1).normalizedTime % 1);
        Quaternion builtRotation = profile.EvaluateClipCurve(activeRotationIndex, aniTime);
        if (lastCalcRot == Quaternion.identity) lastCalcRot = builtRotation;
        //Dampens movement if not in transition
        if (!animator.IsInTransition(animatorMecanimLayer))
        {
            builtRotation = Quaternion.Slerp(lastCalcRot, builtRotation, dampening * Time.deltaTime);
        }

        lastCalcRot = builtRotation;
        //Resets speed value at the end of cyclic animations if needed
        if (profile.animationRecordData[activeRotationIndex].cycleResetValue != -1 && aniTime>0.95f)
        {
            currentSpeed = profile.animationRecordData[activeRotationIndex].cycleResetValue;
        }
        //Gets Correction Factors
        CorrectionFactors activeCorrectionFactors = profile.correctionFactors;
        if (profile.animationRecordData[activeRotationIndex].overrideCorrectionFactors) activeCorrectionFactors = profile.animationRecordData[activeRotationIndex].overrideCorrectionFactorValues;


        //Checks for angles larger than profile limits
        if (Quaternion.Angle(hips.localRotation, builtRotation) > profile.freezeLimit)
        {
            freezeValues = true;
            return;
        }

        //Calculates spine rotation and applies correction factors
        Quaternion calcRot = hips.localRotation * Quaternion.Inverse(builtRotation);
        Vector3 eulerOffset = calcRot.eulerAngles;
        eulerOffset.x += activeCorrectionFactors.bendFactor;
        eulerOffset.y += activeCorrectionFactors.twistFactor;
        eulerOffset.z += activeCorrectionFactors.tiltFactor;
        calcRot.eulerAngles = eulerOffset;
        //Adjusts active correction speed and sets spine IK
        AdjustRotation(calcRot);
    }

    //Adjusts active correction speed and sets spine IK
    void AdjustRotation(Quaternion calcRot)
    {
        //Adjusts active correction speed
        float targetSpeed;
        if (activeRotationIndex != -1)
        {
            targetSpeed = profile.animationRecordData[activeRotationIndex].adjustmentSpeed;
        }
        else
        {
            targetSpeed = profile.defaultAdjustmentSpeed;
        }
        if (targetSpeed > currentSpeed)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, profile.speedRampFactor * Time.deltaTime);
        }
        else
        {
            currentSpeed = targetSpeed;
        }
        //Sets spine ik
        rotDifference = Quaternion.Slerp(rotDifference, calcRot, currentSpeed * Time.deltaTime);
        animator.SetBoneLocalRotation(HumanBodyBones.Spine, Quaternion.Inverse(rotDifference));
    }
    //Adjusts active correction speed and sets spine IK
    void AdjustRotation()
    {
        //Adjusts active correction speed
        float targetSpeed;
        if (activeRotationIndex != -1)
        {
            targetSpeed = profile.animationRecordData[activeRotationIndex].adjustmentSpeed;
        }
        else
        {
            targetSpeed = profile.defaultAdjustmentSpeed;
        }
        if (targetSpeed > currentSpeed)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, profile.speedRampFactor * Time.deltaTime);
        }
        else
        {
            currentSpeed = targetSpeed;
        }
        //Sets spine ik
        animator.SetBoneLocalRotation(HumanBodyBones.Spine, Quaternion.Inverse(rotDifference));
    }

    //Populates hip and animator fields if needed
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (hips == null)
        {
            Transform[] children = gameObject.GetComponentsInChildren<Transform>();
            foreach (Transform t in children)
            {
                if (t.name.Contains("Hips"))
                {
                    hips = t;
                    return;
                }
                if (t.name.Contains("hips"))
                {
                    hips = t;
                    return;
                }
            }
        } 
    }
#endif
}
