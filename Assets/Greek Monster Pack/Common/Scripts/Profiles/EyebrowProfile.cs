using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EyebrowProfile", menuName = "Greek Monster Pack/Eyebrow Profile", order = 5)]
public class EyebrowProfile : ScriptableObject
{
    //List of eyebrow styles and actual gameobject names
    public List<EyebrowId> eyebrowIds = new List<EyebrowId>();

    //Gets actual gameobject name based on eyebrow style
    public string GetId(EyebrowType eyebrowType)
    {
        foreach (EyebrowId h in eyebrowIds)
        {
            if (h.hairType == eyebrowType) return h.id;
        }
        return "";
    }
}

//Data containing eyebrow style and actual gameobject name
[System.Serializable]
public struct EyebrowId
{
    public EyebrowType hairType;
    public string id;
}
