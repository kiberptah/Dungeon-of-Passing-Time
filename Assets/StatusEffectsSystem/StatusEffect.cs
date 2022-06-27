using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public abstract class StatusEffect : MonoBehaviour
{
    public string sourceID = "";
    public float maxDuration;
    public float currentDuration;
    public bool isEndless = false;
    public bool isStackable = false;


   

    public enum types
    {
        buff,
        debuff
    }
    public types type;


    public Damage.elemento elemento;



    private void Start()
    {
        GetComponent<StatusEffectsHolder>().effects.Add(this);
    }

    public abstract bool TryAddingStatusEffect(StatusEffectsHolder holder, StatusEffectsHandler host);

    public void UpdateGeneralValues(StatusEffect effect)
    {
        effect.sourceID = sourceID;
        effect.maxDuration = maxDuration;
        effect.currentDuration = currentDuration;
        effect.isEndless = isEndless;   
        effect.isStackable = isStackable;   
        effect.type = type;
        effect.elemento = elemento;
    }
    //public abstract void UpdateSpecificValues(StatusEffect effect);

    public abstract void StatusStarts(StatusEffectsHandler host);
    public abstract void StatusContinues(StatusEffectsHandler host);
    public abstract void StatusEnds(StatusEffectsHandler host);
}
