using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class AI_Action : ScriptableObject
{
    public abstract void StateEntered(AI_StateController controller);
    public abstract void Act(AI_StateController controller);
    public abstract void StateExited(AI_StateController controller);

}
