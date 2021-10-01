using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float speed = 10f;
    float dashCost = 0;
    Vector3 movementDirection = Vector3.zero;
    ActorStats playerStats;
    ActorStamina playerStamina;

    Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerStats = GetComponent<ActorStats>();
        playerStamina = GetComponent<ActorStamina>();

        speed = playerStats.walkSpeed;
        dashCost = playerStats.dashCost;
    }
    private void Update()
    {
        
    }
    private void FixedUpdate()
    {
        //transform.position = Vector3.MoveTowards(transform.position, transform.position + movementDirection, speed * Time.deltaTime);
        if (movementDirection != Vector3.zero)
        {
            rb.AddRelativeForce(movementDirection * speed * playerStats.walkspeed_Modifiers, ForceMode2D.Force);

            //rb.MovePosition(transform.position + movementDirection * speed * Time.fixedDeltaTime);
        }
        //rb.MovePosition(transform.position + movementDirection * speed * Time.fixedDeltaTime);
    }

    public void InputMovement(Vector3 _movementDirection)
    {
        movementDirection = _movementDirection;
    }

    public void Dash()
    {

        if (playerStamina.currentStamina >= dashCost)
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
        speed = playerStats.walkSpeed;

        yield return null;
    }

}
