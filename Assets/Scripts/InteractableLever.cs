using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableLever : MonoBehaviour, IInteractable
{
    public bool isAvailableforInteraction { get; set; }
    public bool isInteractableByDefault = true;

    public UnityEvent FlipLever;
    private void Start()
    {
        isAvailableforInteraction = isInteractableByDefault;
    }
    public void OnHoverStart(Transform _interactor)
    {

    }
    public void OnInteract(Transform interactor)
    {
        if (isAvailableforInteraction)
        {
            FlipLever?.Invoke();
        }
    }
    public void OnHoverEnd(Transform _interactor)
    {

    }

    public void switchInteractivity()
    {
        isAvailableforInteraction = !isAvailableforInteraction;
    }
}
