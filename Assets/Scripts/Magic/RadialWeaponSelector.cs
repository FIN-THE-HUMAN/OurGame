using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialWeaponSelector : Scrollable
{
    public List<GameObject> weapons;
    public GameObject currentWeapon;
    public int currentWeaponIndex = 0;
    void Start()
    {
        // Deactivate all weapons at the start
        foreach (GameObject weapon in weapons)
        {
            weapon.SetActive(false);
        }

        // Activate the first weapon
        if (weapons.Count > 0)
        {
            currentWeapon = weapons[currentWeaponIndex];
            currentWeapon.SetActive(true);
        }
    }

    void Update()
    {
        if (!Active) return;
        WeaponSelection();
    }

    void WeaponSelection()
    {
        float scrollValue = Input.GetAxis("Mouse ScrollWheel");

        if (scrollValue != 0)
        {
            // Deactivate the current weapon
            currentWeapon.SetActive(false);

            // Calculate the new weapon index
            if (scrollValue > 0)
            {
                currentWeaponIndex = (currentWeaponIndex + 1) % weapons.Count;
            }
            else
            {
                currentWeaponIndex -= 1;
                if (currentWeaponIndex < 0)
                {
                    currentWeaponIndex = weapons.Count - 1;
                }
            }

            // Activate the new weapon
            currentWeapon = weapons[currentWeaponIndex];
            currentWeapon.SetActive(true);
        }
    }
}
