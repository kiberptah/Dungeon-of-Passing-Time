
using System;






[Serializable]
public class DialogueEventTrigger
{
    public string nodeGUID;
    public string eventName = "";

    public DialogueEventTrigger(string _nodeGUID)
    {
        nodeGUID = _nodeGUID;
    }

}