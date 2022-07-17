using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class AI_ValueChangerData : AI_Data
{
    //public System.Type type;

    public AI_DynamicValues dynamicValues = new AI_DynamicValues();

    public AI_ValueChanger valueChanger = null;



    public enum supportedTypes
    {
        unassigned,

        type_int,
        type_float,
        type_bool
    }
    public supportedTypes valueType;
    /*
    public AI_ValueChanger_Int valueChanger_Int = null;
    public AI_ValueChanger_Float valueChanger_Float = null;
    public AI_ValueChanger_Bool valueChanger_Bool = null;
    */
}
