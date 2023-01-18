using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EyeMaterialProfile", menuName = "Greek Monster Pack/Eye Material Profile", order = 7)]
public class EyeMaterialProfile : ScriptableObject
{
    public List<SimpleMaterialInformation> usableMaterials = new List<SimpleMaterialInformation>();
}
