using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public ActorConnector actorConnector;
    Transform actor;
    WeaponConnector weaponConnector;
    GameObject weaponObject;
    Transform customMouseCursor;
    public CustomMouseCursorScript cursorScript;


    PlayerInput playerInput;
    InputAction input_movement;



    bool inputDisabled = false;

    //Weapon weaponScript;
    private void Awake()
    {

        actorConnector.isPlayerControlled = true;
        actor = actorConnector.transform;

        customMouseCursor = GameObject.FindGameObjectWithTag("MouseCursor").transform;



    }
    private void OnEnable()
    {
        //playerInput.Character.Movement.performed += Input_Movement;
        playerInput = new PlayerInput();
        playerInput.Enable();

        input_movement = playerInput.Character.Movement;

        playerInput.Character.DrawWeapon.performed += DrawWeapon;
        playerInput.Character.SheathWeapon.performed += SheathWeapon;

        /* 
                EventDirector.dialogue_start += DisableInput;
                EventDirector.dialogue_end += EnableInput;
         */

        actorConnector.updateDrawnWeapon += UpdateDrawnWeapon;
    }
    private void OnDisable()
    {
        input_movement.Disable();

        playerInput.Character.DrawWeapon.performed -= DrawWeapon;
        playerInput.Character.SheathWeapon.performed -= SheathWeapon;
        //playerInput.Character.Movement.performed -= Input_Movement;

        /* 
                EventDirector.dialogue_start -= DisableInput;
                EventDirector.dialogue_end -= EnableInput;
        */
        actorConnector.updateDrawnWeapon -= UpdateDrawnWeapon;
    }


    private void Update()
    {
        if (!inputDisabled)
        {
            Input_Weapon();
            Input_Movement();
            Input_Interaction();

            Input_WeaponSwing();
        }

    }

    void DrawWeapon(InputAction.CallbackContext call)
    {
        if (cursorScript.cursorCentered == false)
        {
            actorConnector.Input_DrawWeapon(customMouseCursor.position);
            cursorScript.combatMode = true;
        }
    }
    void SheathWeapon(InputAction.CallbackContext call)
    {
        actorConnector.Input_SheathWeapon();
        cursorScript.combatMode = false;
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

        // if (Input.GetButtonDown("DrawWeapon"))
        // {
        //     actorConnector.Input_DrawOrSheathWeapon(customMouseCursor.position);
        // }

        if (Input.GetButtonDown("NextWeaponSlot"))
        {
            actorConnector.Input_NextWeaponSlot();
        }

        // if (Input.GetButtonDown("RMB"))
        // {
        //     actorConnector.Input_SpecialAttack();
        //     //print("pierce");
        // }

    }
    void Input_Movement()
    {
        Vector2 movementDirection = Vector2.zero;
        /* 
                movementDirection.x = Input.GetAxis("Horizontal");
                movementDirection.y = Input.GetAxis("Vertical");
         */
        movementDirection = input_movement.ReadValue<Vector2>();
        movementDirection = movementDirection.normalized;

        actorConnector.Input_Move(movementDirection);

        /* 
                if (Input.GetButtonDown("Dash"))
                {
                    actorConnector.Input_Ability();
                }
         */
    }

    void Input_Interaction()
    {
        if (Input.GetButtonDown("Interact"))
        {
            actorConnector.Input_Interact();
        }
    }


    void UpdateDrawnWeapon(GameObject _weaponObject, WeaponConnector _weaponConnector)
    {
        weaponObject = _weaponObject;
        weaponConnector = actorConnector.weaponManager.weaponConnector;
    }

    int swingDirection = 0;
    void Input_WeaponSwing()
    {
        if (weaponObject != null && weaponConnector != null)
        {

            //int swingDirection = 0;


            /// If mouse further from blade then this angle, it mean player is swinging the sword widely 
            /// and for easier control we shouldn't interrupt the swing 
            /// until mouse is not really close to the blade again 
            float thresholdAngle = weaponConnector.weaponScript.stats.sensitivityAngle;

            //float maxAngle = 2f; // angle after which sword starts moving
            float minAngle = 1f; // angle after which sword should be considered aligned with mouse

            Vector2 mouseProjection = actor.InverseTransformPoint(customMouseCursor.position).normalized;
            Vector2 bladeProjection = actor.InverseTransformPoint(weaponObject.transform.position).normalized;

            //float angleBetweenBladeAndCursor = Vector3.SignedAngle(bladeProjection, mouseProjection, -actor.forward);
            float angleBetweenBladeAndCursor = Vector2.SignedAngle(mouseProjection, bladeProjection);

            float distanceBetweenVectors = Vector2.Distance(bladeProjection, mouseProjection); // to avoid jitter caused by overshooting. It is a hypotnuse?
            //Debug.Log("angleBetweenBladeAndCursor " + angleBetweenBladeAndCursor);
            if (Mathf.Abs(angleBetweenBladeAndCursor) < thresholdAngle)// || swingDirection == 0)
            {
                if (angleBetweenBladeAndCursor > 0)
                {
                    swingDirection = 1;
                }
                if (angleBetweenBladeAndCursor < 0)
                {
                    swingDirection = -1;
                }
                if (Mathf.Abs(angleBetweenBladeAndCursor) < minAngle)
                {
                    swingDirection = 0;
                }

                //Debug.Log(swingDirection);
                //actorConnector.Input_UpdateSwingDirection(swingDirection, distanceBetweenVectors);
            }
            actorConnector.Input_UpdateSwingDirection(swingDirection, distanceBetweenVectors);
        }
    }
}
