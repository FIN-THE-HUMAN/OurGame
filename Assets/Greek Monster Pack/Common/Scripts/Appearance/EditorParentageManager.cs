using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;


#if UNITY_EDITOR
[ExecuteInEditMode]
#endif
public class EditorParentageManager : MonoBehaviour
{
    //Objects and there desired active state
    private List<ParentageSet> parentageSets = new List<ParentageSet>();
    //Checks it it should run a check
    bool checkParentage = false;
    [HideInInspector] //Checks for finalizing
    public bool inDestructionMode = false;
    //main hair group parent used only for finalizing
    private Transform hairParent;
    //accessory manager used only for finalizing
    private AccessoryManager manager;

    //Calls functions in update that cannot be called in the validate function
    void Update()
    {

        //Destroy acc manager, this script, and hair objects not in use
        if (inDestructionMode)
        {
            StartDestroyProcess();
            return;
        }

        //Checks for activation changes
        if (!checkParentage) return;
        foreach (ParentageSet set in parentageSets)
        {
            set.gameObject.SetActive(set.setActive);
            ArmatureExtension armatureExtension = set.gameObject.GetComponent<ArmatureExtension>();
            if (armatureExtension != null)
            {
                if (armatureExtension) armatureExtension.isUsing = set.setActive;
#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    EditorUtility.SetDirty(armatureExtension);
                }
#endif
            }
        }

        parentageSets.Clear();
        checkParentage = false;
        this.enabled = false;
    }

    //Populates change list
    public void SetParentageSet(List<ParentageSet> newParentageSets)
    {
        parentageSets = new List<ParentageSet>(newParentageSets);
        checkParentage = true;
    }
    //Finalizes hair object
    public void DestoryInactiveArmatureExtensions(Transform parent, AccessoryManager accessoryManager)
    {
        inDestructionMode = true;
        hairParent = parent;
        manager = accessoryManager;

    }

    //Destroy acc manager, this script, and hair objects not in use
    void StartDestroyProcess()
    {
        Transform[] transforms = hairParent.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in transforms)
        {
            if (t == null) continue;
            ArmatureExtension armatureExtension = t.GetComponent<ArmatureExtension>();
            if (armatureExtension)
            {
                armatureExtension.DestroySelf();
            }
        }
        ExtremityManager[] extremityManagers = manager.GetComponents<ExtremityManager>();
        foreach (ExtremityManager t in extremityManagers)
        {
            if (t == null) continue;
            DestroyImmediate(t);
        }
        DestroyImmediate(manager);
        DestroyImmediate(this);
    }
}
