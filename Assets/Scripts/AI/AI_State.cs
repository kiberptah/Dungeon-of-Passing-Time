using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "AI/State")]
public class AI_State : ScriptableObject
{
    public Color debug_Color;

    [Header("===[Actions]===")]
    public AI_SingleAction[] onEnterStateActions;
    public AI_Action[] actions;
    public AI_SingleAction[] onExitStateActions;

    [Header("===[Transitions]===")]
    public AI_Transition[] transitions;


    public void EnterState(AI_StateController controller)
    {
        foreach (var action in onEnterStateActions)
        {
            action.Act(controller);
        }
        foreach (var action in actions)
        {
            action.StateEntered(controller);
        }
    }

    public void ExitState(AI_StateController controller)
    {
        foreach (var action in onExitStateActions)
        {
            action.Act(controller);
        }
        foreach (var action in actions)
        {
            action.StateExited(controller);
        }
    }

    public void UpdateState(AI_StateController controller)
    {
        DoActions(controller);
        CheckTransitions(controller);
    }

    void DoActions(AI_StateController controller)
    {
        foreach (var action in actions)
        {
            action.Act(controller);
        }
    }

    void CheckTransitions(AI_StateController controller)
    {
        foreach (var transition in transitions)
        {
            bool decisionSucceeded = transition.decision.Decide(controller);

            if (decisionSucceeded)
            {
                controller.TransitionToState(transition.trueState);
                if (transition.trueState != controller.currentState)
                {
                    break;
                }
            }
            else
            {
                controller.TransitionToState(transition.falseState);
                if (transition.trueState != controller.currentState)
                {
                    break;
                }
            }
        }
    }
}
