using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ExtremityProfile", menuName = "Greek Monster Pack/Extremity Profile", order = 2)]
public class ExtremityProfile : ScriptableObject
{
    [Tooltip("Material index of the mesh renderer to be affected")]
    public int meshMaterialIndex = 0;
    public List<ExtremityMaterialInformation> materialInformation;
}
[System.Serializable]
public class ExtremityMaterialInformation
{
    [Tooltip("New material based on accessory manager calculated index. \n Calculated index is: ((beast color)*(possible skin tones)+(selected skin tone)) \n \n Example: \n beast color index 2, possible skin tones 3, skin tone selection: 3 \n Calculated index = (2*3)+3 = 9")]
    public Material material;
    [Tooltip("Low index range for material. Ranges are used when multiple skin tones are available for use. \n Calculated index is: ((beast color)*(possible skin tones)+(selected skin tone)) \n \n Example: \n beast color index 2, possible skin tones 3, skin tone selection: 3 \n Calculated index = (2*3)+3 = 9")]
    public int lowIndex = 0;
    [Tooltip("high index range for material. Can be the same as low index value. Ranges are used when multiple skin tones are available for use. \n Calculated index is: ((beast color)*(possible skin tones)+(selected skin tone)) \n \n Example: \n beast color index 2, possible skin tones 3, skin tone selection: 3 \n Calculated index = (2*3)+3 = 9")]
    public int highIndex = 0;
    [Tooltip("(Optional) Used in place low and high index range for specfic index activation.")]
    public int[] indexGroup;
}
