using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class AttackCollisionCallback : MonoBehaviour
{
    public event Action<Transform> collisionCallback;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out ActorHealth targetHealth))
        {
            collisionCallback?.Invoke(collision.transform);
        }
    }
}
