using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class ProximityTrigger : MonoBehaviour
{
    public UnityEvent trigger;


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Actor")
        {
            trigger.Invoke();
        }
    }
}
