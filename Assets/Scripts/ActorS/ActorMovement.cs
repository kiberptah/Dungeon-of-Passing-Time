using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
public class ActorMovement : MonoBehaviour
{
    #region DEPENDENCIES
    ActorStamina actorStamina;
    ActorAnimManager animManager;
    #endregion

    #region References
    Rigidbody2D rb;
    #endregion

    public float walkSpeed = 45;
    public float dashForce = 6f;
    public float dashCost = 35;



    Vector3 movementDirection = Vector3.zero;
    Vector3 lastDirection = Vector3.zero;

    //public event Action<Vector2> UpdateMovementDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        actorStamina = GetComponent<ActorStamina>();
        animManager = GetComponent<ActorAnimManager>();
    }

    private void FixedUpdate()
    {
        Walk();
    }
    void Walk()
    {
        if (movementDirection != Vector3.zero)
        {
            animManager.UpdateAnimation("walk", movementDirection);

            rb.AddRelativeForce(movementDirection * walkSpeed, ForceMode2D.Force);
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

    public void Dash()
    {
        if (movementDirection != Vector3.zero)
        {
            if (actorStamina.TrySubtractingStamina(dashCost))
            {
                //rb.AddRelativeForce(movementDirection * dashForce, ForceMode2D.Impulse);
                rb.AddRelativeForce(lastDirection * dashForce, ForceMode2D.Impulse);
            }
        }
    }

    void ChangeWalkspeedBasedOnStamina()
    {
        // it is kinda shitty in terms of compatibility with multiple speed modifiers
        /* 
                actorStats.walkspeed_StaminaMod = minimalSpeedMod + (1 - minimalSpeedMod) * (currentStamina / maxStamina);

                if (currentStamina / maxStamina > speedDebuffThreshold)
                {
                    actorStats.walkspeed_StaminaMod = 1f;
                } */
    }

}
