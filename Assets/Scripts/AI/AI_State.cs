using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class AI_State
{
    public static void EnterState(AI_StateData state)
    {
        GoThroughActions(state, state.onEnterActionGUID);
    }



    public static void StateLoop(AI_StateData state)
    {
        GoThroughActions(state, state.nextLoopActionGUID);
    }



    public static void ExitState(AI_StateData state)
    {
        IncrementAllTimers(state);
        GoThroughActions(state, state.onExitActionGUID);


        GoThroughDecisions(state);
    }








    static void GoThroughActions(AI_StateData stateData, string guid)
    {
        string currentActionGUID = guid;
        while (currentActionGUID != "")
        {
            if (stateData.actionsData.ContainsKey(currentActionGUID))
            {
                stateData.actionsData[currentActionGUID].action.Act(stateData.behavior.controller, stateData, stateData.actionsData[currentActionGUID]);
                currentActionGUID = stateData.actionsData[currentActionGUID]?.nextActionGUID;
            }
            if (stateData.decisionsData.ContainsKey(currentActionGUID))
            {
                stateData.decisionsData[currentActionGUID].decision.Decide(stateData.behavior.controller, stateData.decisionsData[currentActionGUID]);
                currentActionGUID = stateData.decisionsData[currentActionGUID]?.nextActionGUID;
            }
            if (stateData.valueChangersData.ContainsKey(currentActionGUID))
            {
                //stateData.valueChangersData[currentActionGUID].valueChanger.ChangeValue(stateData, stateData.valueChangersData[currentActionGUID]);
                //currentActionGUID = stateData.actionsData[currentActionGUID]?.nextActionGUID;
            }

            // same for decisions !!!
        }
    }


    static void IncrementAllTimers(AI_StateData state)
    {

    }
    static void GoThroughDecisions(AI_StateData state)
    {

    }

    public static void FindValueDataGUID(AI_StateData state, string GUID)
    {
        foreach (var data in state.valuesData)
        {
            
        }
    }



    /*
    public Color debug_Color;

    [Header("===[Actions]===")]
    public AI_SingleAction[] onEnterStateActions;
    public AI_Action[] actions;
    public AI_SingleAction[] onExitStateActions;

    [Header("===[Transitions]===")]
    public AI_Transition[] transitions;


    public void EnterState(AI_Controller controller)
    {
        
        foreach (var action in onEnterStateActions)
        {
            action.Act(controller);
        }
        foreach (var action in actions)
        {
            action.StateEntered(controller);
        }
        
        //controller.myBehavior.
    }

    public void ExitState(AI_Controller controller)
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

    public void UpdateState(AI_Controller controller)
    {
        DoActions(controller);
        CheckTransitions(controller);
    }

    void DoActions(AI_Controller controller)
    {
        foreach (var action in actions)
        {
            action.Act(controller);
        }
    }

    void CheckTransitions(AI_Controller controller)
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
    */
}
