using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class AttackCollisionCallback : MonoBehaviour
{
    public event Action<Transform> collisionCallback;

    /*
    public enum typeOfAttack
    {
        slash,
        pierce
    }
    public typeOfAttack selectTypeOfAttack;
    public Transform weaponMainObject;
    */
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out ActorHealth targetHealth))
        {
            collisionCallback?.Invoke(collision.transform);
        }
            
        /*
        switch (selectTypeOfAttack)
        {
            default:
                break;
            case typeOfAttack.slash:
                EventDirector.someBladeSlashCollision(weaponMainObject, collision.transform);
                break;
            case typeOfAttack.pierce:
                EventDirector.someBladePierceCollision(weaponMainObject, collision.transform);
                break;
        }
        */
    }
}
