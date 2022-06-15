using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "AI/Actions/Example")]
public class AI_ExampleAction : AI_Action
{
    class ActionData_ExampleAction : AI_ActionData
    {
        [HideInInspector] public float exampleVar;
    }

    public override void StateEntered(AI_StateController controller)
    {
        controller.actionData.Add(this.name, new ActionData_ExampleAction());
    }
    public override void Act(AI_StateController controller)
    {
        Something(controller);
    }
    public override void StateExited(AI_StateController controller)
    {
        controller.actionData.Remove(this.name);
    }


    void Something(AI_StateController controller)
    {
        ActionData_ExampleAction data = (ActionData_ExampleAction)controller.actionData[this.name];
        /// ---------------------------------------------------------------------------------------------------------






        /// ---------------------------------------------------------------------------------------------------------
        controller.actionData[this.name] = data;
    }
}
