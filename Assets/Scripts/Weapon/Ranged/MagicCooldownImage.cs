using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class MagicCooldownImage : MonoBehaviour
{
    [SerializeField] private float _cooldown;
    private float _remainingCooldownTime;
    private Image _image;

    private float _width;
    private float _height;

    private void Start()
    {
        _image = GetComponent<Image>();

        _width = _image.rectTransform.rect.width;
        _height = _image.rectTransform.rect.height;

        _image.rectTransform.sizeDelta = new Vector2(_width, 0);
    }

    public void CooldownShrink()
    {
        _image.rectTransform.sizeDelta = new Vector2(_width, _height);
        _remainingCooldownTime = _cooldown;
    }

    private void Update()
    {
        if (_remainingCooldownTime > 0)
        {
            _remainingCooldownTime -= Time.deltaTime;
            _image.rectTransform.sizeDelta = new Vector2(_width, _height * _remainingCooldownTime / _cooldown);
        }
    }
}
