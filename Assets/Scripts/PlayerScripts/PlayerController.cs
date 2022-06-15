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
        playerInput = new PlayerInput();
        playerInput.Enable();

        input_movement = playerInput.Character.Movement;

        playerInput.Character.DrawWeapon.performed += DrawWeapon;
        playerInput.Character.SheathWeapon.performed += SheathWeapon;
        playerInput.Character.NextWeaponSlot.performed += NextWeaponSlot;

        playerInput.Character.Attack.performed += Attack;

        playerInput.Character.Interact.performed += Interact;



        actorConnector.updateDrawnWeapon += UpdateDrawnWeapon;
    }
    private void OnDisable()
    {
        input_movement.Disable();

        playerInput.Character.DrawWeapon.performed -= DrawWeapon;
        playerInput.Character.SheathWeapon.performed -= SheathWeapon;
        playerInput.Character.NextWeaponSlot.performed -= NextWeaponSlot;

        playerInput.Character.Attack.performed -= Attack;

        playerInput.Character.Interact.performed -= Interact;


        actorConnector.updateDrawnWeapon -= UpdateDrawnWeapon;
    }


    private void Update()
    {
        if (!inputDisabled)
        {
            //Input_Weapon();
            Input_Movement();
            //Input_Interaction();

            Input_WeaponSwing();
        }

    }

    void Interact(InputAction.CallbackContext call)
    {
        if (cursorScript.potentialInteractable != null)
        {
            if (actorConnector.interactionDetector.TryToInteract(cursorScript.potentialInteractable, actor))
            {
                // call some visual feedback from cursor
            }
        }
    }
    void NextWeaponSlot(InputAction.CallbackContext call)
    {
        actorConnector.Input_NextWeaponSlot();
    }
    void Dash()
    {
        actorConnector.Input_Ability();
    }
    void Attack(InputAction.CallbackContext call)
    {
        if (weaponObject != null)
        {
            actorConnector.Input_SpecialAttack();
        }
    }
    void DrawWeapon(InputAction.CallbackContext call)
    {
        if (cursorScript.cursorCentered == false)
        {
            actorConnector.Input_DrawWeapon(customMouseCursor.position);
            cursorScript.combatMode = true;
            cursorScript.ChangeCursorState(CustomMouseCursorScript.cursorState.combat);
        }
    }
    void SheathWeapon(InputAction.CallbackContext call)
    {
        actorConnector.Input_SheathWeapon();
        cursorScript.combatMode = false;
        cursorScript.ChangeCursorState(CustomMouseCursorScript.cursorState.original);
    }


    void Input_Movement()
    {
        Vector3 movementDirection = Vector3.zero;

        movementDirection = new Vector3(input_movement.ReadValue<Vector2>().x, 0 , input_movement.ReadValue<Vector2>().y);
        movementDirection = movementDirection.normalized;

        actorConnector.Input_Move(movementDirection);
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

            Vector3 mouseProjection = actor.InverseTransformPoint(customMouseCursor.position.Grounded()).normalized;
            Vector3 bladeProjection = actor.InverseTransformPoint(weaponObject.transform.position.Grounded()).normalized;

            //float angleBetweenBladeAndCursor = Vector3.SignedAngle(bladeProjection, mouseProjection, -actor.forward);
            float angleBetweenBladeAndCursor = Vector3.SignedAngle(mouseProjection, bladeProjection, Vector3.down);

            float distanceBetweenVectors = Vector3.Distance(bladeProjection, mouseProjection); // to avoid jitter caused by overshooting. It is a hypotnuse?
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