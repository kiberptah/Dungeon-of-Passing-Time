using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;

using UnityEngine;
using UnityEngine.UIElements;

using UnityEditor;
using UnityEditor.Experimental.GraphView;



public class GraphSaveUtility
{
    string saveFilePath = "Assets/DialogueSystem/Resources/SavedDialogues/";
    string localizationDefaultPath = "Assets/DialogueSystem/Resources/Localization/EN/Dialogues/";




    DialogueGraphView targetGraphView; // reference to a graphview
    public static GraphSaveUtility GetInstance(DialogueGraphView _targetGraphView)
    {
        return new GraphSaveUtility
        {
            targetGraphView = _targetGraphView
        };
    }


    DialogueContainer dialogueContainer; // container with all the data for save/load 
    
    public void SaveGraph(string _filename)
    {
        //Debug.Log("SAVING GRAPH");

        var dialogueContainer = ScriptableObject.CreateInstance<DialogueContainer>();
        dialogueContainer.filename = _filename;

        // ----- FIND NODES
        EntryNode entryNode = new EntryNode();
        List<EndNode> endNodes = new List<EndNode>();
        List<DialogueNode> dialogueNodes = new List<DialogueNode>();
        List<RedirectionsNode> redirectionsNodes = new List<RedirectionsNode>();
        List<EventTriggerNode> eventNodes = new List<EventTriggerNode>();
        List<CommentsNode> commentsNodes = new List<CommentsNode>();


        foreach (var node in targetGraphView.nodes)
        {
            if (node.GetType() == new EntryNode().GetType())
            {
                entryNode = (EntryNode)node;
            }

            if (node.GetType() == new DialogueNode().GetType())
            {
                dialogueNodes.Add((DialogueNode)node);
            }
            if (node.GetType() == new RedirectionsNode().GetType())
            {
                redirectionsNodes.Add((RedirectionsNode)node);
            }          
            if (node.GetType() == new EventTriggerNode().GetType())
            {
                eventNodes.Add((EventTriggerNode)node);
            }
            if (node.GetType() == new CommentsNode().GetType())
            {
                commentsNodes.Add((CommentsNode)node);
            }

            if (node.GetType() == new EndNode().GetType())
            {
                endNodes.Add((EndNode)node);
            }

        }

        // ----- SAVE NODES
        {
            // ----- Save Entry Node
            {
                var tempNode = new EntryNodeData();
                tempNode.guid = entryNode.GUID;
                tempNode.rect = entryNode.GetPosition();

                dialogueContainer.entryNodeData = tempNode;
            }
            // ----- Save dialogue nodes
            foreach (DialogueNode dialogueNode in dialogueNodes)
            {
                var tempNode = new DialogueNodeData();

                tempNode.rect = dialogueNode.GetPosition();
                tempNode.guid = dialogueNode.GUID;

                tempNode.dialogueTitle = dialogueNode.title;
                tempNode.dialogueText = dialogueNode.dialogueText;
                tempNode.speakerName = dialogueNode.speakerName;
                tempNode.speakerPortrait = dialogueNode.speakerPortrait;

                tempNode.choices = dialogueNode.choices;


                dialogueContainer.dialogueNodesData.Add(tempNode);
            }
            // ----- Save Redirection nodes
            foreach (RedirectionsNode redirectionsNode in redirectionsNodes)
            {
                var tempNode = new RedirectionsNodeData();

                tempNode.guid = redirectionsNode.GUID;
                tempNode.rect = redirectionsNode.GetPosition();

                //tempNode.conditions = conditionsNode.allChoiceConditions;
                tempNode.redirections = redirectionsNode.allRedirectionConditions;

                dialogueContainer.redirectionsNodesData.Add(tempNode);
            }
            // ----- Save Event Nodes
            {
                foreach (EventTriggerNode eventNode in eventNodes)
                {
                    var tempNode = new EventTriggerNodeData();

                    tempNode.guid = eventNode.GUID;
                    tempNode.rect = eventNode.GetPosition();

                    tempNode.dialogueEvents = eventNode.allDialogueEventTriggers;

                    dialogueContainer.eventNodesData.Add(tempNode);
                }
            }
            // ----- Save Comments Nodes
            {
                foreach (CommentsNode commentsNode in commentsNodes)
                {
                    var tempNode = new CommentsNodeData();

                    tempNode.guid = commentsNode.GUID;
                    tempNode.rect = commentsNode.GetPosition();

                    tempNode.text = commentsNode.text;

                    dialogueContainer.commentsNodesData.Add(tempNode);
                }
            }
            // ----- Save End Nodes
            {
                foreach (EndNode endNode in endNodes)
                {
                    var tempNode = new EndNodeData();

                    tempNode.guid = endNode.GUID;
                    tempNode.rect = endNode.GetPosition();

                    dialogueContainer.endNodesData.Add(tempNode);
                }
            }
        }
        /// ----- SAVE EDGES
        {
            List<Edge> edges = targetGraphView.edges.ToList();
            foreach (var edge in edges)
            {
                // visualgraph has a bug where you end up with unconnected edge, so it prevents it from saving
                if (edge.output.node == null || edge.input.node == null)
                    continue;

                //
                var outputNode = edge.output.node as CustomNode;
                var inputNode = edge.input.node as CustomNode;

                dialogueContainer.edgesData.Add(new EdgesData
                {
                    outputPortName = edge.output.portName,
                    outputNodeGuid = outputNode.GUID,

                    targetNodeGuid = inputNode.GUID
                });
            }
        }

        /// ----- SAVE SCRIPTABLE OBJECT OF DIALOGUE CONTAINER TO A SPECIAL FOLDER
        {
            // ----- create resources folder if it doesnt exist
            Directory.CreateDirectory(saveFilePath);
            /*
            if (!AssetDatabase.IsValidFolder(path: "Assets/Resources"))
            {
                AssetDatabase.CreateFolder(parentFolder: "Assets", newFolderName: "Resources");                
            }
            if (!AssetDatabase.IsValidFolder(path: "Assets/Resources/SavedDialogues"))
            {
                AssetDatabase.CreateFolder(parentFolder: "Assets/Resources", newFolderName: "SavedDialogues");
            }
            */
            // ----- SAVE it OR OVERRIDE it if file already exists!!! important overwise serializefields of dialoguetriggers get reset       
            DialogueContainer existingContainer = Resources.Load<DialogueContainer>("SavedDialogues/" + _filename);
            if (existingContainer != null)
            {
                EditorUtility.SetDirty(existingContainer); // there's data loss on reload without this shit

                existingContainer.edgesData = dialogueContainer.edgesData;

                existingContainer.entryNodeData = dialogueContainer.entryNodeData;
                existingContainer.dialogueNodesData = dialogueContainer.dialogueNodesData;
                existingContainer.redirectionsNodesData = dialogueContainer.redirectionsNodesData;
                existingContainer.eventNodesData = dialogueContainer.eventNodesData;
                existingContainer.commentsNodesData = dialogueContainer.commentsNodesData;
                existingContainer.endNodesData = dialogueContainer.endNodesData;
            }
            else
            {
                //Debug.Log("asset doesnt exist");
                //EditorUtility.SetDirty(dialogueContainer); // there's data loss on reload without this shit
                AssetDatabase.CreateAsset(dialogueContainer, path: saveFilePath + $"{_filename}.asset");
            }

            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        /// ----- SAVE DIALOGUE TEXT TO TEXT FILE FOR LOCALIZATION SUPPORT
        {
            string dialogueContents = "";
            string text;

            // START
            {
                text = "[FILENAME=" + _filename + "]";
                dialogueContents += text + '\n';
            }
            foreach (DialogueNode node in dialogueNodes)
            {
                // START PAGE
                {
                    text = "[PAGE_START]";
                    dialogueContents += text + '\n';
                }
                // Node GUID
                {
                    text = "[GUID=" + node.GUID + "]";
                    dialogueContents += text + '\n';
                }
                // Speaker Name
                {
                    text = "[NAME=" + node.speakerName + "]";
                    dialogueContents += text + '\n';
                }
                // Text
                {
                    text = "[TEXT=" + node.dialogueText + "]";
                    dialogueContents += text + '\n';
                }
                // Choices
                int i = 0;
                foreach (var choice in node.choices)
                {
                    ++i;
                    text = "[CHOICE" + i + "=" + choice.choiceName + "]";
                    dialogueContents += text + '\n';
                }
                // END PAGE
                {
                    text = "[PAGE_END]";
                    dialogueContents += text + '\n';
                }
            }
            text = "[END]";
            dialogueContents += text + '\n';


            //string path = $"Assets/Resources/SavedDialogues/{_filename}.txt";
            string path = localizationDefaultPath;
            Directory.CreateDirectory(path);
            path += $"{_filename}.txt";

            
            FileStream stream = File.Create(path);
            stream.Close();

            File.WriteAllText(path, dialogueContents);
            
            //TextAsset asset = new TextAsset(dialogueContents);
            //AssetDatabase.CreateAsset(asset, path: localizationDefaultPath + $"{_filename}.txt");
        }
    }
    public void LoadGraph(string _filename)
    {
        dialogueContainer = Resources.Load<DialogueContainer>("SavedDialogues/" + _filename);
        if (dialogueContainer == null)
        {
            EditorUtility.DisplayDialog(title: "Error", message: "File not found", ok: "OK");
            return;
        }

        ClearGraph();
        GenerateLoadedNodes();
        ConnectLoadedNodes();
    }

    private void ClearGraph()
    {
        foreach(Node node in targetGraphView.nodes)
        {
            targetGraphView.RemoveElement(node);
        }
        foreach(Edge edge in targetGraphView.edges)
        {
            targetGraphView.RemoveElement(edge);
        }
    }
    private void GenerateLoadedNodes()
    {
        /// CREATE NODES
        {
            // ----- Entry Node
            {
                EntryNode tempNode = targetGraphView.CreateEntryNode(dialogueContainer.entryNodeData.guid);

                tempNode.SetPosition(dialogueContainer.entryNodeData.rect);

                targetGraphView.AddElement(tempNode);
            }
            // ----- Dialogue Nodes
            foreach (DialogueNodeData nodeData in dialogueContainer.dialogueNodesData)
            {
                DialogueNode tempNode = targetGraphView.CreateDialogueNode(Vector2.zero, nodeData.guid, 
                    nodeData.speakerName, nodeData.speakerPortrait, nodeData.dialogueText, 
                    nodeData.choices);

                tempNode.title = nodeData.dialogueTitle;
                tempNode.SetPosition(nodeData.rect);

                targetGraphView.AddElement(tempNode);
            }
            // ----- Conditions Nodes
            foreach (RedirectionsNodeData nodeData in dialogueContainer.redirectionsNodesData)
            {
                RedirectionsNode tempNode = targetGraphView.CreateRedirectionsNode(Vector2.zero, nodeData.guid, nodeData.redirections);

                tempNode.SetPosition(nodeData.rect);

                targetGraphView.AddElement(tempNode);
            }
            // ----- Event Nodes
            {
                foreach (EventTriggerNodeData nodeData in dialogueContainer.eventNodesData)
                {
                    EventTriggerNode tempNode = targetGraphView.CreateEventNode(Vector2.zero, nodeData.guid, nodeData.dialogueEvents);

                    tempNode.SetPosition(nodeData.rect);

                    targetGraphView.AddElement(tempNode);
                }
            }
            // ----- Comments Node
            {
                foreach (CommentsNodeData nodeData in dialogueContainer.commentsNodesData)
                {
                    CommentsNode tempNode = targetGraphView.CreateCommentsNode(Vector2.zero, nodeData.guid, nodeData.text);

                    tempNode.SetPosition(nodeData.rect);

                    targetGraphView.AddElement(tempNode);
                }
             }
            // ----- End Nodes
            {
                foreach (EndNodeData nodeData in dialogueContainer.endNodesData)
                {
                    EndNode tempNode = targetGraphView.CreateEndNode(Vector2.zero, nodeData.guid);

                    tempNode.SetPosition(nodeData.rect);

                    targetGraphView.AddElement(tempNode);
                }
            }
        }      
    }
    private void ConnectLoadedNodes()
    {       
        /// we have generated nodes and ports and now we need to connect them all
        ///
        var loadedEdges = dialogueContainer.edgesData;
        foreach (var loadedEdge in loadedEdges)
        {
            var tempEdge = new Edge();
     
            // FIND OUTPUT PORT OF AN EDGE
            foreach (var port in targetGraphView.ports)
            {
                CustomNode portNode = (CustomNode) port.node;
                if (port.portName == loadedEdge.outputPortName && portNode.GUID == loadedEdge.outputNodeGuid)
                {
                    tempEdge.output = port;
                }
            }

            // FIND INPUT PORT
            var graphNodes = targetGraphView.nodes.Cast<CustomNode>();
            foreach (var node in graphNodes)
            {
                if (node.GUID == loadedEdge.targetNodeGuid)
                {
                    tempEdge.input = (Port)node.inputContainer[0]; // we can do it because in dialogue system there's only one input port for each node and it is [0]
                }
            }

            tempEdge.input.Connect(tempEdge);
            tempEdge.output.Connect(tempEdge);
            targetGraphView.Add(tempEdge);
        }
        
    }

    
}
