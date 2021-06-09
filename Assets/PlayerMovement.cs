using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10f;
    Vector3 movementDirection = Vector3.zero;

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
    }
}
