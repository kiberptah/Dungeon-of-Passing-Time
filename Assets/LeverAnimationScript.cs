using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverAnimationScript : MonoBehaviour
{
    Animator animator;
    public string LeverUp = "Lever_Up";
    public string LeverDown = "Lever_Down";

    public enum leverState
    {
        up,
        down
    }
    public leverState currentLeverState;

    private void Start()
    {
        animator = GetComponent<Animator>();
        correlateAnimationToState();
    }

    void correlateAnimationToState()
    {
        if (currentLeverState == leverState.up)
        {
            animator.Play(LeverUp);
        }
        if (currentLeverState == leverState.down)
        {
            animator.Play(LeverDown);
        }
    }

    public void FlipLever()
    {
        if (currentLeverState == leverState.up)
        {
            currentLeverState = leverState.down;
        }

        else if(currentLeverState == leverState.down)
        {
            currentLeverState = leverState.up;
        }


        correlateAnimationToState();

    }


}
