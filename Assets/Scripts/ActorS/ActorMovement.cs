using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorMovement : MonoBehaviour
{
    ActorStats actorStats;
    ActorStamina actorStamina;

    float speed;
    float dashCost;

    Rigidbody2D rb;

    public Vector3 movementDirection = Vector3.zero;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        actorStats = GetComponent<ActorStats>();
        actorStamina = GetComponent<ActorStamina>();

        speed = actorStats.walkSpeed;
        dashCost = actorStats.dashCost;
    }

    private void FixedUpdate()
    {
        if (movementDirection != Vector3.zero)
        {
            rb.AddRelativeForce(movementDirection * speed * actorStats.walkspeed_Modifiers, ForceMode2D.Force);
        }
    }

    public void InputMovement(Vector3 _movementDirection)
    {
        movementDirection = _movementDirection;
    }

    public void Dash()
    {
        if (actorStamina.currentStamina >= dashCost)
        {
            EventDirector.somebody_LoseStamina(transform, dashCost);
            StartCoroutine("SpeedBoost");
        }
    }
    float boostDuration = 0.1f;
    float boostMult = 3f;
    IEnumerator SpeedBoost()
    {
        speed = speed * boostMult;
        yield return new WaitForSeconds(boostDuration);
        speed = actorStats.walkSpeed;

        yield return null;
    }

}
