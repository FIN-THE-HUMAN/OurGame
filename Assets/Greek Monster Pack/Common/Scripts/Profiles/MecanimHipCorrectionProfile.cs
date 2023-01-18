using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MecanimHipCorrectionProfile", menuName = "Greek Monster Pack/Mecanim Hip Correction Profile", order = 10)]
public class MecanimHipCorrectionProfile : ScriptableObject
{
    [Tooltip("Recorded animation data. See documentation for more information.")]
    public List<AnimationClipHipRotation> animationRecordData = new List<AnimationClipHipRotation>();
    [Tooltip("Default Correction Factor")]
    public CorrectionFactors correctionFactors;
    [Tooltip("Default Spine Adjustment Speed. Primary used for transitions to a null mecanim animation.")]
    public float defaultAdjustmentSpeed = 1;
    [Tooltip("Speed that the correction adjustment speed adjusts from a lower speed to a higher speed.")]
    public float speedRampFactor = 1;
    [Tooltip("Maximum correction angle limit.")]
    public float freezeLimit = 65;

    //Returns quaternion from animation clip record
    public Quaternion EvaluateClipCurve(int index, float time)
    {
        return animationRecordData[index].clipRecord.GetBuiltQuaternion(time);
    }

}

[System.Serializable]
public class AnimationClipHipRotation
{
    [Tooltip("Recorded animation data. See documentation for more information.")]
    public AnimationClipRecord clipRecord;
    [Tooltip("Spine Adjustment Speed for this animation.")]
    public float adjustmentSpeed = 5f;
    [Tooltip("Sets the adjustment speed to this value at the end of cycle animations. May be used to reduce jarring movements. Settings this value to -1 will disable this feature.")]
    public float cycleResetValue = -1;
    [Tooltip("Should this animation use different correction factors?")]
    public bool overrideCorrectionFactors = false;
    [Tooltip("Override correction factors must be set to true in order to use these values instead of the default values.")]
    public CorrectionFactors overrideCorrectionFactorValues = new CorrectionFactors();
}
[System.Serializable]
public class CorrectionFactors
{
    [Range(-90, 90)]
    public float twistFactor = 0;
    [Range(-90, 90)]
    public float tiltFactor = 0;
    [Range(-90, 90)]
    public float bendFactor = 0;
}
