using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu]
[Serializable]
public class StoryVariablesContainer : ScriptableObject
{
    public static string storyVariablesPath = "StoryData/StoryVariables";

    public List<StoryVariable> varList = new List<StoryVariable>();


}

