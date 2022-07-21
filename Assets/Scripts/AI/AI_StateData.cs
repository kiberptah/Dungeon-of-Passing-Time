using System.Collections.Generic;


[System.Serializable]
public class AI_StateData : AI_Data
{
    public AI_BehaviorData behavior;

    public string stateName = "";


    public string nextActionGUID = "";
    public string nextLoopActionGUID = "";

    public string onEnterActionGUID = "";
    public string onExitActionGUID = "";

    public string nextDecisionGUID = "";




    public List<string> childNodesGUID = new List<string>();

    public Dictionary<string, AI_ActionData> actionsData = new Dictionary<string, AI_ActionData>();
    public Dictionary<string, AI_DecisionData> decisionsData = new Dictionary<string, AI_DecisionData>();
    public Dictionary<string, AI_TimerData> timersData = new Dictionary<string, AI_TimerData>();
    public Dictionary<string, AI_ValueData> valuesData = new Dictionary<string, AI_ValueData>();
    public Dictionary<string, AI_ValueChangerData> valueChangersData = new Dictionary<string, AI_ValueChangerData>();
    public Dictionary<string, AI_TransitionData> transitionsData = new Dictionary<string, AI_TransitionData>();

}
