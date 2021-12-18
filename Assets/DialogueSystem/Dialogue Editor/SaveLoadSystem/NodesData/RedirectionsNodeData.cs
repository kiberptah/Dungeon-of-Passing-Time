using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class RedirectionsNodeData : CustomNodeData
{
    public List<DialogueChoiceCondition> redirections;
}