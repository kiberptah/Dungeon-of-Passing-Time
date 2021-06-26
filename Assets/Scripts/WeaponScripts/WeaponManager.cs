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
    //PlayerWeaponController playerWeaponController;
    WeaponController weaponController;
    private void Awake()
    {
        //playerWeaponController = GetComponent<PlayerWeaponController>();      
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
    private void Update()
    {

    }

    void Inputs()
    {
        /*
        if (Input.GetButtonDown("DrawWeapon"))
        {
            if (isWeaponDrawn == false)
            {            
                TransferWeaponToHand(selectedWeaponSlot);
            }
            else
            {
                SheathWeapon();
            }
        }
        if (Input.GetButtonDown("NextWeaponSlot"))
        {
            NextWeaponSlot();
        }
        */
    }
    public void Input_DrawOrSheathWeapon()
    {
        if (isWeaponDrawn == false)
        {
            Input_DrawWeapon();
        }
        else
        {
            SheathWeapon();
        }
    }
    public void Input_DrawWeapon()
    {
        TransferWeaponToHand(selectedWeaponSlot);
    }
    public void Input_SheathWeapon()
    {
        SheathWeapon();
    }
    public void Input_NextSlot()
    {
        NextWeaponSlot();
    }    



    void TransferWeaponToHand(int weaponSlot)
    {
        if (weaponsSheathed[weaponSlot] != null)
        {
            weaponController.PlaceWeaponInHand(weaponsSheathed[weaponSlot], weaponSlot);

            Destroy(weaponsSheathed[weaponSlot]);

            isWeaponDrawn = true;
        }
    }
    void SheathWeapon()
    {
        
        //if (weaponController.equippedWeapon != null)
        if (isWeaponDrawn == true)
        {
            weaponsSheathed[weaponController.weaponSlot] = Instantiate(weaponController.equippedWeapon, transform);
            weaponsSheathed[weaponController.weaponSlot].SetActive(false);

            Destroy(weaponController.equippedWeapon);

            isWeaponDrawn = false;
        }
    }
    void NextWeaponSlot()
    {
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
            SheathWeapon();
            TransferWeaponToHand(selectedWeaponSlot);
        }
    }
}
