using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/DrawWeapon")]
public class AI_SingleAction_DrawWeapon : AI_SingleAction
{
    public override void Act(AI_StateController controller)
    {
        DrawWeapon(controller);
    }

    void DrawWeapon(AI_StateController controller)
    {
        controller.input.Input_DrawWeapon();
        //Debug.Log("DRAW");
    }

}
