using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SE_MovespeedMod : StatusEffect
{
    public float slowDown = 0.5f;
    ActorMovement actorMovement;

    public override bool TryAddingStatusEffect(StatusEffectsHolder holder, StatusEffectsHandler host)
    {
        if (holder.effects.OfType<SE_MovespeedMod>().Any() && !isStackable)
        {
            return false;
        }
        else
        {
            SE_MovespeedMod asd = holder.gameObject.AddComponent<SE_MovespeedMod>();
            UpdateGeneralValues(asd);
            UpdateSpecificValues(asd);

            asd.StatusStarts(host);

            return true;
        }
    }

    public void UpdateSpecificValues(SE_MovespeedMod effect)
    {
        effect.slowDown = slowDown;
    }


    public override void StatusStarts(StatusEffectsHandler host)
    {
        if (host.TryGetComponent(out actorMovement))
        {
            actorMovement.speedMods.Add(slowDown);
        }
    }
    public override void StatusContinues(StatusEffectsHandler host)
    {

    }

    public override void StatusEnds(StatusEffectsHandler host)
    {
        if (host.TryGetComponent(out actorMovement))
        {
            actorMovement.speedMods.Remove(slowDown);
        }
    }



  
}
