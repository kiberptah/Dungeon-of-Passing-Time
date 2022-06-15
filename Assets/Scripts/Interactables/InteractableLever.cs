using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableLever : MonoBehaviour, IInteractable
{
    bool isLeverUp = true;
    public Sprite leverUp;
    public Sprite leverDown;
    public SpriteRenderer sprite;
    public UnityEvent LeverDown;
    public UnityEvent LeverUp;

    bool isAvailableforInteraction = true;
    private void Start()
    {
        ChangeSpriteToState();
    }

    public void OnInteract(Transform interactor)
    {
        if (isAvailableforInteraction)
        {
            if (isLeverUp)
            {
                isLeverUp = false;
                LeverDown?.Invoke();
            }
            else
            {
                isLeverUp = true;
                LeverUp?.Invoke();
            }

            ChangeSpriteToState();
        }
    }

    void ChangeSpriteToState()
    {
        if (isLeverUp)
        {
            sprite.sprite = leverUp;
        }
        else
        {
            sprite.sprite = leverDown;
        }
    }

}
