using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{
    [SerializeField] private float _start;
    [SerializeField] private float _end;
    [SerializeField] private float _speed = 1;

    public void Move()
    {
        Debug.Log("Move");
        StartCoroutine(DoMove());
    }

    public IEnumerator DoMove()
    {
        while (transform.localPosition.y < _end)
        {
            transform.localPosition = transform.localPosition.AddY(Time.deltaTime * _speed);
            yield return null;
        }
    }

    public void Back()
    {
        transform.localPosition = transform.localPosition.SetY(_start);
    }

}
