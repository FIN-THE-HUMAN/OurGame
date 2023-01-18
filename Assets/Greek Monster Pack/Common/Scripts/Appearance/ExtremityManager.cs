using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtremityManager : MonoBehaviour
{
    [Tooltip("Mesh renderers to be affected when the accessory manager is updated")]
    public SkinnedMeshRenderer[] meshRenderers;
    [Tooltip("Contains material information for the extremities")]
    public ExtremityProfile profile;
    //used to check for changes
    private int lastMaterialIndex = -1;
    private void Awake()
    {
        AccessoryManager accessoryManager = GetComponent<AccessoryManager>();
        if (accessoryManager == null) Destroy(this);
        GetComponent<AccessoryManager>().OnApperanceUpdate.AddListener(UpdateApperance);
    }
    private void UpdateApperance(float index)
    {
        //Checks for pre reqs and changes
        if (profile == null) return;
        if (meshRenderers.Length == 0) return;
        if (lastMaterialIndex == index) return;

        //Changes the material of every mesh renderer in the list via use of the extremity profile
        foreach (ExtremityMaterialInformation eMaterialInformation in profile.materialInformation)
        {
            if (eMaterialInformation == null)
            {
                continue;
            }
            if (eMaterialInformation.indexGroup == null)
            {
                continue;
            }
            if (eMaterialInformation.indexGroup.Length == 0)
            {
                if (index >= eMaterialInformation.lowIndex && index <= eMaterialInformation.highIndex)
                {
                    foreach (SkinnedMeshRenderer skinnedMeshRenderer in meshRenderers)
                    {
                        Material[] sharedMaterials = skinnedMeshRenderer.sharedMaterials;
                        sharedMaterials[profile.meshMaterialIndex] = eMaterialInformation.material;
                        skinnedMeshRenderer.sharedMaterials = sharedMaterials;
                    }
                    return;
                }
            }
            else
            {
                foreach (int i in eMaterialInformation.indexGroup)
                {
                    if (index == i)
                    {
                        foreach (SkinnedMeshRenderer skinnedMeshRenderer in meshRenderers)
                        {
                            Material[] sharedMaterials = skinnedMeshRenderer.sharedMaterials;
                            sharedMaterials[profile.meshMaterialIndex] = eMaterialInformation.material;
                            skinnedMeshRenderer.sharedMaterials = sharedMaterials;
                        }
                        return;
                    }
                }
            }
        }
    }

    //Adds function as a listener for the acc manager
#if UNITY_EDITOR
    void OnValidate()
    {
        //Prevents editor only event problem by removing then adding
        GetComponent<AccessoryManager>().OnApperanceUpdate.RemoveListener(UpdateApperance);
        GetComponent<AccessoryManager>().OnApperanceUpdate.AddListener(UpdateApperance);
    }
#endif
}
