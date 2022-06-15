using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public bool smoothFollow = true;
    PlayerController playerController;
    Transform target;
    Rigidbody rb;
    public float force = 1f;
    public float lerp = 0.5f;

    public float minimalOffset = 0.5f;
    void Awake()
    {
        playerController = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>();
        target = playerController.actorConnector.transform;
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (smoothFollow)
        {
            FollowTarget();
        }
    }
    void Update()
    {
        if (!smoothFollow)
        {
            StickFollow();
        }
    }
    void FollowTarget()
    {
        if (Vector3.Distance(target.position, transform.position) > minimalOffset)
                //|| (Vector3.Distance(target.position, transform.position) < minimalOffset && rb.velocity != Vector3.zero))
        {
            //rb.AddRelativeForce((target.position - transform.position) * force, ForceMode.Force);


        }
        transform.position = Vector3.Lerp(transform.position, new Vector3(target.position.x, transform.position.y, target.position.z), lerp);

    }

    void StickFollow()
    {
        transform.position = target.position;
    }
}
