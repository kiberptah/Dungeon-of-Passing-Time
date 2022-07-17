using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;



namespace DialogueSystem
{
    [Serializable]
    public class DialogueChoiceCondition
    {
        public string choiceGUID;
        public string nodeGUID;

        public string currentVarName;
        public string logicSign;
        public string comparedVarName;

        public DialogueChoiceCondition()
        {
            choiceGUID = Guid.NewGuid().ToString();
        }

        public void addCurrentVariable(string _var)
        {
            currentVarName = _var;
        }
        public void addComparedVariable(string _var)
        {
            comparedVarName = _var;
        }
        public void addLogicSign(string _sign)
        {
            logicSign = _sign;
        }

        public bool CheckCondition()
        {
            StoryVariablesContainer storyVariables = Resources.Load<StoryVariablesContainer>(path: StoryVariablesContainer.storyVariablesPath);
            if (storyVariables == null)
            {
                CustomRuntimeTools.DebugDump("StoryVariables file not found. This is bad. Really bad.");
                //EditorUtility.DisplayDialog(title: "Error!@#$", message: "StoryVariables file not found", ok: "Fuck...");
                return false;
            }

            StoryVariable currentVar = storyVariables.varList.Where(v => v.varName == currentVarName).FirstOrDefault();
            StoryVariable comparedVar = storyVariables.varList.Where(v => v.varName == comparedVarName).FirstOrDefault();
            if (currentVar == null || comparedVar == null)
            {
                return true;
            }


            bool result = false;

            float? floatCurrentVar = null;
            float? floatComparedVar = null;


            if (currentVar.varType == StoryVariable.dataType.floatType || currentVar.varType == StoryVariable.dataType.intType)
            {
                floatCurrentVar = float.Parse(currentVar.rawValue);
            }
            if (comparedVar.varType == StoryVariable.dataType.floatType || comparedVar.varType == StoryVariable.dataType.intType)
            {
                floatComparedVar = float.Parse(comparedVar.rawValue);
            }


            switch (logicSign)
            {
                case ("=="):
                    if (currentVar.rawValue == comparedVar.rawValue)
                    {
                        result = true;
                    }
                    break;

                case ("!="):
                    if (currentVar != comparedVar)
                    {
                        result = true;
                    }
                    break;

                case (">"):
                    if (floatCurrentVar != null && floatComparedVar != null)
                    {
                        if (floatCurrentVar > floatComparedVar)
                        {
                            result = true;
                        }
                    }
                    break;

                case (">="):
                    if (floatCurrentVar != null && floatComparedVar != null)
                    {
                        if (floatCurrentVar >= floatComparedVar)
                        {
                            result = true;
                        }
                    }
                    break;

                case ("<"):
                    if (floatCurrentVar != null && floatComparedVar != null)
                    {
                        if (floatCurrentVar < floatComparedVar)
                        {
                            result = true;
                        }
                    }
                    break;

                case ("<="):
                    if (floatCurrentVar != null && floatComparedVar != null)
                    {
                        if (floatCurrentVar <= floatComparedVar)
                        {
                            result = true;
                        }
                    }
                    break;
            }

            return result;
        }

        /*
        public void addValueToCompare(string _value)
        {
            //rawValue = _value;
            currentVariable.rawValue = _value;
        }
        */
    }
}