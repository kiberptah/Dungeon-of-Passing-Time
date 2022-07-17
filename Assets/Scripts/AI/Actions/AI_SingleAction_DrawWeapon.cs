using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/DrawWeapon")]
public class AI_SingleAction_DrawWeapon : AI_Action
{

    public override void InitializeWithBehavior(AI_Controller controller, AI_ActionData actionData)
    {

    }

    public override void Act(AI_Controller controller, AI_StateData stateData, AI_ActionData actionData)
    {
        DrawWeapon(controller);
    }

    void DrawWeapon(AI_Controller controller)
    {
        controller.input.Input_DrawWeapon();
        //Debug.Log("DRAW");
    }

}
