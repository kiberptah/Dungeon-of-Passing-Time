using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "AI/Decisions/CheckEnemies")]
public class AI_Decision_CheckEnemies : AI_Decision
{
    public override bool Decide(AI_StateController controller)
    {
        //Debug.Log("seeAnEnemy returns " + seeAnEnemy(controller));
        return seeAnEnemy(controller);
    }

    bool seeAnEnemy(AI_StateController controller)
    {

        foreach (var obj in controller.sightedObjects)
        {
            if (obj.TryGetComponent(out ActorStats objStats))
            {
                foreach (var hatedFaction in controller.actorStats.hatedFactions)
                {
                    foreach (var objFaction in objStats.belongsToFactions)
                    {
                        if (hatedFaction == objFaction)
                        {
                            controller.currentTarget = obj;
                            return true;
                        }
                    }

                }
                /* 
                if (Array.Exists(controller.actorStats.hatedFactions, element => element == objStats.faction))
                {
                    return true;
                } */
            }
            else
            {
                Debug.Log(obj.name + " doesnt have stats");
            }

        }
        return false;
    }
}
