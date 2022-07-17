
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AI_BehaviorData
{
    public AI_Controller controller;

    public string previousStateGUID = "";
    public string currentStateGUID = "";

    public Dictionary<string, AI_StateData> statesData = new Dictionary<string, AI_StateData>();

}

