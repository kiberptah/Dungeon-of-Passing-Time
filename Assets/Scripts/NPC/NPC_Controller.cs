using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Controller : MonoBehaviour
{

    public ActorControllerConnector actor;


    public GameObject weaponObject;
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
    public void Input_Movement(Vector2 movementDirection)
    {
        testmovementDirection = movementDirection;

        movementDirection = movementDirection.normalized;
        //Vector2 movementOffset = Vector2.Perpendicular(movementDirection) * 0.95f;

        //movementDirection += movementOffset;
        //Debug.Log("movementDirection = " + movementDirection);

        actor.Input_Move(movementDirection);
    }
    Vector3 testmovementDirection;
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(actor.transform.position, actor.transform.position + testmovementDirection);
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
