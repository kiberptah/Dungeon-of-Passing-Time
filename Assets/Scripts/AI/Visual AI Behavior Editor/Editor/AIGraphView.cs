using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using System;

namespace AI_BehaviorEditor
{
    public class AIGraphView : GraphView
    {
        class Port_States { };
        class Port_Actions { };
        static EntryNode entryNode;

        public AIGraphView(AIGraph _editorWindow)
        {
            // --- Set up StyleSheets
            styleSheets.Add(styleSheet: Resources.Load<StyleSheet>(path: "StyleSheets/AIGraph")); 
            styleSheets.Add(styleSheet: Resources.Load<StyleSheet>(path: "StyleSheets/Nodes")); 

            // --- Basic stuff from API to intercat with elements
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale); // without it there's no zoom
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(ContextMenu_NewState());
            // --- Just a visual grid so it looks nice
            var grid = new GridBackground();
            Insert(index: 0, grid);
            grid.StretchToParentSize();

            entryNode = Create_EntryNode();
            AddElement(entryNode);          
        }

        string ReturnGUID(CustomNode node)
        {
            string GUID = Guid.NewGuid().ToString();
            node.nodeData.nodeGUID = GUID;

            return GUID;
        }
        public CustomNode FindCustomNodeByGUID(string GUID)
        {
            foreach (CustomNode node in nodes)
            {
                if (node.nodeData.nodeGUID == GUID)
                {
                    return node;
                }
            }
            return null;
        }

        IManipulator ContextMenu_NewState()
        {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction("Add State Node", actionEvent => Create(actionEvent))
                );


