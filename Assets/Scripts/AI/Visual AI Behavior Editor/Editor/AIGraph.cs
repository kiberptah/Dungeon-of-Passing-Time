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
        TextField filenameField;
        ObjectField behaviorField;

        [MenuItem("Graph/AI Graph")]
        public static void OpenWindow()
        {
            var window = GetWindow<AIGraph>();
            window.titleContent = new GUIContent("AI Graph");
        }

        void OnEnable()
        {
            //Debug.Log("AIGraph Window Opened");

            ConstructGraphView();
            GenerateToolbar();
        }
        void OnDisable()
        {
            //Debug.Log("AIGraph Window Closed");
            RemoveGraphView();
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
        void RemoveGraphView()
        {
            rootVisualElement.Remove(graphView);
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
                Button resetButton = new Button(clickEvent: () =>
                {
                    Close();
                    OpenWindow();
                })
                {
                    text = "Reset GraphView"
                };
                toolbar.Add(resetButton);
            }
            {
                Label label = new Label();
                label.text = "Behavior Name:";
                label.AddToClassList("behaviorNameLabel");
                label.style.alignSelf = Align.FlexEnd;
                //toolbar2.contentContainer.Add(label);
                toolbar.Add(label);
            }
            {
                filenameField = new TextField();
                filenameField.RegisterValueChangedCallback(evt => filename = evt.newValue);
                filenameField.AddToClassList("behaviorNameField");
                toolbar.Add(filenameField);

            }
            

            {
                Box saveBox = new Box();
                saveBox.AddToClassList("toolbarBox");
                toolbar.Add(saveBox);

                {
                    Button saveButton = new Button(clickEvent: () =>
                    {
                        SaveGraph(asNew: false);
                    })
                    {
                        text = "Save"
                    };

                    saveBox.Add(saveButton);
                }
                {
                    behaviorField = new ObjectField()
                    {
                        objectType = typeof(AIGraph_SaveData)
                    };
                    behaviorField.RegisterValueChangedCallback(x =>
                    {
                        loadedBehavior = (AIGraph_SaveData)behaviorField.value;

                        if (loadedBehavior != null)
                        {
                            filename = loadedBehavior.name;

                            AIGraph_SaveUtility.LoadGraph(graphView, loadedBehavior);
                            filenameField.value = filename;
                        }
                    });

                    saveBox.Add(behaviorField);
                }              
                {
                    Button saveButton = new Button(clickEvent: () => 
                    {
                        SaveGraph(asNew: true);
                    })
                    {
                        text = "Save as New"
                    };
                    saveBox.Add(saveButton);
                }
            }
            //toolbar1.Add(new Button(() => RequestDataOperation(true)) { text = "Save Data" });

            //toolbar1.Add(new Button(() => RequestDataOperation(false)) { text = "Load Data" });
            void SaveGraph(bool asNew)
            {
                loadedBehavior = AIGraph_SaveUtility.SaveGraph(graphView, behaviorField, loadedBehavior, filename, asNew);
                behaviorField.value = loadedBehavior;
                filenameField.value = loadedBehavior.filename;
            }
        }

    }
}


