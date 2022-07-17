using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "AI/Actions/SheathWeapon")]
public class AI_SingleAction_SheathWeapon : AI_Action
{
    public override void InitializeWithBehavior(AI_Controller controller, AI_ActionData actionData)
    {

    }

    public override void Act(AI_Controller controller, AI_StateData stateData, AI_ActionData actionData)
    {
        SheathWeapon(controller);
    }

    void SheathWeapon(AI_Controller controller)
    {
        controller.input.Input_SheathWeapon();
    }
}
