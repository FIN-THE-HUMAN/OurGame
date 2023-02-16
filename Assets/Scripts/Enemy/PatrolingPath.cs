using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PatrolingPath
{
    [SerializeField] private List<Transform> _points = new List<Transform>();
    private int _tempPatrolingTargetPointIndex;
    public List<Transform> Points => _points;
    public Vector3 TempTargetPoint => _points[_tempPatrolingTargetPointIndex].position;

    public void Next()
    {
        if (_tempPatrolingTargetPointIndex < _points.Count - 1)
        {
            _tempPatrolingTargetPointIndex++;
        }
        else
        {
            _tempPatrolingTargetPointIndex = 0;
        }
    }

    //public Vector3 GetTempTargetPoint()
    //{
    //    return _points[_tempPatrolingTargetPointIndex].position;
    //}

    //public bool IsValid()
    //{
    //    if (_points == null || _points.Count <= 0 || _points.Contains(null)) return false;
    //    return true;
    //}
}
