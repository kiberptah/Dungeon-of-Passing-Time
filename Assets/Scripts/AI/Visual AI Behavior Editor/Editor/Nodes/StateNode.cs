using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace AI_BehaviorEditor
{
    public class StateNode : CustomNode
    {
        public AI_StateData data = new AI_StateData();
        public AIGraphView graphView; // don't forget to specify it!!!

        //public List<CustomNode> childNodes = new List<CustomNode>();



        public StateNode(AIGraphView graphView)
        {
            this.graphView = graphView;
        }

        public void AddChild(CustomNode node)
        {
            data.childNodesGUID.Add(node.nodeData.nodeGUID);
            //childNodes.Add(node);     
        }

        public override void OnSelected()
        {
            for (int i = 0; i < data.childNodesGUID.Count; i++)
            {
                CustomNode tempNode = graphView.FindCustomNodeByGUID(data.childNodesGUID[i]);
                if (tempNode != null)
                {
                    graphView.AddToSelection(tempNode);
                }
                else
                {
                    data.childNodesGUID.RemoveAt(i);
                    i--;
                }
            }
        }
        /*
        public override void OnSelected()
        {
            for (int i = 0; i < childNodes.Count; i++)
            {
                if (graphView.Contains(childNodes[i]))
                {
                    graphView.AddToSelection(childNodes[i]);
                }
                else
                {
                    childNodes.Remove(childNodes[i]);
                    i--;
                }
            }      
        }
        */
        /*
        public void LoadChildren()
        {
            foreach (var guid in data.childNodesGUID)
            {
                CustomNode tempNode = graphView.FindCustomNodeByGUID(guid);
                if (tempNode != null)
                {
                    childNodes.Add(tempNode);
                }
            }
        }
        */
        

    }
    
}