using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HairMaterialProfile", menuName = "Greek Monster Pack/Hair Material Profile", order = 6)]
public class HairMaterialProfile : ScriptableObject
{
    public List<SimpleMaterialInformation> usableMaterials = new List<SimpleMaterialInformation>();
}

[System.Serializable]
public class SimpleMaterialInformation
{
    public string name = "newMaterial";
    public Material material;
}
