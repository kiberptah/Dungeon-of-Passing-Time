using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableLever : MonoBehaviour, IInteractable
{
    public string text = "Flip Lever";
    public bool isAvailableforInteraction { get; set; }
    public bool isInteractableByDefault = true;

    public UnityEvent FlipLever;
    private void Start()
    {
        isAvailableforInteraction = isInteractableByDefault;
    }
    public void OnHoverStart(InteractionUI interactionUI, Transform _interactor)
    {
        interactionUI.Activate(text);

    }
    public void OnInteract(Transform interactor)
    {
        if (isAvailableforInteraction)
        {
            FlipLever?.Invoke();
        }
    }
    public void OnHoverEnd(InteractionUI interactionUI, Transform _interactor)
    {
        interactionUI.Deactivate();
    }

    public void switchInteractivity()
    {
        isAvailableforInteraction = !isAvailableforInteraction;
    }
}
