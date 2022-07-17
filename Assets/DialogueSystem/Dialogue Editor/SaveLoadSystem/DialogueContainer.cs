using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;




namespace DialogueSystem
{
    [Serializable]
    public class DialogueContainer : ScriptableObject
    {
        public string filename;



        public List<EdgesData> edgesData = new List<EdgesData>();


        public EntryNodeData entryNodeData = new EntryNodeData();
        public List<DialogueNodeData> dialogueNodesData = new List<DialogueNodeData>();
        public List<RedirectionsNodeData> redirectionsNodesData = new List<RedirectionsNodeData>();
        public List<EventTriggerNodeData> eventNodesData = new List<EventTriggerNodeData>();
        public List<CommentsNodeData> commentsNodesData = new List<CommentsNodeData>();
        public List<EndNodeData> endNodesData = new List<EndNodeData>();
    }
}