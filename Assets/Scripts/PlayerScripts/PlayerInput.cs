using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    WeaponManager weaponManager;
    WeaponController weaponController;
    public PlayerMovement playerMovement;
    
    private void Awake()
    {
        weaponManager = GetComponent<WeaponManager>();
        weaponController = GetComponent<WeaponController>();


    }

    private void Update()
    {
        Input_Weapon();
        Input_Movement();
        Input_Interaction();

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

        if (Input.GetButtonDown("RMB"))
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
        if (movementDirection != Vector3.zero)
        {
            //spriteManager.UpdateDirection(movementDirection, ActorSpritesDirectionManager.spriteAction.walking);
            EventDirector.somebody_UpdateSpriteVector?.Invoke(transform, movementDirection);
            EventDirector.somebody_UpdateSpriteAction?.Invoke(transform, ActorAnimationManager.spriteAction.walking);

        }
        else
        {
            //spriteManager.UpdateDirection(movementDirection, ActorSpritesDirectionManager.spriteAction.idle);
            EventDirector.somebody_UpdateSpriteVector?.Invoke(transform, movementDirection);
            EventDirector.somebody_UpdateSpriteAction?.Invoke(transform, ActorAnimationManager.spriteAction.idle);

        }


        if (Input.GetButtonDown("Dash"))
        {
            playerMovement.Dash();
        }

    }

    void Input_Interaction()
    {
        /*
        Plane plane = new Plane(Vector2.up, Vector2.zero, Vector2.right);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distanceToPlane;

        if (plane.Raycast(ray, out distanceToPlane))
        {

        }
        */
        if (Input.GetButtonDown("LMB"))
        {
            //Debug.Log("click");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.GetRayIntersectionAll (ray, Mathf.Infinity);
             
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.transform.TryGetComponent(out IInteractable interactable))
                {
                    interactable?.OnInteract(transform);
                    Debug.Log("INTERACTION!");
                }
            }
            
        }
        
    }
}
