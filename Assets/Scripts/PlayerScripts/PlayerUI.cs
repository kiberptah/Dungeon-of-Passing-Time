using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    PlayerController playerController;

    [SerializeField] Transform playerHealthBar;
    [SerializeField] Transform playerStaminaBar;

    [SerializeField] InteractionUI interactionUI;
    IInteractable currentInteractable;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }
    private void OnEnable()
    {
        playerController.actorConnector.updateHealthInfo += UpdateHealthBar;
        playerController.actorConnector.updateStaminaInfo += UpdateStaminaBar;

        playerController.actorConnector.interactableAreaEntered += InteractableAreaEntered;
        playerController.actorConnector.interactableAreaLeft += InteractableAreaLeft;
    }
    private void OnDisable()
    {
        playerController.actorConnector.updateHealthInfo -= UpdateHealthBar;

        playerController.actorConnector.interactableAreaEntered -= InteractableAreaEntered;
        playerController.actorConnector.interactableAreaLeft -= InteractableAreaLeft;
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

    void InteractableAreaEntered(IInteractable interactable)
    {
        interactionUI.Activate(interactable.actionLabel);
        currentInteractable = interactable;
    }
    void InteractableAreaLeft(IInteractable interactable)
    {
        if (interactable == currentInteractable)
        {
            interactionUI.Deactivate();
        }
    }
}
