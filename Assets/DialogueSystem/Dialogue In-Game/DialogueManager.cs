using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using System.Linq;

using DialogueSystem;

public class DialogueManager : MonoBehaviour
{
    StoryEventsContainer storyEvents;// = Resources.Load<StoryEvents>(path: StoryEvents.storyEventsPath);

    public GameObject dialogueUIBox;
    public Image speakerPortrait;
    public TMP_Text nameText;
    public TMP_Text textBox;
    public TMP_Text choices;

    Dialogue dialogue;

    class Choice
    {
        public string name;
        public string targetNodeGUID;

        public Choice(string _name, string _nextNodeGUID)
        {
            name = _name;
            targetNodeGUID = _nextNodeGUID;
        }
    }
    List<Choice> choicesList = new List<Choice>();
    private int highlightedChoiceNumber = 0;

    public enum lang
    {
        EN,
        RU
    }
    public static lang selectedLang = lang.EN;

    private void Awake()
    {
        storyEvents = Resources.Load<StoryEventsContainer>(path: StoryEventsContainer.storyEventsPath);
    }

    private void OnEnable()
    {
        EventDirector.dialogue += StartDialogue;
    }
    private void OnDisable()
    {
        EventDirector.dialogue -= StartDialogue;
    }
    private void Start()
    {
        dialogueUIBox.SetActive(false); //closes dialogue UI on play
        //CustomRuntimeTools.DebugDump("test");
    }
    private void Update()
    {
        if(dialogueUIBox.activeSelf == true)
        {
            ChoiceSelection();
        }
        
        //UpdateChoices();
    }
    public void StartDialogue(Dialogue _dialogue)
    {
        if (dialogue != _dialogue)
        {
            dialogueUIBox.SetActive(true);

            dialogue = _dialogue;

            AdvanceDialogue(dialogue.dialogueContainer.entryNodeData.guid);
        }
    }

