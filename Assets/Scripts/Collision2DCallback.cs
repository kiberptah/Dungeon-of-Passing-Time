using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Collision2DCallback : MonoBehaviour
{
    public Transform weaponMainObject;

    private void OnTriggerStay2D(Collider2D collision)
    {
        //EventDirector.someBladeCollision(weaponMainObject, collision.transform);
        print("DONT USE THIS SCRIPT!!!!");
    }
}
