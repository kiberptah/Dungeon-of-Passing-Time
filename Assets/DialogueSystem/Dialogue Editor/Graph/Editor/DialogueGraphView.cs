using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

using System;
using System.Linq;

public class DialogueGraphView : GraphView
{
    Vector2 defaultNodeSize = new Vector2(150, 200); // it doesnt even affect things
    float dialogueTextFieldMaxLengh = 270;

    //GlobalVariables storyVariables = GameObject.Find("GlobalVeriablesDirector").GetComponent<GlobalVariables>(); // temp reference needs to be removed for modularity?

    public StoryVariablesContainer storyVariables;
    public StoryEventsContainer storyEvents;

    NodeSearchWindow searchWindow;

    public DialogueGraphView(EditorWindow _editorWindow)
    {
        storyVariables = Resources.Load<StoryVariablesContainer>(path: StoryVariablesContainer.storyVariablesPath);
        if (storyVariables == null)
        {
            EditorUtility.DisplayDialog(title: "Error!@#$", message: "StoryVariables file not found", ok: "Fuck...");
            return;
        }
        storyEvents = Resources.Load<StoryEventsContainer>(path: StoryEventsContainer.storyEventsPath);
        if (storyEvents == null)
        {
            EditorUtility.DisplayDialog(title: "Error!@#$", message: "StoryEvents file not found", ok: "Fuuuuuuck");
            return;
        }

        styleSheets.Add(styleSheet: Resources.Load<StyleSheet>(path: "StyleSheets/DialogueGraph")); // uss file for visual customization of elements
        
        // basic stuff from API to intercat with elements
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale); // without it there's no zoom
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        // Just a visual grid so it looks nice
        var grid = new GridBackground();
        Insert(index: 0, grid);
        grid.StretchToParentSize();


