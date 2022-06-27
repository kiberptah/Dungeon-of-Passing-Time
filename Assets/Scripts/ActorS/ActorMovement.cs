using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
public class ActorMovement : MonoBehaviour
{
    ActorAnimManager animManager;
    Rigidbody rb;

    public float walkSpeed = 45;
    //[HideInInspector] public float speedMod = 1f;
    public List<float> speedMods = new List<float>();

    Vector3 movementDirection = Vector3.zero;
    Vector3 lastDirection = Vector3.zero;

    #region Init
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        //actorStamina = GetComponent<ActorStamina>();
        animManager = GetComponent<ActorAnimManager>();
    }
    #endregion

    private void FixedUpdate()
    {
        Walk();
    }

    
    void Walk()
    {
        if (movementDirection != Vector3.zero)
        {
            animManager.UpdateAnimation("walk", movementDirection);

            float speedMod = 1f;
            foreach (var mod in speedMods)
            {
                speedMod *= mod;
            }

            rb.AddRelativeForce(movementDirection * walkSpeed * speedMod, ForceMode.Force);
            movementDirection = Vector3.zero; // to avoid bugs when input is no longer present
        }
        else
        {
            animManager.UpdateAnimation("idle", lastDirection);
        }
    }

    public void UpdateMovement(Vector3 _movementDirection)
    {
        movementDirection = _movementDirection;

        if (movementDirection != Vector3.zero)
        {
            lastDirection = movementDirection.normalized;
        }
    }

    //public void Dash()
    //{
    //    if (movementDirection != Vector3.zero)
    //    {
    //        if (actorStamina.TrySubtractingStamina(dashCost))
    //        {
    //            rb.AddRelativeForce(lastDirection * dashForce, ForceMode.Impulse);
    //        }
    //    }
    //}


}
