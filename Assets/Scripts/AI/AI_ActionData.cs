







[System.Serializable]
public class AI_ActionData : AI_Data
{
    public AI_StateData stateData;
    public AI_Action action;

    public AI_DynamicValues dynamicValues = new AI_DynamicValues();

    public string nextActionGUID = "";

}
