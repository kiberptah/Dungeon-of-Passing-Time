using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Behavior : ScriptableObject
{
    AI_BehaviorData initialData;

    public void Initialize(AI_Controller controller)
    {
        controller.behaviorData = initialData;
        controller.behaviorData.controller = controller;

        StateEnter(controller);
    }
    public void BehaviorLoop(AI_Controller controller)
    {
        if (controller.behaviorData.previousStateGUID != controller.behaviorData.currentStateGUID)
        {
            AI_State.EnterState(controller.behaviorData.statesData[controller.behaviorData.currentStateGUID]);
        }
        else
        {
            AI_State.StateLoop(controller.behaviorData.statesData[controller.behaviorData.currentStateGUID]);
        }
    }

    public void StateEnter(AI_Controller controller)
    {
        AI_State.EnterState(controller.behaviorData.statesData[controller.behaviorData.currentStateGUID]);
    }
    public void StateExit(AI_Controller controller)
    {

    }

    public void DecideState(AI_Controller controller)
    {

    }


}
