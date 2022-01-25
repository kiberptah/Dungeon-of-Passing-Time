using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ATTACH TO ACTORS NOT THE WEAPON ! ! !
/// </summary>
public class WeaponManager : MonoBehaviour
{
    int selectedWeaponSlot = 0;

    [HideInInspector] public bool isWeaponDrawn = false;

    public GameObject[] weaponsSheathed = new GameObject[2];

    WeaponController weaponController;
    private void Awake()
    {
        weaponController = GetComponent<WeaponController>();
    }
    private void Start()
    {    
        // Spawn sheathed weapons in the scene to avoid headache 
        for (int i = 0; i < weaponsSheathed.Length; ++i)
        {
            if (weaponsSheathed[i] != null)
            {
                weaponsSheathed[i] = Instantiate(weaponsSheathed[i], transform);
                weaponsSheathed[i].SetActive(false);
            }
        }        
    }

    public void Input_DrawOrSheathWeapon(Vector2 _position = new Vector2())
    {
        if (isWeaponDrawn == false)
        {
            Input_DrawWeapon(_position);
        }
        else
        {
            Input_SheathWeapon();
        }
    }
    public void Input_DrawWeapon(Vector2 _position = new Vector2())
    {
        DrawWeapon(selectedWeaponSlot, _position);
    }
    public void Input_SheathWeapon()
    {
        SheathWeapon(selectedWeaponSlot);
    }
    public void Input_NextWeaponSlot()
    {
        NextWeaponSlot();
    }    



    void DrawWeapon(int weaponSlot, Vector2 _position = new Vector2())
    {
        if (weaponsSheathed[weaponSlot] != null)
        {
            weaponController.PlaceWeaponInHand(weaponsSheathed[weaponSlot], _position);

            Destroy(weaponsSheathed[weaponSlot]);

            isWeaponDrawn = true;
        }
    }
    void SheathWeapon(int weaponSlot)
    {      
        if (isWeaponDrawn == true)
        {
            weaponsSheathed[weaponSlot] = Instantiate(weaponController.weaponObject, transform);
            weaponsSheathed[weaponSlot].SetActive(false);

            weaponController.RemoveWeapon();

            isWeaponDrawn = false;
        }
    }
    void NextWeaponSlot()
    {
        int previousWeaponSlot = selectedWeaponSlot;
        selectedWeaponSlot++;
        if (selectedWeaponSlot >= weaponsSheathed.Length)
        {
            selectedWeaponSlot = 0;
        }
        if(selectedWeaponSlot < 0)
        {
            selectedWeaponSlot = weaponsSheathed.Length;
        }

        if (isWeaponDrawn)
        {
            Vector2 tempPos = weaponController.weaponObject.transform.position;
            SheathWeapon(previousWeaponSlot);
            DrawWeapon(selectedWeaponSlot, tempPos);
        }
    }
}
