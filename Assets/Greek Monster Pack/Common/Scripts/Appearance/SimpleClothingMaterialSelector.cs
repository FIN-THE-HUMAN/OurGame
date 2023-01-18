using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
[ExecuteInEditMode]
#endif
public class SimpleClothingMaterialSelector : MonoBehaviour
{
    [Tooltip("Skinned Mesh Renderer affected")]
    public SkinnedMeshRenderer myRenderer;
    [Tooltip("Usable Materials")]
    public List<Material> usableMaterials = new List<Material>();
    [Tooltip("Index of the active material in usable materials")]
    public int materialIndex = 0;
    [Tooltip("Index of the affected material in the mesh renderer")]
    public int meshIndex = 0;
    //Checks for changes
    private int lastIndex = 0;


    //Checks for changes
    private void OnValidate()
    {
        if (lastIndex == materialIndex) return;
        lastIndex = materialIndex;
        ChangeMaterial();
    }

    //Manual check for changes
    public void UpdateApp(int newIndex)
    {
        materialIndex = newIndex;
        ChangeMaterial();
    }

    //Change Material
    void ChangeMaterial()
    {
        if (materialIndex < 0 || materialIndex >= usableMaterials.Count) return;
        Material[] sharedMaterials = myRenderer.sharedMaterials;
        sharedMaterials[meshIndex] = usableMaterials[materialIndex];
        myRenderer.sharedMaterials = sharedMaterials;
    }
}
