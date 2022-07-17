using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

using System.IO;
using UnityEditor;




namespace AI_BehaviorEditor
{
    public static class AIGraph_SaveUtility
    {
        //static string saveFilePath = "Assets/AIGraph/Resources/AIGraph_Data/";
        static string saveFilePath = "Assets/Resources/AIGraph_SaveData/";
        //static AIGraphView targetGraphView; // reference to a graphview


        public static void SaveGraph(AIGraphView targetGraphView, AIGraph_SaveData data, string filename)
        {
            AIGraph_SaveData newSaveData = ScriptableObject.CreateInstance<AIGraph_SaveData>();
            newSaveData.name = filename;
            newSaveData.filename = filename;

            // --- SAVE NODES
            foreach (var node in targetGraphView.nodes)
            {
                ((CustomNode)node).nodeData.nodeRect = node.GetPosition();
                newSaveData.nodesData.Add(((CustomNode)node).nodeData);

                if (node.GetType() == typeof(StateNode))
                {
                    newSaveData.statesData.Add(((StateNode)node).data);
                }
                if (node.GetType() == typeof(ActionNode))
                {
                    newSaveData.actionsData.Add(((ActionNode)node).data);
                }
                if (node.GetType() == typeof(DecisionNode))
                {
                    newSaveData.decisionsData.Add(((DecisionNode)node).data);
                }
                if (node.GetType() == typeof(TimerNode))
                {
                    newSaveData.timersData.Add(((TimerNode)node).data);
                    Debug.Log("saved timer node with guid = " + ((TimerNode)node).data.GUID);
                    Debug.Log("saved timer node with nodeguid = " + ((TimerNode)node).nodeData.nodeGUID);
                }
                if (node.GetType() == typeof(ValueNode))
                {
                    newSaveData.valuesData.Add(((ValueNode)node).data);
                }
                if (node.GetType() == typeof(ValueChangerNode))
                {
                    newSaveData.valueChangersData.Add(((ValueChangerNode)node).data);
                }
                if (node.GetType() == typeof(TransitionNode))
                {
                    newSaveData.transitionsData.Add(((TransitionNode)node).data);
                }
            }

            // --- SAVE EDGES
            {
                List<Edge> edges = targetGraphView.edges.ToList();
                foreach (var edge in edges)
                {
                    // --- to avoid visualgraph has a bug where you end up with unconnected edge, so it prevents it from saving
                    if (edge.output.node == null || edge.input.node == null)
                        continue;
                    // ---
                    var outputNode = edge.output.node as CustomNode;
                    var inputNode = edge.input.node as CustomNode;

                    newSaveData.edgesData.Add(new EdgesData
                    {
                        outputPortName = edge.output.portName,
                        outputNodeGUID = outputNode.nodeData.nodeGUID,

                        targetPortName = edge.input.portName,
                        targetNodeGUID = inputNode.nodeData.nodeGUID
                    });
                }
            }

            /// ----- SAVE SCRIPTABLE OBJECT OF DIALOGUE CONTAINER TO A SPECIAL FOLDER
            {
                // ----- create resources folder if it doesnt exist
                Directory.CreateDirectory(saveFilePath);

                // ----- SAVE it OR OVERRIDE it if file already exists!!! important overwise serializefields of dialoguetriggers get reset
                
                //AIGraph_SaveData existingContainer = Resources.Load<AIGraph_SaveData>("AIGraph_SaveData/" + data.name);
                if (data != null)
                {
                    AIGraph_SaveData existingContainer = Resources.Load<AIGraph_SaveData>("AIGraph_SaveData/" + data.name);
                    if (existingContainer != null)
                    {
                        Debug.Log("rewriting ai savedata");
                        EditorUtility.SetDirty(existingContainer); // there's data loss on reload without this shit
                        existingContainer = newSaveData;
                    }
                    else
                    {
                        Debug.Log("ai savedata created");
                        AssetDatabase.CreateAsset(newSaveData, path: saveFilePath + $"{filename}.asset");
                    }
                }
                else
                {
                    Debug.Log("ai savedata created");
                    AssetDatabase.CreateAsset(newSaveData, path: saveFilePath + $"{filename}.asset");
                }


                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            
        }
        
        public static void LoadGraph(AIGraphView targetGraphView, AIGraph_SaveData saveData)
        {
            
            // --- Clear GraphView First
            targetGraphView.ClearGraph();

            // --- Load Nodes
            GenerateLoadedNodes();

            // --- Load Edges
            ConnectLoadedNodes();




            

            void GenerateLoadedNodes()
            {
                foreach (var node in saveData.actionsData)
                {
                    Debug.Log("Load Action Node");
                    targetGraphView.AddElement(targetGraphView.Create_ActionNode(node));                
                }

                foreach (var node in saveData.decisionsData)
                {
                    targetGraphView.AddElement(targetGraphView.Create_DecisionNode(node));
                }

                foreach (var node in saveData.timersData)
                {

                    Debug.Log("load timer node with guid = " + node.GUID);

                    targetGraphView.AddElement(targetGraphView.Create_TimerNode(node));

                }

                foreach (var node in saveData.valuesData)
                {
                    targetGraphView.AddElement(targetGraphView.Create_ValueNode(node));
                }

                foreach (var node in saveData.valueChangersData)
                {
                    targetGraphView.AddElement(targetGraphView.Create_ValueChangerNode(node));
                }

                foreach (var node in saveData.transitionsData)
                {
                    targetGraphView.AddElement(targetGraphView.Create_TransitionNode(node));
                }


                // state nodes should be created the last ??? or not
                foreach (var node in saveData.statesData)
                {
                    targetGraphView.AddElement(targetGraphView.Create_StateNode(node));

                    Debug.Log("load state node?");                 
                }


                foreach (var nodeData in saveData.nodesData)
                {
                    CustomNode tempNode = targetGraphView.FindCustomNodeByGUID(nodeData.nodeGUID);
                    if (tempNode != null)
                    {
                        tempNode.SetPosition(nodeData.nodeRect);
                    }
                }


            }

            void ConnectLoadedNodes()
            {
                /// we have generated nodes and ports and now we need to connect them all
                ///
                var loadedEdges = saveData.edgesData;
                foreach (var loadedEdge in loadedEdges)
                {
                    var tempEdge = new Edge();

                    foreach (var port in targetGraphView.ports)
                    {
                        CustomNode portNode = (CustomNode)port.node;
                        if (port.portName == loadedEdge.outputPortName && portNode.nodeData.nodeGUID == loadedEdge.outputNodeGUID)
                        {
                            tempEdge.output = port;                        
                        }
                        else if (port.portName == loadedEdge.targetPortName && portNode.nodeData.nodeGUID == loadedEdge.targetNodeGUID)
                        {
                            tempEdge.input = port;
                        }
                    }

                    tempEdge.input.Connect(tempEdge);
                    tempEdge.output.Connect(tempEdge);
                    targetGraphView.Add(tempEdge);
                }
            }
            




        }
        
    }

    
}