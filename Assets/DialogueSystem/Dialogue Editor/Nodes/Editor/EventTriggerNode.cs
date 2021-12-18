using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

using System;
using System.Linq;


public class EventTriggerNode : CustomNode
{
    public List<DialogueEventTrigger> allDialogueEventTriggers = new List<DialogueEventTrigger>();

}