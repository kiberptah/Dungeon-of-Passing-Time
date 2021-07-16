using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    WeaponManager weaponManager;
    WeaponController weaponController;
    public PlayerMovement playerMovement;
    public ActorSpritesDirectionManager spriteManager;
    
    private void Awake()
    {
        weaponManager = GetComponent<WeaponManager>();
        weaponController = GetComponent<WeaponController>();


    }

    private void Update()
    {
        Input_Weapon();
        Input_Movement();


    }
    private void FixedUpdate()
    {
        weaponController.UpdateMousePosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));

    }


    void Input_Weapon()
    {
        if (Input.GetButtonDown("DrawWeapon"))
        {
            weaponManager.Input_DrawOrSheathWeapon();
        }
        if (Input.GetButtonDown("NextWeaponSlot"))
        {
            weaponManager.Input_NextSlot();
        }

        if (Input.GetButtonDown("HoldToPierce"))
        {
            weaponController.WeaponPiercing();
            //print("pierce");
        }
    }
    void Input_Movement()
    {
        Vector3 movementDirection = Vector3.zero;

        movementDirection.x = Input.GetAxis("Horizontal");
        movementDirection.y = Input.GetAxis("Vertical");

        movementDirection = movementDirection.normalized;

        playerMovement.InputMovement(movementDirection);
        spriteManager.UpdateDirection(movementDirection);

        if (Input.GetButtonDown("Dash"))
        {
            playerMovement.Dash();
        }

    }
}
