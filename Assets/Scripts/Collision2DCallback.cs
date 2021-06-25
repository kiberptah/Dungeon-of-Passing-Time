using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Collision2DCallback : MonoBehaviour
{
    

    private void OnTriggerStay2D(Collider2D collision)
    {
        EventDirector.someBladeCollision(transform.parent, collision.transform);
        //print("collision with blade");
    }
}
