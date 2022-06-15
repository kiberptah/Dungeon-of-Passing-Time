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

    public event Action noHealth;
    //public Action<float, float> updateHealthInfo;

    [Header("Stamina")]
    ActorStamina actorStamina;
    //public Action<float, float> updateStaminaInfo;

    [Header("Interactions")]
    public InteractionDetector interactionDetector;
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

    #region Init
    private void Awake()
    {
        stats = GetComponent<ActorStats>();

        actorHealth = GetComponent<ActorHealth>();
        actorStamina = GetComponent<ActorStamina>();
        actorMovement = GetComponent<ActorMovement>();
        actorAnimation = GetComponent<ActorAnimManager>();
        weaponManager = GetComponent<WeaponManager>();
    }
    private void OnEnable()
    {
        actorHealth.noHealth += NoHealth;


        weaponManager.updateDrawnWeapon += UpdateDrawnWeapon;

    }
    private void OnDisable()
    {

        actorHealth.noHealth -= NoHealth;


        weaponManager.updateDrawnWeapon -= UpdateDrawnWeapon;

    }
    #endregion

    void NoHealth(float dmg, float kb, Transform killer)
    {
        noHealth?.Invoke();
    }




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
    public void Input_DrawWeapon(Vector3? _mousePosition = null)
    {
        //Debug.Log("_mousePosition " + _mousePosition);
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
