using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

using System;
using System.Linq;

namespace DialogueSystem
{
    public class DialogueNode : CustomNode
    {
        //public string GUID;

        public string dialogueText;
        public string speakerName;
        public Sprite speakerPortrait;

        public List<DialogueChoice> choices = new List<DialogueChoice>();

    }
}