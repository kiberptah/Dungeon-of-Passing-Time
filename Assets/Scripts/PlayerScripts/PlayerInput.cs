using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public ActorControllerConnector actorControllerConnector;
    Transform actor;

    bool inputDisabled = false;

    Transform customMouseCursor;
    GameObject weaponObject;
    Weapon weaponScript;

    private void OnEnable()
    {
        EventDirector.dialogue_start += DisableInput;
        EventDirector.dialogue_end += EnableInput;

        actorControllerConnector.updateDrawnWeapon += UpdateDrawnWeapon;
    }
    private void OnDisable()
    {
        EventDirector.dialogue_start -= DisableInput;
        EventDirector.dialogue_end -= EnableInput;

        actorControllerConnector.updateDrawnWeapon -= UpdateDrawnWeapon;
    }

    private void Awake()
    {
        actor = actorControllerConnector.transform;

        customMouseCursor = GameObject.FindGameObjectWithTag("MouseCursor").transform;



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
            actorControllerConnector.Input_DrawOrSheathWeapon(customMouseCursor.position);
        }
        if (Input.GetButtonDown("NextWeaponSlot"))
        {
            actorControllerConnector.Input_NextWeaponSlot();
        }

        if (Input.GetButtonDown("RMB"))
        {
            actorControllerConnector.Input_PiercingAttack();
            //print("pierce");
        }
    }
    void Input_Movement()
    {
        Vector3 movementDirection = Vector3.zero;

        movementDirection.x = Input.GetAxis("Horizontal");
        movementDirection.y = Input.GetAxis("Vertical");

        movementDirection = movementDirection.normalized;

        actorControllerConnector.Input_Move(movementDirection);


        if (Input.GetButtonDown("Dash"))
        {
            actorControllerConnector.Input_Ability();
        }

    }

    void Input_Interaction()
    {
        if (Input.GetButtonDown("Interact"))
        {
            actorControllerConnector.Input_Interact();
        }
    }


    void UpdateDrawnWeapon(GameObject _weaponObject, Weapon _weaponScript)
    {
        weaponObject = _weaponObject;
        weaponScript = _weaponScript;
    }
    void Input_WeaponSwing()
    {
        if (weaponObject != null && weaponScript != null)
        {

            int swingDirection = 0;


            /// If mouse further from blade then this angle, it mean player is swinging the sword widely 
            /// and for easier control we shouldn't interrupt the swing 
            /// until mouse is not really close to the blade again 
            float thresholdAngle = weaponScript.stats.weaponSensitivityAngle;

            float maxAngle = 2f; // angle after which sword starts moving
            float minAngle = 2f; // angle after which sword should be considered aligned with mouse

            Vector2 mouseProjection = actor.InverseTransformPoint(customMouseCursor.position).normalized;
            Vector2 bladeProjection = actor.InverseTransformPoint(weaponObject.transform.position).normalized;

            //float angleBetweenBladeAndCursor = Vector3.SignedAngle(bladeProjection, mouseProjection, -actor.forward);
            float angleBetweenBladeAndCursor = Vector2.SignedAngle(mouseProjection, bladeProjection);

            float distanceBetweenVectors = Vector2.Distance(bladeProjection, mouseProjection); // to avoid jitter caused by overshooting. It is a hypotnuse?

            if (Mathf.Abs(angleBetweenBladeAndCursor) < thresholdAngle)// || swingDirection == 0)
            {
                if (angleBetweenBladeAndCursor > maxAngle)
                {
                    swingDirection = 1;
                }
                if (angleBetweenBladeAndCursor < -maxAngle)
                {
                    swingDirection = -1;
                }
                //if (Mathf.Abs(angleBetweenBladeAndCursor) <= minAngle)
                if (Mathf.Abs(angleBetweenBladeAndCursor) < minAngle)
                {
                    swingDirection = 0;
                }


                actorControllerConnector.Input_UpdateSwingDirection(swingDirection, distanceBetweenVectors);
            }
            //actorControllerConnector.Input_UpdateMaxSwingOffset(distanceBetweenVectors);

        }
    }
}
