using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerCallback : MonoBehaviour
{
    [System.Serializable]
    public class TriggerEvent : UnityEvent<Collider2D> { }

    public TriggerEvent triggerEnterCallback;
    public TriggerEvent triggerExitCallback;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        triggerEnterCallback?.Invoke(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        triggerExitCallback?.Invoke(collision);
    }
}
