using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "AI/Actions/SheathWeapon")]
public class AI_SingleAction_SheathWeapon : AI_SingleAction
{
    public override void Act(AI_StateController controller)
    {
        SheathWeapon(controller);
    }

    void SheathWeapon(AI_StateController controller)
    {
        controller.input.Input_SheathWeapon();
    }
}
