using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class StoryVariable
{
    public string varName;
    public enum dataType
    {
        stringType,
        boolType,
        intType,
        floatType

    }
    public dataType varType;

    public string rawValue;
    public StoryVariable()
    {
        varName = "";
        varType = dataType.stringType;
        rawValue = "";
    }
    /*
    public string stringValue = "unassigned";
    public int intValue = 0;
    public float floatValue = 0;
    public bool boolValue = false;
    */
    public StoryVariable(bool _boolVar)
    {
        //boolValue = _boolVar;
        rawValue = _boolVar.ToString();
    }
    public StoryVariable(string _stringVar)
    {
        //stringValue = _stringVar;
        rawValue = _stringVar;
    }
    public StoryVariable(int _intVar)
    {
        //intValue = _intVar;
        rawValue = _intVar.ToString();

        int test;
        int.TryParse(rawValue, out test);
    }
    public StoryVariable(float _floatVar)
    {
        //floatValue = _floatVar;
        rawValue = _floatVar.ToString();

        float test;
        float.TryParse(rawValue, out test);
    }

}
