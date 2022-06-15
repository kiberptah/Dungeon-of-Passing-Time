using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WeaponConnector : MonoBehaviour
{
    public float specialAttackCost = 0;
    [HideInInspector] public Weapon weaponScript;
    //[HideInInspector] public WeaponManager weaponManager;
    [HideInInspector] public ActorConnector actor;
    [HideInInspector] public Transform weaponHolder;

    public event Action<int, float> Update_Swing_Direction; // direction, maxoffset
    public event Action Special_Attack_Start;
    public event Action Special_Attack_Stop;

    public void Initialize(Vector3 _position, Transform _weaponHolder, ActorConnector _actor)
    {
        transform.position = _position;
        //Debug.Log("init position " + transform.position);

        transform.eulerAngles = new Vector3(0, 0, 0);

        weaponHolder = _weaponHolder;
        transform.parent = weaponHolder;
        //weaponManager = _manager;
        actor = _actor;

        weaponScript = GetComponent<Weapon>();
        weaponScript.Initialize();
    }

    public void Input_UpdateSwingDirection(int _direction, float _maxSwingOffset = 1)
    {
        Update_Swing_Direction?.Invoke(_direction, _maxSwingOffset);
    }
    public void Input_SpecialStart()
    {
        Special_Attack_Start?.Invoke();
    }
    public void Input_SpecialStop()
    {
        Special_Attack_Stop?.Invoke();
    }



    public void SubtractStamina(float amount)
    {
        //controller.actorStatusManager.SubtractStamina(amount);
    }
}
