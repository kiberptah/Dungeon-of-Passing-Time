using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace DialogueSystem
{
    [Serializable]
    public class DialogueChoice
    {
        public string choiceName;
        public List<DialogueChoiceCondition> allChoiceConditions = new List<DialogueChoiceCondition>();
    }
}


