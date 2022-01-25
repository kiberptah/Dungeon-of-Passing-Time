using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Controller : MonoBehaviour
{

    public ActorControllerConnector actor;


    GameObject weaponObject;
    Weapon weaponScript;

    void OnEnable()
    {
        actor.updateDrawnWeapon += UpdateDrawnWeapon;
    }
    void OnDisable()
    {
        actor.updateDrawnWeapon -= UpdateDrawnWeapon;
    }

    void Awake()
    {

    }
    public void Input_Movement(Vector3 movementDirection)
    {
        movementDirection = movementDirection.normalized;

        actor.Input_Move(movementDirection);
    }
    public void Input_Ability()
    {
        actor.Input_Ability();
    }

    public void UpdateDrawnWeapon(GameObject _weaponObject, Weapon _weaponScript)
    {
        weaponObject = _weaponObject;
        weaponScript = _weaponScript;
    }
    public void Input_DrawWeapon()
    {
        actor.Input_DrawWeapon();
    }
    public void Input_SheathWeapon()
    {
        actor.Input_SheathWeapon();
    }
    public void Input_WeaponPierce()
    {
        actor.Input_PiercingAttack();
    }
    public void Input_WeaponSwing(int swingDirection)
    {
        actor.Input_UpdateSwingDirection(swingDirection, 100f); // idk what distance should be for npc? It will be clamped anyway
    }
    public void Input_Interaction()
    {
        actor.Input_Interact();
    }


}
