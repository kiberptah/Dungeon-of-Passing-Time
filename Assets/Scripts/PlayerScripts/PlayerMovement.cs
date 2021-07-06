using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float speed = 10f;
    Vector3 movementDirection = Vector3.zero;
    ActorStats playerStats;

    private void Awake()
    {
        playerStats = GetComponent<ActorStats>();

        speed = playerStats.walkSpeed;
    }
    private void Update()
    {
        InputMovement();
    }
    private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + movementDirection, speed * Time.deltaTime);
    }

    void InputMovement()
    {
        movementDirection.x = Input.GetAxis("Horizontal");
        movementDirection.y = Input.GetAxis("Vertical");

        movementDirection = movementDirection.normalized;


        if (Input.GetButtonDown("Dash"))
        {
            Dash();
        }
    }

    public void Dash()
    {
        StartCoroutine("SpeedBoost");
    }

    float boostDuration = 0.1f;
    float boostMult = 5f;
    IEnumerator SpeedBoost()
    {
        speed = speed * boostMult;
        yield return new WaitForSeconds(boostDuration);
        speed = playerStats.walkSpeed;

        yield return null;
    }
}
