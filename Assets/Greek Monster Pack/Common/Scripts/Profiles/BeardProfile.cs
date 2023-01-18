using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BeardProfile", menuName = "Greek Monster Pack/Beard Profile", order = 4)]
public class BeardProfile : ScriptableObject
{
    //List of beard styles and actual gameobject names
    public List<BeardId> beardIds = new List<BeardId>();

    //Gets actual gameobject name based on beard style
    public string GetId(BeardType beardType)
    {
        foreach (BeardId h in beardIds)
        {
            if (h.hairType == beardType) return h.id;
        }
        return "";
    }
}

//Data containing beard style and actual gameobject name
[System.Serializable]
public struct BeardId
{
    public BeardType hairType;
    public string id;
}