        // Creating Entry Node
        //AddElement(CreateDialogueNode(isEntryPoint: true));
        AddElement(CreateEntryNode());
        AddSearchWindow(_editorWindow);
        
    }

    void AddSearchWindow(EditorWindow _editorWindow)
    {
        searchWindow = ScriptableObject.CreateInstance<NodeSearchWindow>();
        searchWindow.Init(_editorWindow, this);
        
        nodeCreationRequest = context =>
             SearchWindow.Open(new SearchWindowContext(context.screenMousePosition, requestedHeight: 0.5f), searchWindow);
        
    }

    /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// ///
    /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// ///
    /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// ///
    
    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        /// I think it overrides some default Unity function which is called when you start dragging from an output port
        /// Without this functionn you can't connect ports
        /// also it has logic to prevent illogical node connections

        var compatablePorts = new List<Port>();
        ports.ForEach(funcCall: (port) =>
        {
            if (startPort != port && startPort.node != port.node)
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
    Port GeneratePort(Node _node, Direction _portDirection, Port.Capacity _capacity = Port.Capacity.Single)
    {
        /// Pretty simple, it generates a port
        //return _node.InstantiatePort(Orientation.Horizontal, _portDirection, _capacity, typeof(float)); //typeof is arbitrary for dialogue system?..

        /// and also sets up it the right way visually and stuff

        var port = _node.InstantiatePort(Orientation.Horizontal, _portDirection, _capacity, typeof(float));

        port.portName = _portDirection.ToString();

        // hide port name cause we don't need to see it...
        Label portLabel = port.contentContainer.Q<Label>(name: "type");
        portLabel.style.visibility = Visibility.Hidden;
        portLabel.style.minWidth = 1;
        portLabel.style.maxWidth = 1;

        return port;

        //dialogueNode.inputContainer.Add(inputPort); //

    }
    void RemovePort(Port _port, VisualElement _portParent)
    {
        /// it removes port, duh
        /// before that it dosconnects and removes an edge
        /// so it  won't be left haging around without being connected

        Node node = _port.node;

        /// First delete edge
        // I just copipasted this line i'd write it using foreach because I suck
        var targetEdge = edges.ToList().Where(edge => edge.output.portName == _port.portName && edge.output.node == _port.node);

        if (targetEdge.Any())
        {
            foreach (Edge edge in targetEdge)
            {
                //Edge edge = targetEdge.First();
                edge.input.Disconnect(edge);
                RemoveElement(edge);
            }                   
        }

        /// Then delete port
        //node.outputContainer.Remove(_port);
        _portParent.Remove(_port);

        /// update visuals on the graph
        node.RefreshPorts();
        node.RefreshExpandedState();
    }

    /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// ///
    /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// ///
    /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// ///
    
    public CommentsNode CreateCommentsNode(Vector2 position, string _overrideGUID = "", string _commentText = "")
    {
        // GUID
        string newGUID = Guid.NewGuid().ToString();
        if (!string.IsNullOrEmpty(_overrideGUID))
        {
            newGUID = _overrideGUID;
        }
        // TEXT
        string newText = "";
        if (!string.IsNullOrEmpty(_commentText))
        {
            newText = _commentText;
        }
        // Generate Node
        var commentsNode = new CommentsNode()
        {
            GUID = newGUID,
            title = "Comment",
            text = newText,

        };
        commentsNode.SetPosition(new Rect(position, defaultNodeSize));
        //commentsNode.titleContainer.style.backgroundColor = Color.yellow;


        // ----- Add text field
        {
            TextField textField = new TextField(label: string.Empty)
            {
                multiline = true
            };
            textField.style.maxWidth = dialogueTextFieldMaxLengh;

            //textField.value = commentsNode.text;
            textField.RegisterValueChangedCallback(evt =>
            {
                // as far as I understand each times you change the text event (evt) is fired and triggers code below
                commentsNode.text = evt.newValue;
            });
            textField.SetValueWithoutNotify(commentsNode.text);

            commentsNode.mainContainer.Add(textField);
        }

        // ----- Update visuals on the graph
        commentsNode.styleSheets.Add(styleSheet: Resources.Load<StyleSheet>(path: "StyleSheets/Node"));

        // set up node width so it won't changeand move button when we add choices 
        //commentsNode.style.minWidth = 300;
        //commentsNode.style.maxWidth = 300;
        commentsNode.style.width = 150;


        commentsNode.RefreshExpandedState();
        return commentsNode;
    }

    public EntryNode CreateEntryNode(string _overrideGUID = "")
    {
        // GUID
        string newGUID = Guid.NewGuid().ToString();
        if (!string.IsNullOrEmpty(_overrideGUID))
        {
            newGUID = _overrideGUID;
        }
        // Generate Node
        var entryNode = new EntryNode
        {
            GUID = newGUID,
            title = "ENTRY NODE"
            
        };


        // probably a good idea for an entry point to be unmovable and undeletable.
        //entryNode.capabilities &= ~Capabilities.Movable;
        entryNode.capabilities &= ~Capabilities.Deletable;

        // ----- Generate Output Port
        {
            var outputPort = GeneratePort(entryNode, Direction.Output, Port.Capacity.Single);
            outputPort.portName = "Output";
            entryNode.outputContainer.Add(outputPort);

            // hide port name cause we don't need to see it...

            Label portLabel = outputPort.contentContainer.Q<Label>(name: "type");
            portLabel.style.visibility = Visibility.Hidden;
            portLabel.style.minWidth = 1;
            portLabel.style.maxWidth = 1;

        }

        entryNode.styleSheets.Add(styleSheet: Resources.Load<StyleSheet>(path: "StyleSheets/Node"));
        
        entryNode.RefreshExpandedState();
        entryNode.RefreshPorts();

        return entryNode;

    }
    public EndNode CreateEndNode(Vector2 position, string _overrideGUID = "")
    {
        // GUID
        string newGUID = Guid.NewGuid().ToString();
        if (!string.IsNullOrEmpty(_overrideGUID))
        {
            newGUID = _overrideGUID;
        }
        // Generate Node
        var endNode = new EndNode()
        {
           
            GUID = newGUID,
            title = "END NODE"

        };
        endNode.SetPosition(new Rect(position, defaultNodeSize));
        // ----- Generate Input Port
        {
            var inputPort = GeneratePort(endNode, Direction.Input, Port.Capacity.Multi);
            inputPort.portName = "Input";
            endNode.inputContainer.Add(inputPort);

            // hide port name cause we don't need to see it...         
            Label portLabel = inputPort.contentContainer.Q<Label>(name: "type");
            portLabel.style.visibility = Visibility.Hidden;
            portLabel.style.minWidth = 1;
            portLabel.style.maxWidth = 1;
            
        }
        endNode.styleSheets.Add(styleSheet: Resources.Load<StyleSheet>(path: "StyleSheets/Node"));

        endNode.RefreshExpandedState();
        endNode.RefreshPorts();

        return endNode;
    }
    public EventTriggerNode CreateEventNode(Vector2 position, string _overrideGUID = "", List<DialogueEventTrigger> _loadedEventTriggers = null)
    {
        // ----- GUID
        string newGUID = Guid.NewGuid().ToString();
        if (!string.IsNullOrEmpty(_overrideGUID))
        {
            newGUID = _overrideGUID;
        }
        // ----- Generate Node
        var eventNode = new EventTriggerNode();
        eventNode.GUID = newGUID;
        eventNode.title = "Event Trigger Node";
        eventNode.style.minWidth = 250;
        eventNode.SetPosition(new Rect(position, defaultNodeSize));
        // ----- Generate Input Port
        {
            var inputPort = GeneratePort(eventNode, Direction.Input, Port.Capacity.Single);
            inputPort.portName = "Input";
            eventNode.inputContainer.Add(inputPort);

            // hide port name cause we don't need to see it...
            Label portLabel = inputPort.contentContainer.Q<Label>(name: "type");
            portLabel.style.visibility = Visibility.Hidden;
            portLabel.style.minWidth = 1;
            portLabel.style.maxWidth = 1;
        }
        // ----- Generate Output Port
        {
            var outputPort = GeneratePort(eventNode, Direction.Output, Port.Capacity.Single);
            outputPort.portName = "Output";
            eventNode.outputContainer.Add(outputPort);

            // hide port name cause we don't need to see it...
            Label portLabel = outputPort.contentContainer.Q<Label>(name: "type");
            portLabel.style.visibility = Visibility.Hidden;
            portLabel.style.minWidth = 1;
            portLabel.style.maxWidth = 1;
        }
        // ----- Add Button that adds new event
        {
            Button addRedirectionButt = new Button(clickEvent: () => AddEventTrigger())
            {
                text = "Add Event Trigger"
            };
            void AddEventTrigger(DialogueEventTrigger _trigger = null)
            {
                Box boxContainer = new Box(); // container with equation
                boxContainer.style.flexDirection = FlexDirection.Row;

                DialogueEventTrigger eventTrigger = new DialogueEventTrigger(eventNode.GUID)
                {
                    //nodeGUID = eventNode.GUID
                };
                if (_trigger == null)
                {
                    _trigger = eventTrigger;
                }
                // ----- Add button to remove event trigger (goes in the beginning for better ux)
                {
                    Button removeCondButt = new Button(clickEvent: () => RemoveEventTrigger())
                    {
                        text = "X"
                    };
                    void RemoveEventTrigger()
                    {
                        // just remove the boxcontainer that has all the redirection-related stuff
                        eventNode.mainContainer.Remove(boxContainer);
                        eventNode.allDialogueEventTriggers.Remove(_trigger);
                    }
                    boxContainer.Add(removeCondButt);
                }
                // ----- Add select event dropdown
                {
                    if (_trigger.eventName != null)
                        boxContainer.Add(selectEventMenu(_trigger, _trigger.eventName.ToString()));
                    else
                        boxContainer.Add(selectEventMenu(_trigger));
                }
                // ----- Add the box container with everything we need inside to node
                eventNode.mainContainer.Add(boxContainer);

                // -----
                eventTrigger = _trigger;
                eventNode.allDialogueEventTriggers.Add(eventTrigger);
            }
            // ----- Add button on the node
            {
                eventNode.mainContainer.Add(addRedirectionButt);
            }



            // ----- Add existing eventTriggers if node is loaded
            if (_loadedEventTriggers != null)
            {
                foreach (var entry in _loadedEventTriggers)
                {
                    AddEventTrigger(entry);
                }
            }

            
        }

        eventNode.styleSheets.Add(styleSheet: Resources.Load<StyleSheet>(path: "StyleSheets/Node"));
        // ----- return
        return eventNode;
    }

    ToolbarMenu selectEventMenu(DialogueEventTrigger _eventTrigger, string menuText = "")
    {
        ToolbarMenu selectVarMenu = new ToolbarMenu();
        selectVarMenu.style.height = 20;
        selectVarMenu.style.width = 200;
        selectVarMenu.style.alignSelf = Align.Center;
        selectVarMenu.text = menuText;
        //selectVarMenu.style.marginLeft = 5;

        // put every global variable into this list... Cause we might need any of them
        foreach (var entry in storyEvents.eventsList)
        {
            selectVarMenu.menu.AppendAction(actionName: entry.eventName, action: new Action<DropdownMenuAction>(x => selectEvent()));
            void selectEvent()
            {
                selectVarMenu.text = entry.eventName;
                _eventTrigger.eventName = entry.eventName;
            }
        }
        return selectVarMenu;
        //boxContainer.Add(selectVarMenu);
    }







    public DialogueNode CreateDialogueNode(Vector2 position, string _overrideGUID = "", 
        string _speakerName = "",
        Sprite _speakerPortrait = null,
        string _nodeText = "", 
        List<DialogueChoice> dialogueChoices = null,
        List<DialogueChoiceCondition> _loadedConditions = null)
    {
        /// When calling this function use graphView.AddElement(graphView.CreateDialogueNode();
        /// or 
        /// 1) DialogueNode tempNode = targetGraphView.CreateDialogueNode (...);
        /// 2) targetGraphView.AddElement(tempNode);

        var dialogueNode = new DialogueNode();


        /// Set up parameters
        /// 
        {
            /// Assign stuff in case of override 
            /// because when we create new node we need default values
            /// but when we load we need to use what we have
            /// and we can't update them after loading cause I don't know how to update TextFields      

            // GUID
            string newGUID = Guid.NewGuid().ToString();
            if (!string.IsNullOrEmpty(_overrideGUID))
            {
                newGUID = _overrideGUID;
            }
            // TEXT
            string newText = "";
            if (!string.IsNullOrEmpty(_nodeText))
            {
                newText = _nodeText;
            }
            ///
            dialogueNode = new DialogueNode
            {
                title = newText,
                GUID = newGUID,
                dialogueText = newText,
            };
            dialogueNode.SetPosition(new Rect(position, defaultNodeSize));
            dialogueNode.capabilities &= ~Capabilities.Collapsible;
        }
        // ----- ADD INPUT PORT 
        {
            var inputPort = GeneratePort(dialogueNode, Direction.Input, Port.Capacity.Multi);
            // hide port name cause we don't need to see it...
            Label portLabel = inputPort.contentContainer.Q<Label>(name: "type");
            portLabel.style.visibility = Visibility.Hidden;
            portLabel.style.minWidth = 1;
            portLabel.style.maxWidth = 1;

            dialogueNode.inputContainer.Add(inputPort);

            //

            //
            /// add button to remove input connections
            Button deleteEdgeButton = new Button(clickEvent: () => DisconnectEdge())
            {
                text = "X"
            };
            inputPort.contentContainer.Add(deleteEdgeButton);

            void DisconnectEdge()
            {
                foreach (Edge edge in inputPort.connections)
                {
                    edge.output.Disconnect(edge);
                    RemoveElement(edge);
                }
                inputPort.DisconnectAll();
            }
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////
        /// Below we set up visual elements of the node, but they have important functionality!
        /// ORDER OF ELEMENTS AFFECTS (sometimes...) visual composition on the node
        ///
        ///
        void textSeparator(string _text)
        {
            /// some text to tell what is the element below, for example
            /// 
            TextElement textline = new TextElement();
            textline.text = _text;
            textline.style.maxWidth = 200;
            textline.style.fontSize = 13;

            textline.style.whiteSpace = WhiteSpace.NoWrap;
            dialogueNode.mainContainer.Add(textline);
        }
        /// TEXT FIELD FOR CHAR NAME
        {
            textSeparator("\n— Speaker Name:");
            string charName = "";
            if (!string.IsNullOrEmpty(_speakerName))
            {
                charName = _speakerName;
            }
            TextField textField = new TextField(label: string.Empty)
            {
                multiline = true
            };
            textField.style.maxWidth = dialogueTextFieldMaxLengh;

            textField.RegisterValueChangedCallback(evt =>
            {
                // as far as I understand each times you change the text event (evt) is fired and triggers code below
                dialogueNode.speakerName = evt.newValue;
            });
            //textField.SetValueWithoutNotify(dialogueNode.speakerName);

            dialogueNode.speakerName = charName;
            textField.value = charName;
            //textField.SetValueWithoutNotify(charName);
            //dialogueNode.RefreshExpandedState();


            dialogueNode.mainContainer.Add(textField);
        }
        ///
        ///SPRITE FOR CHARACTER PORTRAIT
        {
            textSeparator("\n— Speaker Portrait:");

            ObjectField spriteField = new ObjectField()
            {
                objectType = typeof(Sprite)
            };           
            spriteField.RegisterValueChangedCallback(x =>
            {
                dialogueNode.speakerPortrait = (Sprite)spriteField.value;             
            });

            if (_speakerPortrait != null)
            {
                spriteField.value = _speakerPortrait;
                dialogueNode.speakerPortrait = _speakerPortrait;
            }
            dialogueNode.mainContainer.Add(spriteField);
        }
        /// TEXT FIELD FOR DIALOGUE TEXT (the most important thing for the node!)
        {
            textSeparator("\n— Dialogue Text:");

            TextField textField = new TextField(label: string.Empty)
            {
                multiline = true
            };
            textField.style.maxWidth = dialogueTextFieldMaxLengh;

            textField.RegisterValueChangedCallback(evt =>
            {
                // as far as I understand each times you change the text event (evt) is fired and triggers code below
                dialogueNode.dialogueText = evt.newValue;

                // Set node name as node text but trim it down to X characters to make it visually pleasant cause why would you won't your text displayed 2 times
                dialogueNode.title = evt.newValue;
                if (dialogueNode.title.Length > 20)
                {
                    dialogueNode.title = evt.newValue.Substring(0, 20) + "...";
                }

            });
            textField.SetValueWithoutNotify(dialogueNode.dialogueText);
            dialogueNode.mainContainer.Add(textField);
        }
        ///
        /// BUTTON TO CREATE NEW OUTPUT PORTS (CHOICES)
        {
            var button = new Button(clickEvent: () => { AddChoicePort(dialogueNode); });
            button.text = "New Choice";
            dialogueNode.titleContainer.Add(button);
            //

        }
        ///
        /// Generate existing choices if node is loaded
        if (dialogueChoices != null)
        {
            foreach (var choice in dialogueChoices)
            {
                AddChoicePort(dialogueNode, choice);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        // ----- Update visuals on the graph
        dialogueNode.styleSheets.Add(styleSheet: Resources.Load<StyleSheet>(path: "StyleSheets/Node"));

        // set up node width so it won't changeand move button when we add choices 
        dialogueNode.style.minWidth = 400;

        dialogueNode.RefreshExpandedState();
        dialogueNode.RefreshPorts();
        //dialogueNode.SetPosition(new Rect(Vector2.zero, Vector2.one));

        ///////////////////////////////////////////////////////////////////////////////////////////////
        return dialogueNode;
    }
    public void AddChoicePort(DialogueNode _dialogueNode, DialogueChoice _choice = null)
    {
        float portNameFieldWidth = 250f; // if you choices texts are long (e.g. rpg game), increase this value


        Box boxContainer = new Box();
        boxContainer.style.flexDirection = FlexDirection.Column;
        boxContainer.style.alignSelf = Align.Auto;

        DialogueChoice newChoice = new DialogueChoice();

        Box choiceContainer = new Box();
        choiceContainer.style.flexDirection = FlexDirection.Row;
        choiceContainer.style.alignSelf = Align.Auto;

        /// Creates another output port to the node
        ///
        Port generatedPort = GeneratePort(_dialogueNode, Direction.Output);
        generatedPort.style.alignSelf = Align.FlexEnd;

        int outputPortCount = _dialogueNode.outputContainer.Query(name: "connector").ToList().Count;

        string choicePortName = $"Choice {outputPortCount}";
        if (_choice != null)
        {
            choicePortName = _choice.choiceName;
        }
        generatedPort.portName = choicePortName;
        newChoice.choiceName = generatedPort.portName;

        // add button to delete choice
        {
            Button deleteChoiceButton = new Button(clickEvent: () => RemoveChoice())
            {
                text = "X"
            };
            void RemoveChoice()
            {
                RemovePort(generatedPort, choiceContainer);
                _dialogueNode.outputContainer.Remove(boxContainer);
                _dialogueNode.choices.Remove(newChoice);
            }
            //generatedPort.contentContainer.Add(deleteChoiceButton);
            choiceContainer.Add(deleteChoiceButton);
        }

        // add text field for choice text
        {
            TextField textField = new TextField
            {
                name = string.Empty,
                value = choicePortName
            };
            textField.style.minWidth = portNameFieldWidth;
            textField.style.maxWidth = portNameFieldWidth;
            textField.style.minHeight = 20;

            textField.RegisterValueChangedCallback(evt =>
            { 
                generatedPort.portName = evt.newValue;
                newChoice.choiceName = generatedPort.portName;
            });
            //generatedPort.contentContainer.Add(textField);
            choiceContainer.Add(textField);
        }
        
        /// assign name to port
        //generatedPort.portName = choicePortName;

        // Make choice label hidden because it just displays same text as text field. Disabling label  or making text empty ("") breaks things!
        Label portLabel = generatedPort.contentContainer.Q<Label>(name: "type");
        portLabel.style.visibility = Visibility.Hidden;
        portLabel.style.minWidth = 1;
        portLabel.style.maxWidth = 1;

        // add port to node and refresh visuals
        //_dialogueNode.outputContainer.Add(generatedPort);
        _dialogueNode.choices.Add(newChoice);

        choiceContainer.Add(generatedPort);

        // ----- Add choice container to main container
        boxContainer.Add(choiceContainer);
        
        /// --------------------------------
        /// --------------------------------
        /// GENERATE "ADD CONDITION" BUTTON
        /// --------------------------------
        /// --------------------------------
        {
            Button addConditionButton = new Button(clickEvent: () => AddCondition())
            {
                text = "Add Condition"
            };
            addConditionButton.style.alignContent = Align.FlexStart;
            addConditionButton.style.alignSelf = Align.FlexStart;

            void AddCondition(DialogueChoiceCondition _condition = null)
            {
                Box conditionContainer = new Box(); // container with equation
                conditionContainer.style.flexDirection = FlexDirection.Row;
                conditionContainer.style.marginLeft = 30f;

                DialogueChoiceCondition choiceCondition = new DialogueChoiceCondition()
                {
                    nodeGUID = _dialogueNode.GUID
                };
                if (_condition == null)
                {
                    _condition = choiceCondition;
                }

                // ----- Add button to remove condition
                {
                    Button removeCondButt = new Button(clickEvent: () => RemoveCondition())
                    {
                        text = "X"
                    };
                    void RemoveCondition()
                    {
                        boxContainer.Remove(conditionContainer);
                        newChoice.allChoiceConditions.Remove(choiceCondition);
                        //_dialogueNode.allChoiceConditions.Remove(choiceCondition);
                    }
                    conditionContainer.Add(removeCondButt);
                }
                // ----- Add select variable dropdown
                {
                    //boxContainer.Add(selectVarMenu(choiceCondition));
                    if (_condition.currentVarName != null)
                        conditionContainer.Add(selectVarMenu(_condition, _condition.currentVarName.ToString()));
                    else
                        conditionContainer.Add(selectVarMenu(_condition));
                }
                // ----- Add dropdown to select logic sign for comparison 
                {
                    //boxContainer.Add(selectSignMenu(choiceCondition));
                    if (_condition.logicSign != null)
                        conditionContainer.Add(selectSignMenu(_condition, _condition.logicSign.ToString()));
                    else
                        conditionContainer.Add(selectSignMenu(_condition));
                }
                // ----- Compare with another variable
                {
                    //boxContainer.Add(selectComparedVar(choiceCondition));
                    if (_condition.comparedVarName != null)
                        conditionContainer.Add(selectComparedVar(_condition, _condition.comparedVarName.ToString()));
                    else
                        conditionContainer.Add(selectComparedVar(_condition));
                }
                // ----- Add the box container with everything we need inside to node
                choiceContainer.Add(conditionContainer);

                choiceCondition = _condition;
                newChoice.allChoiceConditions.Add(choiceCondition);
                //Debug.Log(newChoice.allChoiceConditions.Count());

                boxContainer.Add(conditionContainer);
            }
            // ----- Add button that generates conditions on the node
            boxContainer.Add(addConditionButton);

            // ----- Add existing conditions if node is loaded
            
            if (_choice?.allChoiceConditions != null)
            {
                foreach (var condition in _choice.allChoiceConditions)
                {
                    AddCondition(condition);
                }
            }
            


            

        }





        





        _dialogueNode.outputContainer.Add(boxContainer);

        _dialogueNode.RefreshPorts();
        _dialogueNode.RefreshExpandedState();    
    }






    public RedirectionsNode CreateRedirectionsNode
        (Vector2 position, string _overrideGUID = "",         
        List<DialogueChoiceCondition> _loadedRedirections = null)
    {
        // GUID
        string newGUID = Guid.NewGuid().ToString();
        if (!string.IsNullOrEmpty(_overrideGUID))
        {
            newGUID = _overrideGUID;
        }

        var conditionsNode = new RedirectionsNode()
        {
            GUID = newGUID,
        };
        conditionsNode.SetPosition(new Rect(position, defaultNodeSize));

        conditionsNode.styleSheets.Add(styleSheet: Resources.Load<StyleSheet>(path: "StyleSheets/Node"));

        conditionsNode.title = "Redirections";
        conditionsNode.style.maxWidth = 600;
        conditionsNode.style.minWidth = 300;

        
        /// --------------------------------
        /// --------------------------------
        /// GENERATE PORTS (and related stuff)
        /// --------------------------------
        /// --------------------------------
        {
            /// GENERATE INPUT PORT  
            {
                /// add input port
                var inputPort = GeneratePort(conditionsNode, Direction.Input, Port.Capacity.Multi);
                conditionsNode.inputContainer.Add(inputPort);

                /// add button to remove input connections
                Button deleteEdgeButton = new Button(clickEvent: () => DisconnectEdge())
                {
                    text = "X"
                };
                inputPort.contentContainer.Add(deleteEdgeButton);

                void DisconnectEdge()
                {
                    foreach (Edge edge in inputPort.connections)
                    {
                        edge.output.Disconnect(edge);
                        RemoveElement(edge);
                    }
                    inputPort.DisconnectAll();
                }
            }
            ///  GENERATE "ADD REDIRECTION" BUTTON AND RELATED PORTS
            {                    
                Button addRedirectionButt = new Button(clickEvent: () => AddRedirection())
                {
                    text = "Add Redirection"
                };
                void AddRedirection(DialogueChoiceCondition _condition = null)
                {
                    Box boxContainer = new Box(); // container with equation
                    boxContainer.style.flexDirection = FlexDirection.Row;

                    DialogueChoiceCondition redirectionCondition = new DialogueChoiceCondition()
                    {
                        nodeGUID = conditionsNode.GUID
                    };
                    if (_condition == null)
                    {
                        _condition = redirectionCondition;
                    }                    

                    // ----- PORT STUFF
                    var outputPort = GeneratePort(conditionsNode, Direction.Output, Port.Capacity.Single);
                    
                    void DisconnectEdge()
                    {
                        foreach (Edge edge in outputPort.connections)
                        {
                            edge.input.Disconnect(edge);
                            RemoveElement(edge);
                        }
                        outputPort.DisconnectAll();
                    }
                    // ----- Add button to remove comparison (goes in the beginning for better ux)
                    {
                        Button removeCondButt = new Button(clickEvent: () => RemoveRedirection())
                        {
                            text = "X"
                        };
                        void RemoveRedirection()
                        {
                            // just remove the boxcontainer that has all the redirection-related stuff
                            conditionsNode.outputContainer.Remove(boxContainer);
                            conditionsNode.allRedirectionConditions.Remove(_condition);

                            // but don't forget to disconnect the edge!
                            DisconnectEdge();
                        }
                        boxContainer.Add(removeCondButt);
                    }
                    // ----- Add select variable dropdown
                    {
                        if (_condition.currentVarName != null)
                            boxContainer.Add(selectVarMenu(_condition, _condition.currentVarName.ToString()));
                        else
                            boxContainer.Add(selectVarMenu(_condition));
                    }
                    // ----- Add dropdown to select logic sign for comparison 
                    {
                        if (_condition.logicSign != null)
                            boxContainer.Add(selectSignMenu(_condition, _condition.logicSign.ToString()));
                        else
                            boxContainer.Add(selectSignMenu(_condition));
                    }
                    // ----- Add dropdown to select compared variable
                    {
                        if (_condition.comparedVarName != null)
                            boxContainer.Add(selectComparedVar(_condition, _condition.comparedVarName.ToString()));
                        else
                            boxContainer.Add(selectComparedVar(_condition));
                    }
                    // ----- Add port visually
                    {
                        boxContainer.Add(outputPort);

                        // add button to remove connections
                        Button deleteEdgeButton = new Button(clickEvent: () => DisconnectEdge())
                        {
                            text = "X"
                        };
                        outputPort.contentContainer.Add(deleteEdgeButton);
                    }
                    /// ----- Add the box container with everything we need inside to node
                    conditionsNode.outputContainer.Add(boxContainer);
                    /// -----
                    ///
                    redirectionCondition = _condition;
                    conditionsNode.allRedirectionConditions.Add(redirectionCondition);

                    outputPort.portName = redirectionCondition.choiceGUID; // add at least some identificator to a port cause adge tracks em by name and node guid
                }
                // ----- Add button that generates new redirection on the node
                {
                    conditionsNode.outputContainer.Add(addRedirectionButt);
                }
                // ----- Add existing redirections if node is loaded
                if (_loadedRedirections != null)
                {
                    foreach (var redirection in _loadedRedirections)
                    {
                        AddRedirection(redirection);
                    }
                }
            }
        }
        /// --------------------------------
        /// --------------------------------
        /// UPDATE VISUALS ON THE GRAPH
        /// --------------------------------
        /// --------------------------------
        {
            conditionsNode.styleSheets.Add(styleSheet: Resources.Load<StyleSheet>(path: "StyleSheets/Node"));

            conditionsNode.RefreshExpandedState();
            conditionsNode.RefreshPorts();
            //conditionsNode.SetPosition(new Rect(Vector2.zero, Vector2.one));
        }
        /// --------------------------------
        /// --------------------------------
        return conditionsNode;
    }




    ToolbarMenu selectVarMenu(DialogueChoiceCondition condition, string menuText = "")
    {
        ToolbarMenu selectVarMenu = new ToolbarMenu();
        selectVarMenu.style.height = 20;
        selectVarMenu.style.width = 100;
        selectVarMenu.style.alignSelf = Align.Center;
        selectVarMenu.text = menuText;
        //selectVarMenu.style.marginLeft = 5;

        // put every global variable into this list... Cause we might need any of them
        foreach (var entry in storyVariables.varList)
        {
            selectVarMenu.menu.AppendAction(actionName: entry.varName, action: new Action<DropdownMenuAction>(x => selectVariable()));
            void selectVariable()
            {
                selectVarMenu.text = entry.varName;
                condition.addCurrentVariable(entry.varName);
            }
        }
        return selectVarMenu;
        //boxContainer.Add(selectVarMenu);
    }
    ToolbarMenu selectSignMenu(DialogueChoiceCondition condition, string menuText = "")
    {
        ToolbarMenu selectSignMenu = new ToolbarMenu();
        selectSignMenu.style.height = 20;
        selectSignMenu.style.width = 40;
        selectSignMenu.style.alignSelf = Align.Center;
        selectSignMenu.text = menuText;

        List<string> logicSigns = new List<string> { "==", "!=", ">", ">=", "<", "<=" }; // yep, we just list em like that.
        foreach (var sign in logicSigns)
        {
            selectSignMenu.menu.AppendAction(actionName: sign, action: new Action<DropdownMenuAction>(x => selectLogicSign()));
            void selectLogicSign()
            {
                condition.addLogicSign(sign);
                selectSignMenu.text = sign;
            }
        }
        return selectSignMenu;
    }
    ToolbarMenu selectComparedVar(DialogueChoiceCondition condition, string menuText = "")
    {
        ToolbarMenu selectVarMenu = new ToolbarMenu();
        selectVarMenu.style.height = 20;
        selectVarMenu.style.width = 100;
        selectVarMenu.style.alignSelf = Align.Center;
        selectVarMenu.text = menuText;
        //selectVarMenu.style.marginLeft = 5;

        // put every global variable into this list... Cause we might need any of them
        foreach (var entry in storyVariables.varList)
        {
            selectVarMenu.menu.AppendAction(actionName: entry.varName, action: new Action<DropdownMenuAction>(x => selectVariable()));
            void selectVariable()
            {
                selectVarMenu.text = entry.varName;
                //condition.addComparedVariable(entry);
                condition.addComparedVariable(entry.varName);
            }
        }
        return selectVarMenu;
    }






}






/*
// if you need to generate only 1 port in some node, here it goes
var outputPort = GeneratePort(conditionsNode, Direction.Output, Port.Capacity.Single);
outputPort.portName = "Output";
conditionsNode.outputContainer.Add(outputPort);

// hide port name cause we don't need to see it...
Label portLabel = outputPort.contentContainer.Q<Label>(name: "type");
portLabel.style.visibility = Visibility.Hidden;
portLabel.style.minWidth = 1;
portLabel.style.maxWidth = 1;

// add button to remove connections
Button deleteEdgeButton = new Button(clickEvent: () => DisconnectEdge())
{
    text = "X"
};
outputPort.contentContainer.Add(deleteEdgeButton);

void DisconnectEdge()
{
    foreach (Edge edge in outputPort.connections)
    {
        edge.input.Disconnect(edge);
        RemoveElement(edge);
    }
    outputPort.DisconnectAll();
}
*/