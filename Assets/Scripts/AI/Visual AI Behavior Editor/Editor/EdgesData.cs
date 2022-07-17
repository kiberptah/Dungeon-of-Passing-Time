using UnityEngine;
using UnityEditor.Experimental.GraphView;

namespace AI_BehaviorEditor
{
    [System.Serializable]
    public class EdgesData
    {
        public string outputPortName;
        public string outputNodeGUID;


        public string targetPortName;
        public string targetNodeGUID;
    }
}