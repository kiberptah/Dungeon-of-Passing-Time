using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AI_SingleAction : ScriptableObject
{
    public abstract void Act(AI_StateController controller);
}
