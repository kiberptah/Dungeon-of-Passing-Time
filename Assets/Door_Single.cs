using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Single : MonoBehaviour
{
    public Collider2D doorCollider;
    Animator animator;
    bool isDoorOpened = false;
    bool isAnimationInProcess = false;
    private void Start()
    {
        animator = GetComponent<Animator>();

        if (doorCollider == null)
        {
            if (TryGetComponent(out Collider2D coll))
            {
                doorCollider = coll;
            }
        }
    }

    public void FlipDoorState()
    {
        isDoorOpened = !isDoorOpened;

        if (isDoorOpened)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
    }
    public void SetDoorOpened(bool isOpened)
    {
        isDoorOpened = isOpened;
    }

    void OpenDoor()
    {
        if (!isAnimationInProcess)
        {
            animator.Play("Door_Open");
        }
        
    }
    void CloseDoor()
    {
        if (!isAnimationInProcess)
        {
            animator.Play("Door_Close");
        }
            
    }
    public void StartAnimation()
    {
        isAnimationInProcess = true;
    }
    public void EndAnimation()
    {
        isAnimationInProcess = false;
    }
    public void DisableCollider()
    {
        if (doorCollider != null)
        {
            doorCollider.enabled = false;
        }
    }
    public void EnableCollider()
    {
        if (doorCollider != null)
        {
            doorCollider.enabled = true;
        }
    }
}
