using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class ProximityTrigger : MonoBehaviour
{
    public UnityEvent trigger;


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Actor")
        {
            trigger.Invoke();
        }
    }
}
