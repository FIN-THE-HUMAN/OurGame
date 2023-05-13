using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Extentions
{
    public static Vector3? AddY(this Vector3? vector, float y)
    {
        return new Vector3(vector.Value.x, vector.Value.y + y, vector.Value.z);
    }

    public static Vector3 AddY(this Vector3 vector, float y)
    {
        return new Vector3(vector.x, vector.y + y, vector.z);
    }

    public static Vector3 SetY(this Vector3 vector, float y)
    {
        return new Vector3(vector.x, y, vector.z);
    }

}
