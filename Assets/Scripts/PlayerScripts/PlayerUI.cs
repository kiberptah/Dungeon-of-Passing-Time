using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    PlayerController playerController;


    ActorHealth actorHealth;
    ActorStamina actorStamina;
    [SerializeField] Transform playerHealthBar;
    [SerializeField] Transform playerStaminaBar;

    [SerializeField] InteractionUI interactionUI;
    IInteractable currentInteractable;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();

        playerController.actorConnector.transform.TryGetComponent(out actorHealth);
        playerController.actorConnector.transform.TryGetComponent(out actorStamina);
    }
    private void OnEnable()
    {
        if (actorHealth != null)
        {
            actorHealth.updateHealthInfo += UpdateHealthBar;
        }
        if (actorHealth != null)
        {
            actorStamina.updateStaminaInfo += UpdateStaminaBar;
        }
    }
    private void OnDisable()
    {
        if (actorHealth != null)
        {
            actorHealth.updateHealthInfo -= UpdateHealthBar;
        }
        if (actorHealth != null)
        {
            actorStamina.updateStaminaInfo -= UpdateStaminaBar;
        }
    }

    void UpdateHealthBar(float newHealth, float maxHealth)
    {
        if (playerHealthBar != null)
        {
            playerHealthBar.localScale = new Vector3(newHealth / maxHealth, playerHealthBar.localScale.y, playerHealthBar.localScale.z);
        }
    }
    void UpdateStaminaBar(float newStamina, float maxStamina)
    {
        if (playerStaminaBar != null)
        {
            playerStaminaBar.localScale = new Vector3(newStamina / maxStamina, playerStaminaBar.localScale.y, playerStaminaBar.localScale.z);
        }
    }
}
