using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using System;
public class AIGraphView : GraphView
{
    public AIGraphView(AIGraph _editorWindow)
    {
        //styleSheets.Add(styleSheet: Resources.Load<StyleSheet>(path: "StyleSheets/AIGraph")); // uss file for visual customization of elements

        // basic stuff from API to intercat with elements
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale); // without it there's no zoom
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        // Just a visual grid so it looks nice
        var grid = new GridBackground();
        Insert(index: 0, grid);
        grid.StretchToParentSize();


        AddElement(CreateEntryNode());
        Node newNode = CreateEntryNode();

        Node test = CreateEntryNode();
        newNode.Add(test);


        AddElement(newNode);
        Edge edge = new Edge();
        edge.style.color = Color.red;

    }




    public Node CreateEntryNode(string _overrideGUID = "")
    {
        /*
        // GUID
        string newGUID = Guid.NewGuid().ToString();
        if (!string.IsNullOrEmpty(_overrideGUID))
        {
            newGUID = _overrideGUID;
        }
        */
        // Generate Node
        var newNode = new Node
        {
            title = "ENTRY NODE"

        };
        

        return newNode;

    }

}
