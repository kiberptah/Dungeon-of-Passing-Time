using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class AttackCollisionCallback : MonoBehaviour
{
    public event Action<IHealth> collisionCallback;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (gameObject.activeSelf && collision.isTrigger)
        {
            if (collision.TryGetComponent(out DamageReciever target))
            {
                if (target.health != null)
                {
                    collisionCallback?.Invoke(target.health);
                }
                //Debug.Log("collisioner: " + gameObject.name);
            }
        }



    }

}
