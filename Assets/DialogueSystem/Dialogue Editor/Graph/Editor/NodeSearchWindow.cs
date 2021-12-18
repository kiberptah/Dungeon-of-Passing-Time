using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

using System;
using System.Linq;


public class NodeSearchWindow : ScriptableObject, ISearchWindowProvider
{
    DialogueGraphView graphView;
    EditorWindow editorWindow;

    public void Init(EditorWindow _editorWindow, DialogueGraphView _graphView)
    {
        editorWindow = _editorWindow;
        graphView = _graphView;
    }
    
    public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
    {
        var tree = new List<SearchTreeEntry>
        {
            new SearchTreeGroupEntry(new GUIContent(text: "Create Elements"), level: 0),
            new SearchTreeEntry(new GUIContent(text: "  Dialogue Node"))
            {
                userData = new DialogueNode(), level = 1
            },

            new SearchTreeEntry(new GUIContent(text: "  Redirections Node"))
            {
                userData = new RedirectionsNode(), level = 1
            },

            new SearchTreeEntry(new GUIContent(text: "  Event Node"))
            {
                userData = new EventTriggerNode(), level = 1
            },

            new SearchTreeEntry(new GUIContent(text: "  Comments Node"))
            {
                userData = new CommentsNode(), level = 1
            },

            new SearchTreeEntry(new GUIContent(text: "  End Node"))
            {
                userData = new EndNode(), level = 1
            }
        };
        return tree;
    }
    
    public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
    {
        Vector2 worldMousePosition = editorWindow.rootVisualElement.ChangeCoordinatesTo(editorWindow.rootVisualElement.parent,
            context.screenMousePosition - editorWindow.position.position);
        Vector2 localMousePosition = graphView.contentViewContainer.WorldToLocal(worldMousePosition);


        switch(searchTreeEntry.userData)
        {
            default:
                return false;
            case DialogueNode dialogueNode:
                graphView.AddElement(graphView.CreateDialogueNode(localMousePosition));
                SearchWindow.focusedWindow.Close();
                break;
            case RedirectionsNode redirectionsNode:
                graphView.AddElement(graphView.CreateRedirectionsNode(localMousePosition));
                SearchWindow.focusedWindow.Close();
                break;
            case EventTriggerNode eventTriggerNode:
                graphView.AddElement(graphView.CreateEventNode(localMousePosition));
                SearchWindow.focusedWindow.Close();
                break;
            case CommentsNode endNode:
                graphView.AddElement(graphView.CreateCommentsNode(localMousePosition));
                SearchWindow.focusedWindow.Close();
                break;
            case EndNode endNode:
                graphView.AddElement(graphView.CreateEndNode(localMousePosition));
                SearchWindow.focusedWindow.Close();
                break;
        }


        return false;
    }

}