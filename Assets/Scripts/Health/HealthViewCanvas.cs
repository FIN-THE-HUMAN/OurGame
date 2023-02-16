using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))] 
public class HealthViewCanvas : MonoBehaviour
{
    [SerializeField] Gradient _healthColors;
    private Image _image;

    private float _width;
    private float _height;

    void Awake()
    {
        _image = GetComponent<Image>();
        _width = _image.rectTransform.rect.width;
        _height = _image.rectTransform.rect.height;
    }

    public void ChangeHealthView(int maxHealth, int health)
    {
        float healthProcent = ((float)health) / maxHealth;
        //gameObject.transform.localScale = new Vector3(healthProcent, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
        //gameObject.transform.localPosition = new Vector3((1 - healthProcent) / 2 * coef, gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);
        _image.color = _healthColors.Evaluate(healthProcent);

        _image.rectTransform.sizeDelta = new Vector2(healthProcent * _width, _height);
    }
}
