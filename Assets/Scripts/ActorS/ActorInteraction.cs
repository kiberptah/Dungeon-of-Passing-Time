using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ActorInteraction : MonoBehaviour
{
    ActorConnector controllerConnector;

    public event Action<IInteractable> interactableAreaEntered;
    public event Action<IInteractable> interactableAreaLeft;

    private void Awake()
    {
        controllerConnector = GetComponent<ActorConnector>();
    }
    public void TriggerEnterCallbackCatch(Collider collision)
    {
        if (collision.transform.TryGetComponent(out IInteractable interactable))
        {
            interactableAreaEntered?.Invoke(interactable);
        }

    }
    public void TriggerExitCallbackCatch(Collider collision)
    {
        if (collision.transform.TryGetComponent(out IInteractable interactable))
        {
            interactableAreaLeft?.Invoke(interactable);
        }

    }
}
