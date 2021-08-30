using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollisionCallback : MonoBehaviour
{
    public enum typeOfAttack
    {
        slash,
        pierce
    }
    public typeOfAttack selectTypeOfAttack;
    public Transform weaponMainObject;

    private void OnTriggerStay2D(Collider2D collision)
    {
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
        //print("collision with blade");
    }
}
