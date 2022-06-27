using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectsTrigger : MonoBehaviour
{
    public StatusEffectsHolder holder;
    public bool removeBuffOnExit = true;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("1 " + other.name);
        if (other.TryGetComponent(out StatusEffectsHandler handler))
        {
            foreach (var effect in holder.effects)
            {
                handler.ApplyEffect(effect);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (removeBuffOnExit)
        {
            if (other.TryGetComponent(out StatusEffectsHandler handler))
            {
                foreach (var effect in holder.effects)
                {
                    handler.RemoveEffectOfSource(holder.sourceID);
                }
            }
        }
    }
}