    public void AdvanceDialogue(string _currentNodeGUID)
    {
        dialogue.currentNodeGUID = _currentNodeGUID;
        dialogue.currentNodeType = dialogue.FindNodeTypeByGUID(_currentNodeGUID);

        switch (dialogue.currentNodeType)
        {
            // ----- Entry Node
            case Dialogue.nodeType.entry:
                {
                    EdgesData edge = dialogue.dialogueContainer.edgesData.Where(e => e.outputNodeGuid == dialogue.currentNodeGUID).FirstOrDefault();

                    dialogue.nextNodeGUID = edge.targetNodeGuid;

                    AdvanceDialogue(dialogue.nextNodeGUID);
                    break;
                }
            // ----- Dialogue Node
            case Dialogue.nodeType.dialogue:
                {
                    // ----- Determine page number in list
                    {
                        int i = 0;
                        foreach (var page in dialogue.dialoguePages)
                        {
                            if (page.pageGUID == dialogue.currentNodeGUID)
                            {
                                dialogue.currentPageNumber = i;
                            }
                            ++i;
                        }
                    }

                    // ----- Assign Portrait
                    {
                        foreach (var entry in dialogue.dialogueContainer.dialogueNodesData)
                        {
                            if (entry.guid == dialogue.currentNodeGUID)
                            {
                                // Keep the previous portrait if it doesn't change so it is easier to make dialogues in editor!
                                if (entry.speakerPortrait != null) 
                                {
                                    speakerPortrait.sprite = entry.speakerPortrait;
                                }                           
                            }
                        }
                    }

                    // ----- Assign name and text
                    // this data is taken not from dialoguecontainer but from pages for easier localization support
                    {
                        // Keep the previous name if it doesn't change so it is easier to make dialogues in editor!
                        if ( !string.IsNullOrEmpty(dialogue.dialoguePages[dialogue.currentPageNumber].speakerName) )
                        {
                            nameText.text = dialogue.dialoguePages[dialogue.currentPageNumber].speakerName;
                        }                       
                        textBox.text = dialogue.dialoguePages[dialogue.currentPageNumber].pageText;
                    }

                    // ----- Assign choices
                    {

                        DialogueNodeData dialogueNodeData = dialogue.dialogueContainer.dialogueNodesData.Where(nodeData => nodeData.guid == _currentNodeGUID).First();
                        foreach (var entry in dialogue.dialogueContainer.dialogueNodesData)
                        {
                            if (entry.guid == _currentNodeGUID)
                            {
                                dialogueNodeData = entry;
                            }
                        }
                        int choiceNumberInList = 0;
                        foreach (var choice in dialogueNodeData.choices)
                        {
                            bool isChoiceAccessible = true;
                            foreach (var condition in choice.allChoiceConditions)
                            {
                                /*
                                Debug.Log(condition.currentVarName);
                                Debug.Log(condition.logicSign);
                                Debug.Log(condition.comparedVarName);
                                */
                                if (condition.CheckCondition() == false)
                                {
                                    isChoiceAccessible = false;
                                    break;
                                }
                            }
                            if (isChoiceAccessible)
                            {
                                EdgesData edge = dialogue.dialogueContainer.edgesData.Where(
                                    e => e.outputNodeGuid == dialogue.currentNodeGUID && e.outputPortName == choice.choiceName).FirstOrDefault();

                                if (edge != null)
                                {
                                    string targetNodeGUID = edge.targetNodeGuid;

                                    //choicesList.Add(new Choice(choice.choiceName, targetNodeGUID));

                                    // this data is taken not from dialoguecontainer but from pages for easier localization support
                                    choicesList.Add(new Choice(dialogue.dialoguePages[dialogue.currentPageNumber].choices[choiceNumberInList], targetNodeGUID));
                                }

                            }
                            ++choiceNumberInList;
                        }
                        UpdateChoices();
                    }

                    break;
                }
            case Dialogue.nodeType.redirections:
                {
                    RedirectionsNodeData redirectionsNodeData;
                    redirectionsNodeData = dialogue.dialogueContainer.redirectionsNodesData.Where(x => x.guid == dialogue.currentNodeGUID).FirstOrDefault();
                    foreach (var redirection in redirectionsNodeData.redirections)
                    {
                        if (redirection.CheckCondition() == true)
                        {
                            EdgesData edge = dialogue.dialogueContainer.edgesData.Where(
                                e => e.outputNodeGuid == dialogue.currentNodeGUID && e.outputPortName == redirection.choiceGUID).FirstOrDefault();

                            if (edge != null)
                            {
                                string targetNodeGUID = edge.targetNodeGuid;
                                dialogue.nextNodeGUID = targetNodeGUID;
                                AdvanceDialogue(dialogue.nextNodeGUID);
                                break;
                            }
                        }
                    }
                    break;
                }
            case Dialogue.nodeType.events:
                {
                    // fire events
                    EventTriggerNodeData eventNodeData;
                    eventNodeData = dialogue.dialogueContainer.eventNodesData.Where(x => x.guid == dialogue.currentNodeGUID).FirstOrDefault();
                    foreach (var entry in eventNodeData.dialogueEvents)
                    {
                        //Debug.Log("here event should be fired:");
                        StoryEventsContainer.FireEvent(storyEvents.eventsList, entry.eventName);
                    }
                    // Find next node guid
                    {
                        EdgesData edge = dialogue.dialogueContainer.edgesData.Where(
                                    e => e.outputNodeGuid == dialogue.currentNodeGUID).FirstOrDefault();

                        if (edge != null)
                        {
                            string targetNodeGUID = edge.targetNodeGuid;
                            dialogue.nextNodeGUID = targetNodeGUID;
                            AdvanceDialogue(dialogue.nextNodeGUID);
                        }
                    }
                    break;
                }
            case Dialogue.nodeType.end:
                {
                    EndDialogue();
                    break;
                }
            case Dialogue.nodeType.unknown:
                {
                    EndDialogue();
                    break;
                }
        }

        
        
    }

    void EndDialogue()
    {
        textBox.text = "";
        nameText.text = "";
        //dialogue.dialoguePages.Clear();
        dialogue = null;
        dialogueUIBox.SetActive(false);

        EventDirector.dialogue_end?.Invoke();
    }


    void ChoiceSelection()
    {      
        if (Input.GetButtonDown("NextUISelection"))
        {
            highlightedChoiceNumber = Mathf.Clamp(++highlightedChoiceNumber, 0, choicesList.Count - 1);
            UpdateChoices();
        }
        if (Input.GetButtonDown("PreviousUISelection"))
        {
            highlightedChoiceNumber = Mathf.Clamp(--highlightedChoiceNumber, 0, choicesList.Count - 1);
            UpdateChoices();
        }

        

        
        if (Input.GetButtonDown("SubmitUISelection"))
        {
            if (choicesList.Count != 0)
            {
                string nextGUID = choicesList[highlightedChoiceNumber].targetNodeGUID;
                choicesList.Clear();

                AdvanceDialogue(nextGUID);
            }
            else
            {
                EndDialogue();
            }
        }    
    }
    void UpdateChoices()
    {
        string openingTag = "";
        string closingTag = "";
        choices.text = "";

        int i = 0;
        foreach (Choice choice in choicesList)
        {
            if (i == highlightedChoiceNumber)
            {
                openingTag = "<color=yellow>";
                closingTag = "</color>";
            }
            else
            {
                openingTag = "";
                closingTag = "";
            }

            choices.text += openingTag + choice.name + closingTag + '\n';


            i++;
        }
    }
}