            void Create(DropdownMenuAction actionEvent)
            {
                StateNode newNode = Create_StateNode();
                newNode.SetPosition(new Rect (viewTransform.matrix.inverse.MultiplyPoint(actionEvent.eventInfo.localMousePosition), Vector3.zero)); //no idea how it works
                AddElement(newNode);
            }
            return contextualMenuManipulator;
        }


        public void ClearGraph()
        {
            foreach (Node node in nodes)
            {
                if (((CustomNode)node).nodeData.nodeGUID != "0") // do not delete Entry Node
                {
                    RemoveElement(node);
                }
            }
            foreach (Edge edge in edges)
            {
                RemoveElement(edge);
            }
        }


        Port GeneratePort(Node _node, Direction _portDirection, Type type, Port.Capacity _capacity = Port.Capacity.Single, Orientation orientation = Orientation.Horizontal)
        {
            Port port = _node.InstantiatePort(orientation, _portDirection, _capacity, type); // change to custom type and force assigning on creation

            // style for label, probably to hide it
            Label portLabel = port.contentContainer.Q<Label>(name: "type");
            portLabel.AddToClassList("port-label");
            //port.contentContainer.AddToClassList("port-container");
            
            return port;
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            /// I think it overrides some default Unity function which is called when you start dragging from an output port
            /// Without this functionn you can't connect ports
            /// also it has logic to prevent illogical node connections

            var compatablePorts = new List<Port>();
            ports.ForEach(port =>
            {
                if (startPort != port && startPort.node != port.node && startPort.direction != port.direction && startPort.portType == port.portType)
                {
                    
                    /*
                    if (!(startPort.node.GetType() == new EntryNode().GetType() && port.node.GetType() == new ConditionsNode().GetType()))
                    {

                    }
                    */
                    compatablePorts.Add(port);
                }
            });

            return compatablePorts;
        }

        public EntryNode Create_EntryNode()
        {
            // --- Generate Node
            var thisNode = new EntryNode();
            thisNode.nodeData.nodeGUID = "0";

            thisNode.title = "ENTRY";

            thisNode.capabilities &= ~Capabilities.Deletable;
            thisNode.capabilities &= ~Capabilities.Movable;
            thisNode.capabilities &= ~Capabilities.Copiable;

            // --- Generate Output Port
            {
                Port outputPort = GeneratePort(thisNode, Direction.Output, typeof(Port_States), Port.Capacity.Single);
                outputPort.portName = "Output";
                outputPort.portColor = Color.red;


                thisNode.outputContainer.Add(outputPort);
            }


            thisNode.RefreshExpandedState();
            thisNode.RefreshPorts();


            return thisNode;
        }

        public StateNode Create_StateNode(AI_StateData loadData = null, CustomNode_Data loadNodeData = null)
        {
            // Generate Node
            var thisNode = new StateNode(this);
            thisNode.title = "STATE";
            thisNode.mainContainer.contentContainer.AddToClassList("contentContainer");

            // --- GUID
            thisNode.data.GUID = ReturnGUID(thisNode);

            // --- State Name Field
            {
                TextField stateNameField = new TextField();
                stateNameField.RegisterValueChangedCallback(x => { thisNode.data.stateName = stateNameField.value; });
                stateNameField.AddToClassList("stateNodeNameField");

                thisNode.mainContainer.contentContainer.Add(stateNameField);
                if (loadData != null)
                {
                    stateNameField.value = loadData.stateName;
                }
            }

            // --- Generate Input Port
            {
                var inputPort = GeneratePort(thisNode, Direction.Input, typeof(Port_States), Port.Capacity.Multi);
                inputPort.portName = "Input";
                inputPort.portColor = Color.red;
                thisNode.inputContainer.Add(inputPort);
            }

            // --- Generate Output Ports
            {
                Port outputPort;

                // Port for onEnter actions
                outputPort = GeneratePort(thisNode, Direction.Output, typeof(Port_Actions), Port.Capacity.Single);
                outputPort.portName = "onEnter";
                outputPort.portColor = new Color(1, 0.6f, 0);
                thisNode.outputContainer.Add(outputPort);


                // Port for actions loop
                outputPort = GeneratePort(thisNode, Direction.Output, typeof(Port_Actions), Port.Capacity.Single);
                outputPort.portName = "Loop";
                outputPort.portColor = Color.yellow;
                thisNode.outputContainer.Add(outputPort);

                // Port for decisions loop
                outputPort = GeneratePort(thisNode, Direction.Output, typeof(Port_Actions), Port.Capacity.Single);
                outputPort.portName = "Decisions";
                outputPort.portColor = Color.magenta;
                thisNode.outputContainer.Add(outputPort);

                // Port for onExit actions
                outputPort = GeneratePort(thisNode, Direction.Output, typeof(Port_Actions), Port.Capacity.Single);
                outputPort.portName = "onExit";
                outputPort.portColor = new Color(1, 0.6f, 0);
                thisNode.outputContainer.Add(outputPort);
            }


            // --- BUTTONS TO ADD NODES
            {
                // --- Button to add actions
                {
                    Button button = new Button(clickEvent: () => AddActionToState())
                    {
                        text = "Add Action"
                    };
                    button.AddToClassList("newActionButt");

                    thisNode.mainContainer.Add(button);

                    void AddActionToState()
                    {
                        ActionNode newNode = Create_ActionNode();
                        AddElement(newNode);
                        newNode.SetPosition(thisNode.GetPosition());
                        thisNode.AddChild(newNode);

                    }
                }

                // --- Button to add decisions
                {
                    Button button = new Button(clickEvent: () => AddActionToState())
                    {
                        text = "Add Decision"
                    };
                    button.AddToClassList("newDecisionButt");

                    thisNode.mainContainer.Add(button);

                    void AddActionToState()
                    {
                        DecisionNode newNode = Create_DecisionNode();
                        AddElement(newNode);
                        newNode.SetPosition(thisNode.GetPosition());
                        thisNode.AddChild(newNode);
                    }
                }
                // --- Button to add timers
                {
                    Button button = new Button(clickEvent: () => AddActionToState())
                    {
                        text = "Add Timer"
                    };
                    button.AddToClassList("newTimerButt");

                    thisNode.mainContainer.Add(button);

                    void AddActionToState()
                    {
                        TimerNode newNode = Create_TimerNode();
                        AddElement(newNode);
                        newNode.SetPosition(thisNode.GetPosition());
                        thisNode.AddChild(newNode);
                    }
                }
                // --- Button to add values
                {
                    Button button = new Button(clickEvent: () => AddActionToState())
                    {
                        text = "Add Value"
                    };
                    button.AddToClassList("newValueButt");

                    thisNode.mainContainer.Add(button);

                    void AddActionToState()
                    {
                        ValueNode newNode = Create_ValueNode();
                        AddElement(newNode);
                        newNode.SetPosition(thisNode.GetPosition());
                        thisNode.AddChild(newNode);
                    }
                }
                // --- Button to add valueschangers
                {
                    Button button = new Button(clickEvent: () => AddActionToState())
                    {
                        text = "Add Value Changer"
                    };
                    button.AddToClassList("newValueChangerButt");

                    thisNode.mainContainer.Add(button);

                    void AddActionToState()
                    {
                        ValueChangerNode newNode = Create_ValueChangerNode();
                        AddElement(newNode);
                        newNode.SetPosition(thisNode.GetPosition());
                        thisNode.AddChild(newNode);
                    }
                }
                
                // --- Button to add state change
                {
                    Button button = new Button(clickEvent: () => AddActionToState())
                    {
                        text = "Add State Transition"
                    };
                    button.AddToClassList("newStateTransitionButt");

                    thisNode.mainContainer.Add(button);

                    void AddActionToState()
                    {
                        TransitionNode newNode = Create_TransitionNode();
                        AddElement(newNode);
                        newNode.SetPosition(thisNode.GetPosition());
                        thisNode.AddChild(newNode);
                    }
                }

            }


            

            

            thisNode.RefreshExpandedState();
            thisNode.RefreshPorts();

            if (loadData != null)
            {
                thisNode.data = loadData;
                thisNode.nodeData.nodeGUID = loadData.GUID;
            }
            if (loadNodeData != null)
            {
                thisNode.SetPosition(loadNodeData.nodeRect);
            }
            return thisNode;
        }







        public TransitionNode Create_TransitionNode(AI_StateTransitionData loadData = null, CustomNode_Data loadNodeData = null)
        {
            // --- Generate Node
            var thisNode = new TransitionNode();
            thisNode.title = "TRANSITION";
            // --- GUID
            thisNode.data.GUID = ReturnGUID(thisNode);

            // --- Generate Input Port
            {
                Port inputPort = GeneratePort(thisNode, Direction.Input, typeof(Port_Actions), Port.Capacity.Multi);
                inputPort.portName = "Decision";
                inputPort.portColor = Color.yellow;

                thisNode.inputContainer.Add(inputPort);

            }
            // --- Generate Output Port
            {
                Port outputPort = GeneratePort(thisNode, Direction.Output, typeof(Port_States), Port.Capacity.Single);
                outputPort.portName = "Next State";
                outputPort.portColor = Color.red;

                thisNode.outputContainer.Add(outputPort);
            }


            if (loadData != null)
            {
                thisNode.data = loadData;
                thisNode.nodeData.nodeGUID = loadData.GUID;
            }
            if (loadNodeData != null)
            {
                thisNode.SetPosition(loadNodeData.nodeRect);
            }
            return thisNode;
        }

        
        public ActionNode Create_ActionNode(AI_ActionData loadData = null, CustomNode_Data loadNodeData = null)
        {
            // --- Generate Node
            var thisNode = new ActionNode();
            thisNode.title = "ACTION";
            thisNode.mainContainer.contentContainer.AddToClassList("contentContainer");
            // --- GUID
            thisNode.data.GUID = ReturnGUID(thisNode);

            // --- Generate Input Port
            {
                Port inputPort = GeneratePort(thisNode, Direction.Input, typeof(Port_Actions), Port.Capacity.Multi);
                inputPort.portName = "Prev";
                inputPort.portColor = Color.yellow;

                thisNode.inputContainer.Add(inputPort);
            }
            // --- Generate Output Port
            {
                Port outputPort = GeneratePort(thisNode, Direction.Output, typeof(Port_Actions), Port.Capacity.Single);
                outputPort.portName = "Next";
                outputPort.portColor = Color.yellow;


                thisNode.outputContainer.Add(outputPort);
            }

            Box dynamicValuesContainer = new Box();
            thisNode.mainContainer.Add(dynamicValuesContainer);

            // --- Action Field
            {
                ObjectField actionField = new ObjectField()
                {
                    objectType = typeof(AI_Action)
                };
                actionField.RegisterValueChangedCallback(x =>
                {
                    thisNode.data.action = (AI_Action)actionField.value;
                    actionField.tooltip = actionField.value.name;
                    AddDynamicValues();

                });
                if (loadData != null)
                {
                    if (loadData.action != null)
                    {
                        thisNode.data.action = loadData.action;

                        actionField.value = loadData.action;
                        actionField.tooltip = actionField.value.name;

                        AddDynamicValues();
                    }
                }


                thisNode.mainContainer.Add(actionField);


                void AddDynamicValues()
                {
                    dynamicValuesContainer.Clear();
                    //don't forget to assign values to data on save!!!
                    thisNode.data.dynamicValues = thisNode.data.action.dynamicValues;

                    foreach (var value in thisNode.data.dynamicValues.boolValues)
                    {
                        // --- bool Port
                        {
                            Port inputPort = GeneratePort(thisNode, Direction.Input, typeof(bool), Port.Capacity.Multi);
                            inputPort.portName = "(bool) " + value.Key;
                            inputPort.portColor = Color.white;

                            dynamicValuesContainer.Add(inputPort);
                        }
                    }
                    foreach (var value in thisNode.data.dynamicValues.floatValues)
                    {
                        // --- float Port
                        {
                            Port inputPort = GeneratePort(thisNode, Direction.Input, typeof(float), Port.Capacity.Multi);
                            inputPort.portName = "(float) " + value.Key;
                            inputPort.portColor = Color.white;

                            dynamicValuesContainer.Add(inputPort);
                        }
                    }
                    foreach (var value in thisNode.data.dynamicValues.intValues)
                    {
                        // --- int Port
                        {
                            Port inputPort = GeneratePort(thisNode, Direction.Input, typeof(int), Port.Capacity.Multi);
                            inputPort.portName = "(int) " + value.Key;
                            inputPort.portColor = Color.white;

                            dynamicValuesContainer.Add(inputPort);
                        }
                    }
                }
            }
            

            thisNode.RefreshExpandedState();
            thisNode.RefreshPorts();
            if (loadData != null)
            {
                thisNode.data = loadData;
                thisNode.nodeData.nodeGUID = loadData.GUID;
            }
            if (loadNodeData != null)
            {
                thisNode.SetPosition(loadNodeData.nodeRect);
            }
            return thisNode;





            
        }


        public DecisionNode Create_DecisionNode(AI_DecisionData loadData = null, CustomNode_Data loadNodeData = null)
        {
            // --- Generate Node
            var thisNode = new DecisionNode();
            thisNode.title = "DECISION";
            thisNode.mainContainer.contentContainer.AddToClassList("contentContainer");
            // --- GUID
            thisNode.data.GUID = ReturnGUID(thisNode);


            // --- Generate Input Port
            {
                Port inputPort = GeneratePort(thisNode, Direction.Input, typeof(Port_Actions), Port.Capacity.Multi);
                inputPort.portName = "Input";
                inputPort.portColor = Color.magenta;

                thisNode.inputContainer.Add(inputPort);

            }

            // --- Generate Output Ports
            {
                Port outputPort;
                outputPort = GeneratePort(thisNode, Direction.Output, typeof(Port_Actions), Port.Capacity.Single);
                outputPort.portName = "True";
                outputPort.portColor = Color.magenta;


                thisNode.outputContainer.Add(outputPort);

                outputPort = GeneratePort(thisNode, Direction.Output, typeof(Port_Actions), Port.Capacity.Single);
                outputPort.portName = "False";
                outputPort.portColor = Color.magenta;


                thisNode.outputContainer.Add(outputPort);
            }

            {
                Box dynamicValuesContainer = new Box();
                thisNode.mainContainer.Add(dynamicValuesContainer);

                ObjectField field = new ObjectField()
                {
                    objectType = typeof(AI_Decision)
                };
                field.RegisterValueChangedCallback(x =>
                {
                    thisNode.data.decision = (AI_Decision)field.value;
                    field.tooltip = field.value.name;

                    AddDynamicValues();
                    
                });
                if (loadData != null)
                {
                    if (loadData.decision != null)
                    {
                        thisNode.data.decision = loadData.decision;

                        field.value = loadData.decision;
                        field.tooltip = field.value.name;

                        AddDynamicValues();
                    }         
                }

                void AddDynamicValues()
                {
                    dynamicValuesContainer.Clear(); // clear on update to delete ports from prev script

                    thisNode.data.dynamicRefValues = thisNode.data.decision.dynamicRefValues;

                    foreach (var value in thisNode.data.dynamicRefValues.boolValues)
                    {
                        // --- bool Port
                        {
                            Port inputPort = GeneratePort(thisNode, Direction.Input, typeof(bool), Port.Capacity.Multi);
                            inputPort.portName = "(bool) " + value.Key;
                            inputPort.portColor = Color.white;

                            dynamicValuesContainer.Add(inputPort);
                        }
                    }
                    foreach (var value in thisNode.data.dynamicRefValues.floatValues)
                    {
                        // --- float Port
                        {
                            Port inputPort = GeneratePort(thisNode, Direction.Input, typeof(float), Port.Capacity.Multi);
                            inputPort.portName = "(float) " + value.Key;
                            inputPort.portColor = Color.white;

                            dynamicValuesContainer.Add(inputPort);

                        }
                    }
                    foreach (var value in thisNode.data.dynamicRefValues.intValues)
                    {
                        // --- int Port
                        {
                            Port inputPort = GeneratePort(thisNode, Direction.Input, typeof(int), Port.Capacity.Multi);
                            inputPort.portName = "(int) " + value.Key;
                            inputPort.portColor = Color.white;

                            dynamicValuesContainer.Add(inputPort);
                        }
                    }

                }

                //field.AddToClassList("actionField");
                thisNode.mainContainer.Add(field);
            }




            thisNode.RefreshExpandedState();
            thisNode.RefreshPorts();


            if (loadData != null)
            {
                thisNode.data = loadData;
                thisNode.nodeData.nodeGUID = loadData.GUID;
            }
            if (loadNodeData != null)
            {
                thisNode.SetPosition(loadNodeData.nodeRect);
            }
            return thisNode;
        }

     

        public ValueNode Create_ValueNode(AI_ValueData loadData = null, CustomNode_Data loadNodeData = null)
        {
            // --- Generate Node
            var thisNode = new ValueNode();
            thisNode.title = "Value";
            thisNode.inputContainer.AddToClassList("horizontal-arrangement");
            // --- GUID
            thisNode.data.GUID = ReturnGUID(thisNode);



            EnumField operatorField = new EnumField(AI_ValueData.supportedTypes.unassigned);
            operatorField.RegisterValueChangedCallback(evt =>
            {
                thisNode.data.valueType = (AI_ValueData.supportedTypes)evt.newValue;
                switch (thisNode.data.valueType)
                {
                    case AI_ValueData.supportedTypes.type_int:
                        {
                            SetUpNode(typeof(int));
                            break;
                        }
                    case AI_ValueData.supportedTypes.type_float:
                        {
                            SetUpNode(typeof(float));
                            break;
                        }
                    case AI_ValueData.supportedTypes.type_bool:
                        {
                            SetUpNode(typeof(bool));
                            break;
                        }
                }             
            });
            thisNode.mainContainer.Add(operatorField);

            if (loadData != null)
            {
                if (loadData.valueType != AI_ValueData.supportedTypes.unassigned)
                {
                    //thisNode.data.valueType = loadData.valueType;
                    switch (loadData.valueType)
                    {
                        case AI_ValueData.supportedTypes.type_int:
                            {
                                SetUpNode(typeof(int));
                                break;
                            }
                        case AI_ValueData.supportedTypes.type_float:
                            {
                                SetUpNode(typeof(float));
                                break;
                            }
                        case AI_ValueData.supportedTypes.type_bool:
                            {
                                SetUpNode(typeof(bool));
                                break;
                            }
                    }
                } 
            }


            void SetUpNode(Type valueType)
            {
                thisNode.mainContainer.Remove(operatorField);
                thisNode.titleContainer.AddToClassList("valueNodeTitle");

                AddPorts(valueType);
                if (valueType == typeof(int))
                {                
                    IntegerField field = new IntegerField();
                    field.AddToClassList("valueNodeField");
                    field.RegisterValueChangedCallback(evt =>
                    {
                        thisNode.data.intValue = field.value;
                    });
                    thisNode.data.intValue = field.value;
                    thisNode.inputContainer.Add(field);

                    if (loadData != null)
                    {
                        field.value = loadData.intValue;
                    }
                }
                else if (valueType == typeof(float))
                {
                    FloatField field = new FloatField();
                    field.AddToClassList("valueNodeField");
                    field.RegisterValueChangedCallback(evt =>
                    {
                        thisNode.data.floatValue = evt.newValue;
                    });
                    thisNode.data.floatValue = field.value;
                    thisNode.inputContainer.Add(field);

                    if (loadData != null)
                    {
                        field.value = loadData.floatValue;
                    }
                }
                else if (valueType == typeof(bool))
                {
                    Toggle toggle = new Toggle();
                    toggle.RegisterValueChangedCallback(evt =>
                    {
                        thisNode.data.boolValue = evt.newValue;
                    });
                    thisNode.data.boolValue = toggle.value;
                    thisNode.inputContainer.Add(toggle);

                    if (loadData != null)
                    {
                        toggle.value = loadData.boolValue;
                    }
                }
                else
                {
                    RemoveElement(thisNode); // to prevent impossible errors?..
                    Debug.Log("ERROR: CREATING NODE WITH UNSUPPORTED VALUE TYPE = " + valueType.ToString());
                }

                void AddPorts(Type valueType)
                {
                    // --- Generate Input Port
                    {
                        Port inputPort = GeneratePort(thisNode, Direction.Input, valueType, Port.Capacity.Multi);
                        inputPort.portColor = Color.white;
                        inputPort.portName = "Input";
                        inputPort.contentContainer.Q<Label>(name: "type").AddToClassList("port-labelHidden");
                        thisNode.inputContainer.Add(inputPort);
                    }

                    // --- Generate Output Port
                    {
                        Port outputPort = GeneratePort(thisNode, Direction.Output, valueType, Port.Capacity.Multi);
                        outputPort.portColor = Color.white;
                        outputPort.portName = "Output";
                        thisNode.outputContainer.Add(outputPort);
                    }
                }
               
                thisNode.RefreshExpandedState();
                thisNode.RefreshPorts();

            }

            thisNode.RefreshExpandedState();
            thisNode.RefreshPorts();

            if (loadData != null)
            {
                thisNode.data = loadData;
                thisNode.nodeData.nodeGUID = loadData.GUID;
            }
            if (loadNodeData != null)
            {
                thisNode.SetPosition(loadNodeData.nodeRect);
            }
            return thisNode;
        }



        public ValueChangerNode Create_ValueChangerNode(AI_ValueChangerData loadData = null, CustomNode_Data loadNodeData = null)
        {
            // --- Generate Node
                var thisNode = new ValueChangerNode();
                thisNode.title = "Value Changer";
                thisNode.mainContainer.contentContainer.AddToClassList("contentContainer");
            // --- GUID
                thisNode.data.GUID = ReturnGUID(thisNode);

            // --- Generate Input Port
            {
                Port inputPort = GeneratePort(thisNode, Direction.Input, typeof(Port_Actions), Port.Capacity.Multi);
                inputPort.portName = "Input";
                inputPort.portColor = Color.green;

                thisNode.inputContainer.Add(inputPort);
            }
            // --- Generate Output Port
            {
                Port outputPort = GeneratePort(thisNode, Direction.Output, typeof(Port_Actions), Port.Capacity.Single);
                outputPort.portName = "Output";
                outputPort.portColor = Color.green;

                thisNode.outputContainer.Add(outputPort);
            }

            // --- Before generating Node correctly, select value type
                EnumField selectTypeField = new EnumField(AI_ValueChangerData.supportedTypes.unassigned);
                selectTypeField.RegisterValueChangedCallback(evt =>
                {
                    thisNode.data.valueType = (AI_ValueChangerData.supportedTypes)evt.newValue;
                    switch (thisNode.data.valueType)
                    {
                        case AI_ValueChangerData.supportedTypes.type_int:
                            {
                                SetUpNode(typeof(int));
                                break;
                            }
                        case AI_ValueChangerData.supportedTypes.type_float:
                            {
                                SetUpNode(typeof(float));
                                break;
                            }
                        case AI_ValueChangerData.supportedTypes.type_bool:
                            {
                                SetUpNode(typeof(bool));
                                break;
                            }
                    }
                });

                thisNode.mainContainer.Add(selectTypeField);

            // --- Assign ValueType and autogenerate the correct node variation if we load behavior from file
                if (loadData != null)
                {
                    if (loadData.valueType != AI_ValueChangerData.supportedTypes.unassigned)
                    {
                        switch (loadData.valueType)
                        {
                            case AI_ValueChangerData.supportedTypes.type_int:
                                {
                                    SetUpNode(typeof(int));
                                    break;
                                }
                            case AI_ValueChangerData.supportedTypes.type_float:
                                {
                                    SetUpNode(typeof(float));
                                    break;
                                }
                            case AI_ValueChangerData.supportedTypes.type_bool:
                                {
                                    SetUpNode(typeof(bool));
                                    break;
                                }
                        }
                    }
                    
                }

            void SetUpNode(Type valueType)
            {
                thisNode.mainContainer.Remove(selectTypeField);

                Box dynamicValuesContainer = new Box();
                thisNode.mainContainer.Add(dynamicValuesContainer);

                AddValuePort(valueType);

                if (valueType == typeof(int))
                {
                    ObjectField field = new ObjectField()
                    {
                        objectType = typeof(AI_ValueChanger_Int)
                    };
                    field.RegisterValueChangedCallback(x =>
                    {
                        thisNode.data.valueChanger = (AI_ValueChanger_Int)field.value;
                        field.tooltip = field.value.name;
                        AddDynamicValues(thisNode.data.valueChanger.dynamicValues);
                    });

                    thisNode.mainContainer.Add(field);

                    if (loadData != null)
                    {
                        if (loadData.valueChanger != null)
                        {
                            field.value = loadData.valueChanger;
                            field.tooltip = field.value.name;
                            AddDynamicValues(loadData.valueChanger.dynamicValues);
                        }
                    }
                }
                else if (valueType == typeof(float))
                {
                    ObjectField field = new ObjectField()
                    {
                        objectType = typeof(AI_ValueChanger_Float)
                    };
                    field.RegisterValueChangedCallback(x =>
                    {
                        thisNode.data.valueChanger = (AI_ValueChanger_Float)field.value;
                        field.tooltip = field.value.name;
                        AddDynamicValues(thisNode.data.valueChanger.dynamicValues);
                    });
                    thisNode.mainContainer.Add(field);

                    if (loadData != null)
                    {
                        if (loadData.valueChanger != null)
                        {
                            field.value = loadData.valueChanger;
                            field.tooltip = field.value.name;
                            AddDynamicValues(loadData.valueChanger.dynamicValues);
                        }
                    }
                }
                else if (valueType == typeof(bool))
                {
                    ObjectField field = new ObjectField()
                    {
                        objectType = typeof(AI_ValueChanger_Bool)
                    };
                    field.RegisterValueChangedCallback(x =>
                    {
                        thisNode.data.valueChanger = (AI_ValueChanger_Bool)field.value;
                        field.tooltip = field.value.name;
                        AddDynamicValues(thisNode.data.valueChanger.dynamicValues);
                    });
                    thisNode.mainContainer.Add(field);

                    if (loadData != null)
                    {
                        if (loadData.valueChanger != null)
                        {
                            field.value = loadData.valueChanger;
                            field.tooltip = field.value.name;
                            AddDynamicValues(loadData.valueChanger.dynamicValues);
                        }
                    }
                }
                else
                {
                    RemoveElement(thisNode); // to prevent impossible errors?..
                    Debug.Log("ERROR: CREATING NODE WITH UNSUPPORTED VALUE TYPE");
                }
                
                void AddValuePort(Type type)
                {
                    // --- Generate value port
                    {
                        Port outputPort = GeneratePort(thisNode, Direction.Output, type, Port.Capacity.Single);
                        outputPort.portName = "Value";
                        outputPort.portColor = Color.white;

                        thisNode.mainContainer.Add(outputPort);
                    }
                }

                void AddDynamicValues(AI_DynamicValues dynamicValues)
                {
                    dynamicValuesContainer.Clear();
                    thisNode.data.dynamicValues = dynamicValues;

                    foreach (var value in thisNode.data.dynamicValues.boolValues)
                    {
                        // --- bool Port
                        {
                            Port inputPort = GeneratePort(thisNode, Direction.Input, typeof(bool), Port.Capacity.Multi);
                            inputPort.portName = "(bool) " + value.Key;
                            inputPort.portColor = Color.white;

                            dynamicValuesContainer.Add(inputPort);
                        }
                    }
                    foreach (var value in thisNode.data.dynamicValues.floatValues)
                    {
                        // --- float Port
                        {
                            Port inputPort = GeneratePort(thisNode, Direction.Input, typeof(float), Port.Capacity.Multi);
                            inputPort.portName = "(float) " + value.Key;
                            inputPort.portColor = Color.white;

                            dynamicValuesContainer.Add(inputPort);

                        }
                    }
                    foreach (var value in thisNode.data.dynamicValues.intValues)
                    {
                        // --- int Port
                        {
                            Port inputPort = GeneratePort(thisNode, Direction.Input, typeof(int), Port.Capacity.Multi);
                            inputPort.portName = "(int) " + value.Key;
                            inputPort.portColor = Color.white;

                            dynamicValuesContainer.Add(inputPort);
                        }
                    }

                }


                thisNode.RefreshExpandedState();
                thisNode.RefreshPorts();

            }

            thisNode.RefreshExpandedState();
            thisNode.RefreshPorts();

            // --- Load the rest of data properly (in the end to avoid accidental override)
            if (loadData != null)
            {
                thisNode.data = loadData;
                thisNode.nodeData.nodeGUID = loadData.GUID;
            }
            if (loadNodeData != null)
            {
                thisNode.SetPosition(loadNodeData.nodeRect);
            }
            return thisNode;
        }



        public TimerNode Create_TimerNode(AI_TimerData loadData = null, CustomNode_Data loadNodeData = null)    
        {
            // --- Generate Node
            var thisNode = new TimerNode();

            thisNode.title = "Timer";
            thisNode.mainContainer.contentContainer.AddToClassList("contentContainer");
            // --- GUID
            thisNode.data.GUID = ReturnGUID(thisNode);

            // --- Generate Input Port
            {
                Port inputPort = GeneratePort(thisNode, Direction.Input, typeof(Port_Actions), Port.Capacity.Multi);
                inputPort.portName = "Input";
                inputPort.portColor = Color.cyan;

                thisNode.inputContainer.Add(inputPort);
            }

            Box content = new Box();
            content.AddToClassList("horizontalContent");

            thisNode.mainContainer.Add(content);

            // --- Time Interval
            {
                Box box = new Box();
                box.AddToClassList("centeredContent");
                content.Add(box);

                Label label = new Label("Time Interval:");
                label.AddToClassList("timerFieldLabel");
                box.Add(label);

                FloatField field = new FloatField();
                field.RegisterValueChangedCallback(evt =>
                {
                    thisNode.data.timeInterval = evt.newValue;
                });
                field.AddToClassList("timerField");
                box.Add(field);

                if (loadData != null)
                {
                    field.value = loadData.timeInterval;
                }
            }
            // --- Interval Randomizer
            {
                Box box = new Box();
                box.AddToClassList("centeredContent");
                content.Add(box);

                Label label = new Label("Random Offset:");
                label.AddToClassList("timerFieldLabel");
                box.Add(label);


                FloatField field = new FloatField();
                field.RegisterValueChangedCallback(evt =>
                {
                    thisNode.data.intervalRandomOffset = evt.newValue;
                });
                field.AddToClassList("timerField");
                box.Add(field);

                if (loadData != null)
                {
                    field.value = loadData.intervalRandomOffset;
                }
            }
            

            // --- Generate Output Ports
            {
                Port outputPort_False;
                outputPort_False = GeneratePort(thisNode, Direction.Output, typeof(Port_Actions), Port.Capacity.Single);
                outputPort_False.portName = "False";
                outputPort_False.portColor = Color.cyan;
                thisNode.outputContainer.Add(outputPort_False);


                Port outputPort_True;
                outputPort_True = GeneratePort(thisNode, Direction.Output, typeof(Port_Actions), Port.Capacity.Single);
                outputPort_True.portName = "True";
                outputPort_True.portColor = Color.cyan;
                thisNode.outputContainer.Add(outputPort_True);              
            }


            // --- Load the rest of data properly in the end to avoid potential override
            if (loadData != null)
            {
                thisNode.data = loadData;
                thisNode.nodeData.nodeGUID = loadData.GUID;
                Debug.Log("timer assigned with loaded guid = " + loadData.GUID);
            }
            if (loadNodeData != null)
            {
                thisNode.SetPosition(loadNodeData.nodeRect);
            }
            return thisNode;
        }
    }
}
