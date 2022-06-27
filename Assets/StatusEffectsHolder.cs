using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StatusEffectsHolder : MonoBehaviour
{
    public string sourceID;
    public List<StatusEffect> effects = new List<StatusEffect>();

    private void Awake()
    {
        sourceID = Guid.NewGuid().ToString();
    }

    private void Start()
    {
        foreach (var effect in effects)
        {
            effect.sourceID = sourceID;
        }
    }

}
