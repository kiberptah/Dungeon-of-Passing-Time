using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorInteraction : MonoBehaviour
{
    public InteractionUI interactionUI;
    IInteractable closestInteractableObject;


    public void TriggerEnterCallbackCatch(Collider2D collision)
    {
        
        if (collision.transform.TryGetComponent(out IInteractable interactable))
        {
            interactable?.OnHoverStart(interactionUI, transform);
            closestInteractableObject = interactable;
            //interactionUI.SetActive(true);
            //Debug.Log("CAN INTERACT WITH " + collision.name);
        }

    }
    public void TriggerExitCallbackCatch(Collider2D collision)
    {
        
        if (collision.transform.TryGetComponent(out IInteractable interactable))
        {
            interactable?.OnHoverEnd(interactionUI, transform);
            closestInteractableObject = null;
            //interactionUI.SetActive(false);
            //Debug.Log("NO LONGER CAN INTERACT WITH " + collision.name);
        }
        
    }

    public void Interact()
    {
        if (closestInteractableObject != null)
        {
            closestInteractableObject?.OnInteract(transform);
            //Debug.Log("INTERACTION WITH " + closestInteractableObject);

        }
    }

}
