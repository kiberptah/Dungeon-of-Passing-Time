using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DialogueSystem
{
    [Serializable]
    public class EventTriggerNodeData : CustomNodeData
    {
        public List<DialogueEventTrigger> dialogueEvents;
    }
}