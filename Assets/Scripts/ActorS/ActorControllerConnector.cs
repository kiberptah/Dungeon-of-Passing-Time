using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class ActorControllerConnector : MonoBehaviour
{
    [HideInInspector] public ActorStats actorStats;
    [Header("Health")]
    ActorHealth actorHealth;
    public event Action<float, float> updateHealthInfo;

    [Header("Stamina")]
    ActorStamina actorStamina;
    public event Action<float, float> updateStaminaInfo;

    [Header("Interactions")]
    ActorInteraction actorInteraction;
    IInteractable interactableObject;
    public event Action<IInteractable> interactableAreaEntered;
    public event Action<IInteractable> interactableAreaLeft;
    bool isInteractableClose = false;

    [Header("Movement")]
    ActorMovement actorMovement;

    [Header("Animation")]
    ActorAnimationManager actorAnimation;

    [Header("Weapon")]
    public WeaponController weaponController;
    WeaponManager weaponManager;
    public event Action<GameObject, Weapon> updateDrawnWeapon;

    private void Awake()
    {
        actorStats = GetComponent<ActorStats>();

        actorHealth = GetComponent<ActorHealth>();
        actorStamina = GetComponent<ActorStamina>();
        actorInteraction = GetComponent<ActorInteraction>();
        actorMovement = GetComponent<ActorMovement>();
        actorAnimation = GetComponent<ActorAnimationManager>();
        weaponController = GetComponent<WeaponController>();
        weaponManager = GetComponent<WeaponManager>();
    }
    private void OnEnable()
    {
        actorHealth.updateHealthInfo += UpdateHealthInfo;
        actorStamina.updateStaminaInfo += UpdateStaminaInfo;

        weaponController.updateDrawnWeapon += UpdateDrawnWeapon;

        actorInteraction.interactableAreaEntered += InteractableAreaEntered;
        actorInteraction.interactableAreaLeft += InteractableAreaLeft;

    }
    private void OnDisable()
    {
        actorHealth.updateHealthInfo -= UpdateHealthInfo;
        actorStamina.updateStaminaInfo -= UpdateStaminaInfo;

        weaponController.updateDrawnWeapon -= UpdateDrawnWeapon;

        actorInteraction.interactableAreaEntered -= InteractableAreaEntered;
        actorInteraction.interactableAreaLeft -= InteractableAreaLeft;
    }

    // ----- Stats
    void UpdateHealthInfo(float newHealth, float maxHealth)
    {
        updateHealthInfo?.Invoke(newHealth, maxHealth);
    }
    void UpdateStaminaInfo(float newStamina, float maxStamina)
    {
        updateStaminaInfo?.Invoke(newStamina, maxStamina);
    }


    // ----- Interactions
    void InteractableAreaEntered(IInteractable _interactableObject)
    {
        interactableObject = _interactableObject;
        interactableAreaEntered?.Invoke(_interactableObject);

        isInteractableClose = true;
    }
    void InteractableAreaLeft(IInteractable _interactableObject)
    {
        interactableObject = _interactableObject;
        interactableAreaLeft?.Invoke(_interactableObject);

        isInteractableClose = false;
    }
    public void Input_Interact()
    {
        if (interactableObject != null && isInteractableClose)
        {
            interactableObject.OnInteract(transform);
        }
    }



    // ----- Movement
    public void Input_Move(Vector3 _direction)
    {
        //playerMovement.InputMovement(movementDirection);
        if (_direction != Vector3.zero)
        {
            actorAnimation.UpdateVector(transform, _direction);
            actorAnimation.UpdateAction(transform, ActorAnimationManager.spriteAction.walking);
        }
        else
        {
            actorAnimation.UpdateVector(transform, _direction);
            actorAnimation.UpdateAction(transform, ActorAnimationManager.spriteAction.idle);
        }

        actorMovement.movementDirection = _direction;
    }

    // ----- Ability
    public void Input_Ability()
    {
        actorMovement.Dash();
    }

    // ----- Weapon


    public void Input_UpdateSwingDirection(int direction, float distanceBetweenVectors)
    {
        weaponController.UpdateSwingDirection(direction, distanceBetweenVectors);
    }
    /*
    public void Input_UpdateMaxSwingOffset(float distance)
    {
        weaponController.UpdateMaxSwingOffset(distance);
    }
    */

    public void Input_PiercingAttack()
    {
        weaponController.PiercingAttack();
    }

    #region Draw/Sheath
    public void Input_DrawOrSheathWeapon(Vector2 _position = new Vector2())
    {
        weaponManager.Input_DrawOrSheathWeapon(_position);
    }
    public void Input_DrawWeapon()
    {
        weaponManager.Input_DrawWeapon();
    }
    public void Input_SheathWeapon()
    {
        weaponManager.Input_SheathWeapon();
    }
    public void Input_NextWeaponSlot()
    {
        weaponManager.Input_NextWeaponSlot();
    }

    void UpdateDrawnWeapon(GameObject _weaponObject, Weapon _weaponScript)
    {
        updateDrawnWeapon?.Invoke(_weaponObject, _weaponScript);
    }

    #endregion


}
