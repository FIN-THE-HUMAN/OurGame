using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CreatureProfile", menuName = "Greek Monster Pack/Creature Profile", order = 1)]
public class CreatureProfile : ScriptableObject
{
    [Tooltip("Creature gender")]
    public CreatureSex creatureGender;
    [Tooltip("Body Material index of the mesh renderer")]
    public int meshMaterialIndex = 0;
    [Tooltip("Eye Material index of the mesh renderer")]
    public int eyeMaterialIndex = 1;
    [Tooltip("Prevents skin tone selections from being calculated for the actual body material index")]
    public bool handleSkinTonesSeperately = false;
    [Tooltip("Number of skin tones per beast color")]
    public int skinTones = 3;
    [Tooltip("Possible body materials. Organized as: \n \n (beast color 1, skin tone 1), \n (beast color 1, skin tone 2), \n (beast color 1, skin tone 3), \n (beast color 2, skin tone 1), \n ect...")]
    public List<CreatureMaterialInformation> usableMaterials = new List<CreatureMaterialInformation>();
}

[System.Serializable]
public class CreatureMaterialInformation
{
    public string name = "newMaterial";
    public Material material;
    public int skinTone = 0;
}

[System.Serializable]
public enum CreatureSex { Male, Female }

