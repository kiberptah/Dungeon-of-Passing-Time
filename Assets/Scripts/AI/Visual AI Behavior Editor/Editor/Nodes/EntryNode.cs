using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using System;

namespace AI_BehaviorEditor
{
    public class EntryNode : CustomNode
    {
        public AI_EntryData data = new AI_EntryData();     
    }

    /*
    [System.Serializable]
    public class EntryNode_Data : CustomNode_Data
    {

    }
    */
}