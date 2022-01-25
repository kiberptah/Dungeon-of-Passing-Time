using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    //public bool isAvailableforInteraction { get; set; }
    public string actionLabel { get; }
    public void OnHoverStart(InteractionUI _interactionUI, Transform _interactor);
    public void OnInteract(Transform interactor);
    public void OnHoverEnd(InteractionUI _interactionUI, Transform _interactor);

}

