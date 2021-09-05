using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Single : MonoBehaviour
{
    Animator animator;
    bool isDoorOpened = false;
    private void Start()
    {
        animator = GetComponent<Animator>();
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
        animator.Play("Door_Open");
    }
    void CloseDoor()
    {
        animator.Play("Door_Close");
    }

    public void DisableCollider()
    {
        if (TryGetComponent(out Collider coll))
        {
            coll.enabled = false;
        }
    }
    public void EnableCollider()
    {
        if (TryGetComponent(out Collider coll))
        {
            coll.enabled = true;
        }
    }
}
