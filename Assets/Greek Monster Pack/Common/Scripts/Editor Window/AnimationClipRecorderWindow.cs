using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

public class AnimationClipRecorderWindow : EditorWindow
{
    //Record to be affected
    AnimationClipRecord clipRecord;
    //Animation clip to read
    AnimationClip animationClip;
    //Sets the last key to be equal the first key
    bool makeCyclic = true;

    [MenuItem("Window/Greek Monster Pack/Animation Clip Recorder")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        AnimationClipRecorderWindow window = (AnimationClipRecorderWindow)EditorWindow.GetWindow(typeof(AnimationClipRecorderWindow));
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Record Animation Clip:", EditorStyles.boldLabel);
        clipRecord = (AnimationClipRecord) EditorGUILayout.ObjectField("Animation Clip Record:", clipRecord, typeof(AnimationClipRecord), false);
        animationClip = (AnimationClip)EditorGUILayout.ObjectField("Animation Clip:", animationClip, typeof(AnimationClip), false);
        makeCyclic = EditorGUILayout.Toggle("Make Cyclic:", makeCyclic);
        //Records the hip bone movement to the animation clip record
        if (GUILayout.Button("Record"))
        {
            if (clipRecord == null || animationClip == null)
            {
                Debug.LogWarning("Missing record or animation clip!");
                return;
            }
            clipRecord.animationClipName = animationClip.name;

            foreach (var binding in AnimationUtility.GetCurveBindings(animationClip))
            {
                if (binding.path + "/" + binding.propertyName == "/RootQ.x")
                {
                    clipRecord.rotCurveX = AnimationUtility.GetEditorCurve(animationClip, binding);
                    continue;
                }
                if (binding.path + "/" + binding.propertyName == "/RootQ.y")
                {
                    clipRecord.rotCurveY = AnimationUtility.GetEditorCurve(animationClip, binding);
                    continue;
                }
                if (binding.path + "/" + binding.propertyName == "/RootQ.z")
                {
                    clipRecord.rotCurveZ = AnimationUtility.GetEditorCurve(animationClip, binding);
                    continue;
                }
                if (binding.path + "/" + binding.propertyName == "/RootQ.w")
                {
                    clipRecord.rotCurveW = AnimationUtility.GetEditorCurve(animationClip, binding);
                    continue;
                }
            }

            Debug.Log("Animation Curve Recorded.");
            //Sets the last key to be equal the first key
            if (makeCyclic)
            {
                if (clipRecord.rotCurveX.keys[0].value != clipRecord.rotCurveX.keys[clipRecord.rotCurveX.keys.Length-1].value)
                {
                    Debug.Log("X Rotation Curve Adjusted");
                    clipRecord.rotCurveX.keys[clipRecord.rotCurveX.keys.Length - 1].value = clipRecord.rotCurveX.keys[0].value;
                }
                if (clipRecord.rotCurveY.keys[0].value != clipRecord.rotCurveY.keys[clipRecord.rotCurveY.keys.Length - 1].value)
                {
                    Debug.Log("Y Rotation Curve Adjusted");
                    clipRecord.rotCurveY.keys[clipRecord.rotCurveY.keys.Length - 1].value = clipRecord.rotCurveY.keys[0].value;
                }
                if (clipRecord.rotCurveZ.keys[0].value != clipRecord.rotCurveZ.keys[clipRecord.rotCurveZ.keys.Length - 1].value)
                {
                    Debug.Log("X Rotation Curve Adjusted");
                    clipRecord.rotCurveZ.keys[clipRecord.rotCurveZ.keys.Length - 1].value = clipRecord.rotCurveZ.keys[0].value;
                }
                if (clipRecord.rotCurveW.keys[0].value != clipRecord.rotCurveW.keys[clipRecord.rotCurveW.keys.Length - 1].value)
                {
                    Debug.Log("X Rotation Curve Adjusted");
                    clipRecord.rotCurveW.keys[clipRecord.rotCurveW.keys.Length - 1].value = clipRecord.rotCurveW.keys[0].value;
                }
            }
           
            EditorUtility.SetDirty(clipRecord);
            animationClip = null;
            clipRecord = null;
        }
    }
}

#endif
