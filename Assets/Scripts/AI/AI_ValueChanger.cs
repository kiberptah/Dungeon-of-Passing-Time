using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AI_ValueChanger : ScriptableObject
{
    public AI_ValueChangerData data;
    public AI_DynamicValues dynamicValues = new AI_DynamicValues();

    


    
}
public abstract class AI_ValueChanger_Int : AI_ValueChanger
{
    public abstract void ChangeValue(AI_ValueData data);
}
public abstract class AI_ValueChanger_Float : AI_ValueChanger
{
    public abstract void ChangeValue(AI_ValueData data);
}
public abstract class AI_ValueChanger_Bool : AI_ValueChanger
{
    public abstract void ChangeValue(AI_ValueData data);
}