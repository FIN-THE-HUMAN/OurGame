using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HairProfile", menuName = "Greek Monster Pack/Hair Profile", order = 3)]
public class HairProfile : ScriptableObject
{
    //List of hair styles and actual gameobject names
    public List<HairId> hairIds = new List<HairId>();

    //Gets actual gameobject name based on hair style
    public string GetId(HairType hairType)
    {
        foreach (HairId h in hairIds)
        {
            if (h.hairType == hairType) return h.id;
        }
        return "";
    }
}

//Data containing hair style and actual gameobject name
[System.Serializable]
public struct HairId
{
    public HairType hairType;
    public string id;
}
