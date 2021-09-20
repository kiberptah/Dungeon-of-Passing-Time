using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public bool isAvailableforInteraction { get; set; }
    public void OnHoverStart(Transform _interactor);
    public void OnInteract(Transform interactor);
    public void OnHoverEnd(Transform _interactor);

}

