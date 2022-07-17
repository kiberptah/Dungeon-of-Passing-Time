using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class AI_DynamicValues 
{
    public Dictionary<string, string> valueNodeGUID; // to reference value nodes

    public Dictionary<string, float> floatValues = new Dictionary<string, float>();
    public Dictionary<string, int> intValues = new Dictionary<string, int>();
    public Dictionary<string, bool> boolValues = new Dictionary<string, bool>();


}
