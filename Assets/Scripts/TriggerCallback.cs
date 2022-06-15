using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerCallback : MonoBehaviour
{
    [System.Serializable]
    public class TriggerEvent : UnityEvent<Collider> { }

    public TriggerEvent triggerEnterCallback;
    public TriggerEvent triggerExitCallback;

    private void OnTriggerEnter(Collider collision)
    {
        triggerEnterCallback?.Invoke(collision);
    }

    private void OnTriggerExit(Collider collision)
    {
        triggerExitCallback?.Invoke(collision);
    }
}
