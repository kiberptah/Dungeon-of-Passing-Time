using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class DialogueTrigger : MonoBehaviour, IInteractable
{
    //public DialogueManager dialogueManager;
    [SerializeField] string text = "Talk";
    public DialogueContainer dialogueContainer;

    Dialogue dialogue;

    private void Start()
    {
        dialogue = new Dialogue(dialogueContainer);

    }
    public void OnHoverStart(InteractionUI interactionUI, Transform _interactor)
    {
        interactionUI.Activate(text);
    }
    public void OnInteract(Transform interactor)
    {
        TriggerDialogue();
    }
    public void OnHoverEnd(InteractionUI interactionUI, Transform _interactor)
    {
        interactionUI.Deactivate();
    }
    void TriggerDialogue()
    {
        //dialogueManager.StartDialogue(dialogue);


        EventDirector.dialogue_start?.Invoke();
        EventDirector.dialogue?.Invoke(dialogue);
    }

}
