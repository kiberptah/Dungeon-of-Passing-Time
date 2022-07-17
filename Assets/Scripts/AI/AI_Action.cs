using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class AI_Action : ScriptableObject
{

    public AI_DynamicValues dynamicValues = new AI_DynamicValues();
    
    /* probably delete
    public void SetUpDynamicValues(Dictionary<string, float> floatValues, Dictionary<string, int> intValues, Dictionary<string, bool> boolValues)
    {
        dynamicValues.floatValues = floatValues;
        dynamicValues.intValues = intValues;    
        dynamicValues.boolValues = boolValues;
    }
    */

    public abstract void InitializeWithBehavior(AI_Controller controller, AI_ActionData actionData);
    public abstract void Act(AI_Controller controller, AI_StateData stateData, AI_ActionData actionData);
}







/*
//example
public override void InitializeWithBehavior(AI_Controller controller, AI_ActionData actionData)
{
    ActionData_SwingWeapon data = (ActionData_SwingWeapon)actionData;
    actionData = data;
}

public void UpdateValues(AI_StateData stateData, AI_ActionData actionData)
{
    foreach (var value in actionData.dynamicValues.intValues)
    {
        actionData.dynamicValues.intValues[value.Key] = stateData.valuesData[value.Key].intValue.Value;
    }
}

public override void Act(AI_Controller controller, AI_StateData stateData, AI_ActionData actionData)
{
            ActionData_SwingWeapon data = (ActionData_SwingWeapon)actionData;
        UpdateValues(stateData, data);


sdfasdfasdfasdf

    actionData = data;
}

*/