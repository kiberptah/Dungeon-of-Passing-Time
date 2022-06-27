using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionDetector : MonoBehaviour
{
    public List<IInteractable> interactables = new List<IInteractable>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IInteractable interact))
        {
            interactables.Add(interact);
            //Debug.Log(other.transform.name + " added to the list");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IInteractable interact))
        {
            interactables.Remove(interact);
            //Debug.Log(other.transform.name + " removed from the list");

        }
    }

    public bool TryToInteract(IInteractable interactable, Transform interactor)
    {
        if (interactables.Contains(interactable))
        {
            interactable.OnInteract(interactor);
            return true;
        }
        else
        {
            return false;
        }
    }

}
