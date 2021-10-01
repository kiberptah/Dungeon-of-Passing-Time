using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorInteraction : MonoBehaviour
{
    public GameObject interactionUI;
    IInteractable closestInteractableObject;

    private void Update()
    {
        if (closestInteractableObject != null)
        {
            if (closestInteractableObject.isAvailableforInteraction == false)
            {
                closestInteractableObject?.OnHoverEnd(transform);
                interactionUI.SetActive(false);
                //Debug.Log("TEMPORARY CAN'T INTERACT WITH " + closestInteractableObject);
            }
            else
            {
                closestInteractableObject?.OnHoverStart(transform);
                interactionUI.SetActive(true);
                //Debug.Log("AGAIN CAN INTERACT WITH " + closestInteractableObject);
            }
        }
    }
    public void TriggerEnterCallbackCatch(Collider2D collision)
    {
        
        if (collision.transform.TryGetComponent(out IInteractable interactable))
        {
            if (interactable.isAvailableforInteraction)
            {
                interactable?.OnHoverStart(transform);
                closestInteractableObject = interactable;
                interactionUI.SetActive(true);
                //Debug.Log("CAN INTERACT WITH " + collision.name);
            }
        }
        
    }
    public void TriggerExitCallbackCatch(Collider2D collision)
    {
        
        if (collision.transform.TryGetComponent(out IInteractable interactable))
        {

            interactable?.OnHoverEnd(transform);
            closestInteractableObject = null;
            interactionUI.SetActive(false);
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
