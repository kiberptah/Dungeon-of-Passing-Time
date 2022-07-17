using UnityEditor.Experimental.GraphView;
using System.Collections.Generic;
using UnityEngine;

namespace AI_BehaviorEditor
{
    public class CustomNode : Node
    {
        public CustomNode_Data nodeData = new CustomNode_Data();
    }


    [System.Serializable]
    public class CustomNode_Data
    {
        public string nodeGUID;
        public Rect nodeRect;
    }
}