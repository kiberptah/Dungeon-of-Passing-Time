using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public abstract class AI_Decision : ScriptableObject
{
    public AI_DynamicValues dynamicRefValues = new AI_DynamicValues();


    public abstract void InitializeWithBehavior(AI_Controller controller, AI_DecisionData decisionData);
    public abstract void Decide(AI_Controller controller, AI_DecisionData decisionData);
}
