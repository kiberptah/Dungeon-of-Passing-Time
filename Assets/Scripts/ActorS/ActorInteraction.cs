using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ActorInteraction : MonoBehaviour
{
    ActorControllerConnector controllerConnector;

    public event Action<IInteractable> interactableAreaEntered;
    public event Action<IInteractable> interactableAreaLeft;
    public event Action<IInteractable> interaction;

    private void Awake()
    {
        controllerConnector = GetComponent<ActorControllerConnector>();
    }
    public void TriggerEnterCallbackCatch(Collider2D collision)
    {        
        if (collision.transform.TryGetComponent(out IInteractable interactable))
        {
            interactableAreaEntered?.Invoke(interactable);
        }

    }
    public void TriggerExitCallbackCatch(Collider2D collision)
    {     
        if (collision.transform.TryGetComponent(out IInteractable interactable))
        {
            interactableAreaLeft?.Invoke(interactable);
        }
        
    }
}
