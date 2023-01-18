using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimationClipRecord", menuName = "Greek Monster Pack/Animation Clip Record", order = 9)]
public class AnimationClipRecord : ScriptableObject
{
    [Tooltip("This name must exactly match the animation clip name.")]
    public string animationClipName= "";
    [Tooltip("Recorded X rotation curve of the Hip bone.")]
    public AnimationCurve rotCurveX;
    [Tooltip("Recorded Y rotation curve of the Hip bone.")]
    public AnimationCurve rotCurveY;
    [Tooltip("Recorded Z rotation curve of the Hip bone.")]
    public AnimationCurve rotCurveZ;
    [Tooltip("Recorded W rotation curve of the Hip bone.")]
    public AnimationCurve rotCurveW;

    //Creates a quaternio from the rotation curves for a given time
    public Quaternion GetBuiltQuaternion(float time)
    {
        return new Quaternion(rotCurveX.Evaluate(time),
            rotCurveY.Evaluate(time),
            rotCurveZ.Evaluate(time),
            rotCurveW.Evaluate(time));
    }
}
