using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;




namespace DialogueSystem
{
    [Serializable]
    public class RedirectionsNodeData : CustomNodeData
    {
        public List<DialogueChoiceCondition> redirections;
    }
}