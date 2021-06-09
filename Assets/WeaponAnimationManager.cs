using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimationManager : MonoBehaviour
{
    public Animator animator;
    string currentState;
    //WeaponStats.Attack currentAttack = new WeaponStats.Attack();

    private void OnEnable()
    {
        EventDirector.someBladeAttackStarted += ChangeAnimationState;
    }
    private void OnDisable()
    {
        EventDirector.someBladeAttackStarted -= ChangeAnimationState;
    }
    private void ChangeAnimationState(Transform blade, WeaponStats.Attack attack)
    {
        string newState = attack.attackType.ToString();
        if (currentState == newState)
        {
            return;
        }

        //currentAttack = attack;
        animator.CrossFade(newState, 0.1f);
    }

}
