using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    PlayerController playerController;
    Transform target;
    Rigidbody2D rb;
    public float force = 1f;

    public float minimalOffset = 0.5f;
    void Awake()
    {
        playerController = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>();
        target = playerController.actorConnector.transform;
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        FollowTarget();
    }

    void FollowTarget()
    {
        if (Vector2.Distance(target.position, transform.position) > minimalOffset
            || (Vector2.Distance(target.position, transform.position) < minimalOffset && rb.velocity != Vector2.zero))
        {
            //force = Vector2.Distance(target.position, transform.position);
            rb.AddRelativeForce((target.position - transform.position) * force, ForceMode2D.Force);

        }
        else
        {
            //transform.position = target.position;
        }
    }
}
