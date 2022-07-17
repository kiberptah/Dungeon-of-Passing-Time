using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


namespace AI_BehaviorEditor
{
    public class AIGraph : EditorWindow
    {
        AIGraphView graphView;
        string filename = "";
        AIGraph_SaveData loadedBehavior = null;

        [MenuItem("Graph/AI Graph")]
        public static void OpenWindow()
        {
            var window = GetWindow<AIGraph>();
            window.titleContent = new GUIContent("AI Graph");
        }

        void OnEnable()
        {
            Debug.Log("AIGraph Window Opened");

            ConstructGraphView();
            GenerateToolbar();
        }
        void OnDisable()
        {
            Debug.Log("AIGraph Window Closed");

            rootVisualElement.Remove(graphView);
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



        private void GenerateToolbar()
        {

            var toolbar = new Toolbar();
            rootVisualElement.Add(toolbar);

            toolbar.styleSheets.Add(styleSheet: Resources.Load<StyleSheet>(path: "StyleSheets/AIGraph"));
            /*
            var fileNameTextField = new TextField("File Name:");
            fileNameTextField.SetValueWithoutNotify(filename);
            fileNameTextField.MarkDirtyRepaint();
            fileNameTextField.RegisterValueChangedCallback(evt => filename = evt.newValue);
            toolbar1.Add(fileNameTextField);
            */

            {
                Label label = new Label();
                label.text = "Behavior Name:";
                label.AddToClassList("behaviorNameLabel");
                label.style.alignSelf = Align.FlexEnd;
                //toolbar2.contentContainer.Add(label);
                toolbar.Add(label);
            }
            {
                TextField nameField = new TextField();
                nameField.RegisterValueChangedCallback(evt => filename = evt.newValue);
                nameField.AddToClassList("behaviorNameField");
                toolbar.Add(nameField);

            }
            

            {
                Box saveBox = new Box();
                saveBox.AddToClassList("toolbarBox");
                toolbar.Add(saveBox);

                {
                    Button saveButton = new Button(clickEvent: () => AIGraph_SaveUtility.SaveGraph(graphView, loadedBehavior, filename))
                    {
                        text = "Save"
                    };

                    saveBox.Add(saveButton);
                }
                {
                    ObjectField field = new ObjectField()
                    {
                        objectType = typeof(AIGraph_SaveData)
                    };
                    field.RegisterValueChangedCallback(x =>
                    {
                        loadedBehavior = (AIGraph_SaveData)field.value;

                        if (loadedBehavior != null)
                        {
                            filename = loadedBehavior.name;

                            AIGraph_SaveUtility.LoadGraph(graphView, loadedBehavior);
                        }
                    });
                    saveBox.Add(field);
                }              
                {
                    Button saveButton = new Button()
                    {
                        text = "Save as New"
                    };
                    saveBox.Add(saveButton);
                }
            }
            //toolbar1.Add(new Button(() => RequestDataOperation(true)) { text = "Save Data" });

            //toolbar1.Add(new Button(() => RequestDataOperation(false)) { text = "Load Data" });

        }

    }
}


