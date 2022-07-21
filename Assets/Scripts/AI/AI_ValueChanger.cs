using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AI_ValueChanger : ScriptableObject
{
    //public AI_ValueChangerData data;
    public AI_DynamicValues dynamicValues = new AI_DynamicValues();


    public abstract void ChangeValue(AI_StateData stateData, AI_ValueData valueData);


}
public abstract class AI_ValueChanger_Int : AI_ValueChanger
{
    public override void ChangeValue(AI_StateData stateData, AI_ValueData valueData)
    {
        ChangeValue_Int(stateData, valueData);
    }
    protected abstract void ChangeValue_Int(AI_StateData stateData, AI_ValueData valueData);
}
public abstract class AI_ValueChanger_Float : AI_ValueChanger
{
    public override void ChangeValue(AI_StateData stateData, AI_ValueData valueData)
    {
        ChangeValue_Float(stateData, valueData);
    }
    protected abstract void ChangeValue_Float(AI_StateData stateData, AI_ValueData valueData);
}
public abstract class AI_ValueChanger_Bool : AI_ValueChanger
{
    public override void ChangeValue(AI_StateData stateData, AI_ValueData valueData)
    {
        ChangeValue_Bool(stateData, valueData);
    }
    protected abstract void ChangeValue_Bool(AI_StateData stateData, AI_ValueData valueData);
}