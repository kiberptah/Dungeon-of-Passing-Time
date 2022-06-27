using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class AIGraph : EditorWindow
{
    AIGraphView graphView;

    [MenuItem("Graph/AI Graph")]
    public static void OpenWindow()
    {
        var window = GetWindow<AIGraph>();
        window.titleContent = new GUIContent("AI Graph");
    }

    private void OnEnable()
    {
        ConstructGraphView();
        GenerateToolbar();
    }

    void ConstructGraphView()
    {
        graphView = new AIGraphView(this)
        {
            name = "AI Graph"
        };
        graphView.StretchToParentSize();
        rootVisualElement.Add(graphView);
    }

    void GenerateToolbar()
    {

    }
}
