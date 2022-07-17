using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor.Experimental.GraphView;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

/// okay listen, I copypasted most of it from the tutorial exept all the things that were retarded
/// also this shit is in experimental state and there's no comprehensible API or guides by Unity
/// so it's a miracle that a shitty coder like me could even dig into this
/// feel free to modify this for your needs

namespace DialogueSystem
{
    public class DialogueGraph : EditorWindow
    {
        DialogueGraphView graphView;
        string filename = "New Narrative";

        [MenuItem("Graph/Dialogue Graph")]
        public static void OpenDialogueGrapthWindow()
        {
            //Debug.Log("window");

            var window = GetWindow<DialogueGraph>();
            window.titleContent = new GUIContent("Dialogue Graph");
        }

        private void OnEnable()
        {
            ConstructGraphView();
            GenerateToolbar();
            //Debug.Log("enable");
            //GenerateMinimap();
        }

        private void OnDisable()
        {
            rootVisualElement.Remove(graphView);
            //Debug.Log("disable");
        }

        void ConstructGraphView()
        {
            graphView = new DialogueGraphView(this)
            {
                name = "Dialogue Graph"
            };
            graphView.StretchToParentSize();
            rootVisualElement.Add(graphView);
        }

        void GenerateToolbar()
        {
            //create toolbar as a variable
            var toolbar = new Toolbar();

            // text field to name save/load file
            var title = new TextElement();
            title.text = "  File Name:  ";
            title.style.alignSelf = Align.Center;
            toolbar.Add(title);


            //var fileNameTextField = new TextField(label: "File Name:");
            var fileNameTextField = new TextField();
            fileNameTextField.SetValueWithoutNotify(filename);
            fileNameTextField.MarkDirtyRepaint();
            fileNameTextField.RegisterValueChangedCallback(evt => filename = evt.newValue);
            toolbar.Add(fileNameTextField);
            // save/load buttons     
            toolbar.Add(child: new Button(clickEvent: () => SaveData()) { text = "Save Data" });
            toolbar.Add(child: new Button(clickEvent: () => LoadData()) { text = "Load Data" });

            // add toolbar to the graph
            rootVisualElement.Add(toolbar);


            /*
            var toolbar2 = new Toolbar();
            // dialogue node creation button
            {
                var nodeCreateButton = new Button(clickEvent: () => { graphView.AddElement(graphView.CreateDialogueNode()); });
                nodeCreateButton.text = "Add Dialogue Node";
                toolbar2.Add(nodeCreateButton);
            }
            // conditions node creation button
            {
                var nodeCreateButton = new Button(clickEvent: () => { graphView.AddElement(graphView.CreateRedirectionsNode()); });
                nodeCreateButton.text = "Add Redirections Node";
                toolbar2.Add(nodeCreateButton);
            }
            // event node creation button
            {
                var nodeCreateButton = new Button(clickEvent: () => { graphView.AddElement(graphView.CreateEventNode()); });
                nodeCreateButton.text = "Add Event Node";
                toolbar2.Add(nodeCreateButton);
            }
            // end node creation button
            {
                var nodeCreateButton = new Button(clickEvent: () => { graphView.AddElement(graphView.CreateEndNode()); });
                nodeCreateButton.text = "Add End Node";
                toolbar2.Add(nodeCreateButton);
            }
            // add toolbar to the graph
            rootVisualElement.Add(toolbar2);
            */
        }

        void GenerateMinimap()
        {
            MiniMap miniMap = new MiniMap();
            miniMap.anchored = true;
            miniMap.SetPosition(new Rect(x: 10, y: 30, width: 200, height: 140));

            graphView.Add(miniMap);
        }

        void SaveData()
        {
            if (string.IsNullOrEmpty(filename))
            {
                EditorUtility.DisplayDialog(title: "Error", message: "Invalid file name!", ok: "OK");
                return;
            }
            var saveUtility = GraphSaveUtility.GetInstance(graphView);

            saveUtility.SaveGraph(filename);
            CustomEditorUtilities.ReloadScene(); // to avoid some bugs
        }
        void LoadData()
        {
            if (string.IsNullOrEmpty(filename))
            {
                EditorUtility.DisplayDialog(title: "Error", message: "Invalid file name!", ok: "OK");
                return;
            }
            var saveUtility = GraphSaveUtility.GetInstance(graphView);

            saveUtility.LoadGraph(filename);
        }

        /*
        void RequestDataOperation(bool save) //true to save false to load
        {
            if (string.IsNullOrEmpty(filename))
            {
                EditorUtility.DisplayDialog(title: "Error", message: "Invalid file name!", ok: "OK");
                return;
            }
            var saveUtility = GraphSaveUtility.GetInstance(graphView);
            if (save)
            {
                saveUtility.SaveGraph(filename);
            }
            else
            {
                saveUtility.LoadGraph(filename);
            }
        }
        */
    }
}
