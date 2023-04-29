using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]  
public class SliderValueFromSoundsProvider : MonoBehaviour
{
    [SerializeField] private string _key;
    private Slider _slider;

    void Start()
    {
        _slider = GetComponent<Slider>();

        _slider.value = PlayerPrefs.GetFloat(_key, 0f);
    }
}
