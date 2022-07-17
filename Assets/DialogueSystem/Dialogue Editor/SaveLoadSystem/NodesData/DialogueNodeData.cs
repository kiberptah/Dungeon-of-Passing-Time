using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;




namespace DialogueSystem
{
    [Serializable]
    public class DialogueNodeData
    {
        public bool isEntryNode;
        public string guid;
        public string dialogueTitle;
        public string dialogueText;
        public string speakerName;
        public Sprite speakerPortrait;
        public Rect rect;

        public List<DialogueChoice> choices = new List<DialogueChoice>();


    }
}