using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

using System;
using System.Linq;


namespace DialogueSystem
{
    public class RedirectionsNode : CustomNode
    {
        //public string GUID;

        //public List<ChoiceCondition> allChoiceConditions = new List<ChoiceCondition>();
        public List<DialogueChoiceCondition> allRedirectionConditions = new List<DialogueChoiceCondition>();
    }
}