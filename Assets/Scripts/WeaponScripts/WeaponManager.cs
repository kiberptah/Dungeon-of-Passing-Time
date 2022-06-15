using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WeaponManager : MonoBehaviour
{
    ActorConnector actorConnector;
    public Transform weaponHolder;
    [HideInInspector] public GameObject weaponObject;
    [HideInInspector] public WeaponConnector weaponConnector;

    int selectedWeaponSlot = 0;
    public GameObject[] weaponsSheathed = new GameObject[2];
    [HideInInspector] public bool isWeaponDrawn = false;

    Vector3 savedWeaponPosition = Vector3.forward;

    public event Action<GameObject, WeaponConnector> updateDrawnWeapon;
    void Awake()
    {
        actorConnector = GetComponent<ActorConnector>();
    }
    #region Input
    public void Input_DrawOrSheathWeapon(Vector3? _mousePosition = null)
    {
        if (isWeaponDrawn == false)
        {
            Input_DrawWeapon(_mousePosition);
        }
        else
        {
            Input_SheathWeapon();
        }
    }
    public void Input_DrawWeapon(Vector3? _mousePosition = null)
    {
        DrawWeapon(selectedWeaponSlot, _mousePosition);
    }
    public void Input_SheathWeapon()
    {
        SheathWeapon(selectedWeaponSlot);
    }
    public void Input_NextWeaponSlot()
    {
        NextWeaponSlot();
    }

    #region Input Routing (to weapon)
    public void UpdateSwingDirection(int _direction, float _maxSwingOffset = 1)
    {
        weaponConnector.Input_UpdateSwingDirection(_direction, _maxSwingOffset);
    }
    public void SpecialAttack()
    {
        weaponConnector.Input_SpecialStart();
    }

    #endregion
    #endregion




    #region Sheath/Draw
    void DrawWeapon(int weaponSlot, Vector3? _mousePosition = null)
    {
        //Debug.Log("_mousePosition " + _mousePosition);
        //Debug.Log("actorConnector.transform.position " + actorConnector.transform.position);

        if (weaponsSheathed[weaponSlot] != null && isWeaponDrawn == false)
        {
            if (_mousePosition != null)
            {
                // if (_mousePosition == actorConnector.transform.position)
                // {
                //     _mousePosition += Vector2.up;
                // }
                savedWeaponPosition = _mousePosition.Value;
            }

            isWeaponDrawn = true;

            weaponObject = Instantiate(weaponsSheathed[weaponSlot], transform);
            weaponConnector = weaponObject.GetComponent<WeaponConnector>();
            //Debug.Log("savedWeaponPosition " + savedWeaponPosition);
            weaponConnector.Initialize(savedWeaponPosition, weaponHolder, actorConnector);

            updateDrawnWeapon?.Invoke(weaponObject, weaponConnector);
        }
    }
    void SheathWeapon(int weaponSlot)
    {
        //Debug.Log("sheath");
        //Debug.Log("isWeaponDrawn " + isWeaponDrawn);

        if (isWeaponDrawn == true)
        {
            isWeaponDrawn = false;
            savedWeaponPosition = weaponObject.transform.position;

            Destroy(weaponObject);
        }
    }
    #endregion


    void NextWeaponSlot()
    {
        int previousWeaponSlot = selectedWeaponSlot;
        selectedWeaponSlot++;
        if (selectedWeaponSlot >= weaponsSheathed.Length)
        {
            selectedWeaponSlot = 0;
        }
        if (selectedWeaponSlot < 0)
        {
            selectedWeaponSlot = weaponsSheathed.Length;
        }

        if (isWeaponDrawn)
        {
            Vector2 tempPos = weaponObject.transform.position;
            SheathWeapon(previousWeaponSlot);
            DrawWeapon(selectedWeaponSlot);
        }
    }
}
