using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelector : Scrollable
{
    public List<Image> weapons; // Assign the weapon icons in the Unity Inspector
    public int selectedWeaponIndex = 0;
    public float distance = 100f; // Adjust this value to set the distance between weapons
    public float rotationSpeed = 100f;

    private float anglePerWeapon;
    private RectTransform rectTransform;
    
    private void Start()
    {
        anglePerWeapon = 360f / weapons.Count;
        rectTransform = GetComponent<RectTransform>();

        for (int i = 0; i < weapons.Count; i++)
        {
            weapons[i].rectTransform.localPosition = GetWeaponPosition(anglePerWeapon * ((i + 3) % weapons.Count));
        }

        ScaleSelectedWeaponImage();
    }

    private void Update()
    {
        if (!Active) return;

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0)
        {
            if (scroll > 0) // Scroll Up
                selectedWeaponIndex = (selectedWeaponIndex - 1 + weapons.Count) % weapons.Count;
            else // Scroll Down
                selectedWeaponIndex = (selectedWeaponIndex + 1) % weapons.Count;

            ScaleSelectedWeaponImage();
            ScaleSelectedWeaponNeighboursImages();

            StartCoroutine(RotateToSelectedWeapon());
        }
    }

    private Vector2 GetWeaponPosition(float angle)
    {
        float angleInRadians = angle * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(angleInRadians) * distance, Mathf.Sin(angleInRadians) * distance);
    }

    private IEnumerator RotateToSelectedWeapon()
    {
        float targetAngle = -anglePerWeapon * selectedWeaponIndex;
        float currentAngle = rectTransform.localEulerAngles.z;
        float timeElapsed = 0;

        // Smoothly rotate the weapon wheel
        while (timeElapsed < 1)
        {
            timeElapsed += Time.deltaTime * rotationSpeed;
            float angle = Mathf.LerpAngle(currentAngle, targetAngle, timeElapsed);
            rectTransform.localEulerAngles = new Vector3(0, 0, angle);

            weapons.ForEach(e => e.transform.localEulerAngles = new Vector3(0, 0, -angle));

            yield return new WaitForEndOfFrame();
        }

        // Snap to the final position
        rectTransform.localEulerAngles = new Vector3(0, 0, targetAngle);

        // YourWeaponManager.SwitchWeapon(selectedWeaponIndex);
        // Uncomment the above line and replace it with your weapon switching function
    }

    /// <summary>
    /// ”величить размер иконки выбранного оружи€
    /// </summary>
    private void ScaleSelectedWeaponImage()
    {
        weapons[selectedWeaponIndex].transform.localScale = Vector3.one * 1.5f;
        weapons[selectedWeaponIndex].color = new Color(1, 1, 1, 1f);
    }

    /// <summary>
    /// ¬ернуть размер сосед€м иконки выбранного оружи€
    /// </summary>
    private void ScaleSelectedWeaponNeighboursImages()
    {
        weapons[Previous(selectedWeaponIndex)].transform.localScale = Vector3.one;
        weapons[Next(selectedWeaponIndex)].transform.localScale = Vector3.one;

        weapons[Previous(selectedWeaponIndex)].color = new Color(1, 1, 1, 0.5f);
        weapons[Next(selectedWeaponIndex)].color = new Color(1, 1, 1, 0.5f);
    }

    private int Previous(int index)
    {
        return index == 0 ? weapons.Count - 1 : index - 1;
    }

    private int Next(int index)
    {
        return index == weapons.Count - 1 ? 0 : index + 1;
    }
}