using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class AI_ValueData : AI_Data
{
    public AI_StateData stateData;
    //public System.Type type;
    public enum supportedTypes
    {
        unassigned,

        type_int,
        type_float,
        type_bool
    }
    public supportedTypes valueType;

    public int intValue = 0;
    public float floatValue = 0;
    public bool boolValue = false;

    public AI_DynamicValues dynamicValues = new AI_DynamicValues();

}
