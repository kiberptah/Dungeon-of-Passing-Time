using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "AI/Actions/Pierce with Weapon")]

public class AI_Action_PierceWithWeapon : AI_Action
{
    class ActionData_PierceWithWeapon : AI_ActionData
    {
        public int swingDirection = 0;
    }
    public override void InitializeWithBehavior(AI_Controller controller, AI_ActionData actionData)
    {
        ActionData_PierceWithWeapon data = (ActionData_PierceWithWeapon)actionData;
        actionData = data;
    }


    public override void Act(AI_Controller controller, AI_StateData stateData, AI_ActionData actionData)
    {
        ActionData_PierceWithWeapon data = (ActionData_PierceWithWeapon)actionData;


        float attackAngle = 10f;

        float maxAngle = .5f;
        float minAngle = .25f;


        Vector3 targetProjection = controller.actor.InverseTransformPoint(controller.currentTarget.position).normalized;
        Vector3 bladeProjection = controller.actor.InverseTransformPoint(controller.input.weaponObject.transform.position.OverrideY(0f)).normalized;

        float angleBetweenBladeAndEnemy = Vector3.SignedAngle(bladeProjection, targetProjection, controller.transform.up);
        if (angleBetweenBladeAndEnemy > maxAngle)
        {
            data.swingDirection = 1;
        }
        if (angleBetweenBladeAndEnemy < -maxAngle)
        {
            data.swingDirection = -1;
        }
        if (Mathf.Abs(angleBetweenBladeAndEnemy) <= minAngle)
        {
            data.swingDirection = 0;
        }
        controller.input.Input_WeaponSwing(data.swingDirection);


        if (Mathf.Abs(angleBetweenBladeAndEnemy) <= attackAngle)
        {
            if (Vector3.Distance(controller.actor.position, controller.currentTarget.position) < 3f)
            {
                controller.input.Input_WeaponPierce();
            }
        }


        actionData = data;

    }
}
