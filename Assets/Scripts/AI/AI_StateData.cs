using System.Collections.Generic;


[System.Serializable]
public class AI_StateData : AI_Data
{
    public AI_BehaviorData behavior;


    public string stateName = "";
    public string nextActionGUID = "";

    public string loopActionGUID = "";

    public string onEnterActionGUID = "";
    public string onExitActionGUID = "";

    public string decisionGUID = "";

    public List<string> childNodesGUID = new List<string>();

    public Dictionary<string, AI_ActionData> actionsData = new Dictionary<string, AI_ActionData>();
    public Dictionary<string, AI_Action> actions = new Dictionary<string, AI_Action>();
    public Dictionary<string, AI_DecisionData> decisionsData = new Dictionary<string, AI_DecisionData>();
    public Dictionary<string, AI_Decision> decisions = new Dictionary<string, AI_Decision>();

    public Dictionary<string, AI_ValueData> valuesData = new Dictionary<string, AI_ValueData>();
}
