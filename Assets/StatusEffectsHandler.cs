using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;



public class StatusEffectsHandler : MonoBehaviour
{
    public StatusEffectsHolder holder;

    public List<StatusEffect> appliedEffects = new List<StatusEffect>();
    public List<Damage.elemento> immunityToBuffs = new List<Damage.elemento>();
    public List<Damage.elemento> immunityToDebuffs = new List<Damage.elemento>();



    private void Start()
    {
        // temporary solutiom
        if (holder == null)
        {
            this.enabled = false;
        }
    }
    public void ApplyEffect(StatusEffect effect)
    {

        if (immunityToBuffs.Contains(effect.elemento) && effect.type == StatusEffect.types.buff
            || immunityToDebuffs.Contains(effect.elemento) && effect.type == StatusEffect.types.debuff)        
        {
            Debug.Log("failed applying statuf effect to " + transform.name);

            return;
        }
        else
        {

            if (effect.TryAddingStatusEffect(holder, this))
            {
                Debug.Log("status effect applied to " + transform.name);

            }
            //appliedEffects.Add(effect);
            //effect.StatusStarts(this);
        }
    }


    private void Update()
    {
        foreach (var effect in holder.effects)
        {
            effect.StatusContinues(this);

            if (!effect.isEndless)
            {
                effect.currentDuration += Time.deltaTime;
                if (effect.currentDuration > effect.maxDuration)
                {
                    RemoveEffect(effect);
                }
            }            
        }
    }

    void RemoveEffect(StatusEffect effect)
    {
        effect.StatusEnds(this);
        holder.effects.Remove(effect);
        Destroy(effect);
    }

    public void RemoveEffectOfSource(string sourceID)
    {
        foreach (var effect in holder.effects)
        {         
            if (effect.sourceID == sourceID)
            {
                RemoveEffect(effect);
            }
        }
    }
}
