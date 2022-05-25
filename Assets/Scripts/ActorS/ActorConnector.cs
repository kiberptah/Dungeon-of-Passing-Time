using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class ActorConnector : MonoBehaviour
{
    public Transform controller;
    public bool isPlayerControlled = false;
    [HideInInspector] public ActorStats stats;
    [Header("Health")]
    ActorHealth actorHealth;
    //[SerializeField] Transform actorHealthBar;

    public event Action noHealth;
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
    ActorAnimManager actorAnimation;

    [Header("Weapon")]
    [HideInInspector] public WeaponManager weaponManager;
    public event Action<GameObject, WeaponConnector> updateDrawnWeapon;

    #region
    private void Awake()
    {
        stats = GetComponent<ActorStats>();

        actorHealth = GetComponent<ActorHealth>();
        actorStamina = GetComponent<ActorStamina>();
        actorInteraction = GetComponent<ActorInteraction>();
        actorMovement = GetComponent<ActorMovement>();
        actorAnimation = GetComponent<ActorAnimManager>();
        //weaponController = GetComponent<WeaponController>();
        weaponManager = GetComponent<WeaponManager>();

    }
    private void OnEnable()
    {

        actorHealth.noHealth += NoHealth;
        actorHealth.updateHealthInfo += UpdateHealthInfo;
        actorStamina.updateStaminaInfo += UpdateStaminaInfo;

        weaponManager.updateDrawnWeapon += UpdateDrawnWeapon;

        actorInteraction.interactableAreaEntered += InteractableAreaEntered;
        actorInteraction.interactableAreaLeft += InteractableAreaLeft;

    }
    private void OnDisable()
    {

        actorHealth.noHealth -= NoHealth;
        actorHealth.updateHealthInfo -= UpdateHealthInfo;
        actorStamina.updateStaminaInfo -= UpdateStaminaInfo;

        weaponManager.updateDrawnWeapon -= UpdateDrawnWeapon;

        actorInteraction.interactableAreaEntered -= InteractableAreaEntered;
        actorInteraction.interactableAreaLeft -= InteractableAreaLeft;
    }
    #endregion

    void NoHealth(float dmg, float kb, Transform killer)
    {
        noHealth?.Invoke();
    }

    #region Stats
    void UpdateHealthInfo(float newHealth, float maxHealth)
    {
        updateHealthInfo?.Invoke(newHealth, maxHealth);
    }
    void UpdateStaminaInfo(float newStamina, float maxStamina)
    {
        updateStaminaInfo?.Invoke(newStamina, maxStamina);
    }
    #endregion


    #region Interactions
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
    #endregion


    #region Movement
    public void Input_Move(Vector3 _direction)
    {
        /* 
        if (_direction != Vector3.zero)
        {
            actorAnimation.UpdateAnimation("walk", _direction);
        }
        else
        {
            actorAnimation.UpdateAnimation("idle", _direction);
        }
        */
        actorMovement.UpdateMovement(_direction);
    }
    public void Input_Ability()
    {
        actorMovement.Dash();
    }
    #endregion


    #region Weapon
    public void Input_UpdateSwingDirection(int direction, float distanceBetweenVectors = 1f)
    {
        weaponManager.UpdateSwingDirection(direction, distanceBetweenVectors);
    }
    public void Input_SpecialAttack()
    {
        weaponManager.SpecialAttack();
    }
    #endregion

    #region Draw/Sheath
    public void Input_DrawOrSheathWeapon(Vector2? _mousePosition = null)
    {
        weaponManager.Input_DrawOrSheathWeapon(_mousePosition);
    }
    public void Input_DrawWeapon(Vector2? _mousePosition = null)
    {
        weaponManager.Input_DrawWeapon(_mousePosition);
    }
    public void Input_SheathWeapon()
    {
        weaponManager.Input_SheathWeapon();
    }
    public void Input_NextWeaponSlot()
    {
        weaponManager.Input_NextWeaponSlot();
    }

    void UpdateDrawnWeapon(GameObject _weaponObject, WeaponConnector _weaponConnector)
    {
        updateDrawnWeapon?.Invoke(_weaponObject, _weaponConnector);
    }

    #endregion


}
