using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "AI/Decisions/CheckIfSeeEnemies")]
public class AI_Decision_CheckEnemies : AI_Decision
{
    void OnValidate()
    {
        //dynamicValues.intValues.Add("swingDirection", 1);
        //dynamicValues.boolValues.Add("testBool", true);
        //dynamicValues.floatValues.Add("testFloat", 1);
        //Debug.Log("validated");
    }
    public override void InitializeWithBehavior(AI_Controller controller, AI_DecisionData decisionData)
    {

    }
    public override void Decide(AI_Controller controller, AI_DecisionData decisionData)
    {
        //Debug.Log("seeAnEnemy returns " + seeAnEnemy(controller));
        if (seeAnEnemy(controller))
        {
            decisionData.nextActionGUID = decisionData.trueGUID;
        }
        else
        {
            decisionData.nextActionGUID = decisionData.falseGUID;
        }
        
    }
    
    bool seeAnEnemy(AI_Controller controller)
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
