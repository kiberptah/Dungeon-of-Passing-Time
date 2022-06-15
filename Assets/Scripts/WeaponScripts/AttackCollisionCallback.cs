using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class AttackCollisionCallback : MonoBehaviour
{
    public event Action<DamageReciever> collisionCallback;

    private void OnTriggerStay(Collider collision)
    {
        if (gameObject.activeSelf && collision.isTrigger)
        {
            if (collision.TryGetComponent(out DamageReciever target))
            {
                collisionCallback?.Invoke(target);
                //Debug.Log("hit: " + target.transform);
            }
        }



    }

}
