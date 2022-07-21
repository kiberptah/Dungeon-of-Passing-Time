


[System.Serializable]
public class AI_TimerData : AI_Data
{
    public AI_StateData stateData;

    public float timeInterval = 0;
    public float intervalRandomOffset = 0;

    public float currentRandomOffset = 0;
    public float currentTime = 0;



    public string trueGUID = "";
    public string falseGUID = "";

    public string nextGUID = "";
}
