using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField]
    int selectedWeaponSlot = 0;
    [SerializeField]

    bool isWeaponDrawn = false;
    public GameObject[] weaponsSheathed = new GameObject[2];
    PlayerWeaponController playerWeaponController;
    private void Awake()
    {
        playerWeaponController = GetComponent<PlayerWeaponController>();      
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
        Inputs();
    }

    void Inputs()
    {
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
    }
    void TransferWeaponToHand(int weaponSlot)
    {
        if (weaponsSheathed[weaponSlot] != null)
        {
            playerWeaponController.PlaceWeaponInHand(weaponsSheathed[weaponSlot], weaponSlot);

            Destroy(weaponsSheathed[weaponSlot]);

            isWeaponDrawn = true;
        }
    }

    void SheathWeapon()
    {
        if (playerWeaponController.equippedWeapon != null)
        {
            weaponsSheathed[playerWeaponController.weaponSlot] = Instantiate(playerWeaponController.equippedWeapon, transform);
            weaponsSheathed[playerWeaponController.weaponSlot].SetActive(false);

            Destroy(playerWeaponController.equippedWeapon);

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
