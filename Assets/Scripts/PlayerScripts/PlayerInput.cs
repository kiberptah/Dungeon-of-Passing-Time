using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    WeaponManager weaponManager;
    WeaponController weaponController;

    
    private void Awake()
    {
        weaponManager = GetComponent<WeaponManager>();
        weaponController = GetComponent<WeaponController>();


    }

    private void Update()
    {
        if (Input.GetButtonDown("DrawWeapon"))
        {
            weaponManager.Input_DrawOrSheathWeapon();
        }
        if (Input.GetButtonDown("NextWeaponSlot"))
        {
            weaponManager.Input_NextSlot();
        }


    }
    private void FixedUpdate()
    {
        weaponController.UpdateMousePosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));

    }

}
