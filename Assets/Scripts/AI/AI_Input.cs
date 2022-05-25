using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Input : MonoBehaviour
{
    AI_StateController controller;
    ActorConnector connector;


    [HideInInspector] public GameObject weaponObject;
    WeaponConnector weaponConnector;

    void OnEnable()
    {
        connector.updateDrawnWeapon += UpdateDrawnWeapon;
    }
    void OnDisable()
    {
        connector.updateDrawnWeapon -= UpdateDrawnWeapon;
    }

    void Awake()
    {
        controller = GetComponent<AI_StateController>();

        connector = controller.actor.GetComponent<ActorConnector>();
        connector.isPlayerControlled = false;
    }
    public void Input_Movement(Vector2 movementDirection)
    {
        testmovementDirection = movementDirection;

        movementDirection = movementDirection.normalized;

        connector.Input_Move(movementDirection);


        if (connector.controller != null && connector.controller != transform)
        {
            gameObject.SetActive(false);
        }
        else
        {
            connector.controller = transform;
        }
    }
    public void Input_Dash()
    {
        connector.Input_Ability();
    }

    Vector3 testmovementDirection;

    public void Input_Ability()
    {
        connector.Input_Ability();
    }

    public void UpdateDrawnWeapon(GameObject _weaponObject, WeaponConnector _weaponConnector)
    {
        weaponObject = _weaponObject;
        weaponConnector = _weaponConnector;
    }
    public void Input_DrawWeapon()
    {
        connector.Input_DrawWeapon();
    }
    public void Input_SheathWeapon()
    {
        connector.Input_SheathWeapon();
    }
    public void Input_WeaponPierce()
    {
        connector.Input_SpecialAttack();
    }
    public void Input_WeaponSwing(int swingDirection)
    {
        connector.Input_UpdateSwingDirection(swingDirection); // idk what distance should be for npc? It will be clamped anyway
    }
    public void Input_Interaction()
    {
        connector.Input_Interact();
    }


}
