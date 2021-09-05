using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableLever : MonoBehaviour, IInteractable
{
    public UnityEvent FlipLever;   
    public void OnHoverStart(Transform _interactor)
    {

    }
    public void OnInteract(Transform interactor)
    {
        FlipLever?.Invoke();
    }
    public void OnHoverEnd(Transform _interactor)
    {

    }
}
