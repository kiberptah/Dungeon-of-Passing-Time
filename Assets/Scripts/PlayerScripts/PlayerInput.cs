using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    WeaponManager weaponManager;
    WeaponController weaponController;
    public PlayerMovement playerMovement;
    public ActorInteraction actorInteraction;
    bool inputDisabled = false;

    Transform customMouseCursor;
    private void OnEnable()
    {
        EventDirector.dialogue_start += DisableInput;
        EventDirector.dialogue_end += EnableInput;
    }
    private void OnDisable()
    {
        EventDirector.dialogue_start -= DisableInput;
        EventDirector.dialogue_end -= EnableInput;
    }

    private void Awake()
    {
        weaponManager = GetComponent<WeaponManager>();
        weaponController = GetComponent<WeaponController>();


    }
    private void Start()
    {
        customMouseCursor = GameObject.FindGameObjectWithTag("MouseCursor").transform;
    }

    private void Update()
    {
        if (inputDisabled == false)
        {
            Input_Weapon();
            Input_Movement();
            Input_Interaction();
        }
        

    }
    private void FixedUpdate()
    {
        //weaponController.UpdateMousePosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        weaponController.UpdateMousePosition(customMouseCursor.position);

    }

    void DisableInput()
    {
        inputDisabled = true;
    }
    void EnableInput()
    {
        inputDisabled = false;
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
        if (Input.GetButtonDown("Interact"))
        {
            actorInteraction.Interact();
        }

        /*
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
                    //Debug.Log("INTERACTION!");
                    break;
                }
            }
            
        }
        */
        
    }
}
