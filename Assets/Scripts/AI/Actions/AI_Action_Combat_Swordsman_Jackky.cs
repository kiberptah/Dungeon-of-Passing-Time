using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "AI/Actions/Combat_Swordsman_Jackky")]
public class AI_Action_Combat_Swordsman_Jackky : AI_Action
{
    float healthPercentToChangeToSwing = 0.33f;
    float swingingMinRange = 5f;
    class ActionData_Combat_Swordsman_Jackky : AI_ActionData
    {
        public int swingDirection = 0;

        public float swingDirInterval = 4f;
        public float swingDirTimer = 0;
    }

    #region Default Methods
    public override void StateEntered(AI_StateController controller)
    {
        controller.actionData.Add(this.name, new ActionData_Combat_Swordsman_Jackky());
    }
    public override void StateExited(AI_StateController controller)
    {
        controller.actionData.Remove(this.name);
    }
    public override void Act(AI_StateController controller)
    {
        if (controller.actorHealth.CurrentHealth > controller.actorHealth.MaxHealth * healthPercentToChangeToSwing
        || Vector3.Distance(controller.actor.position, controller.currentTarget.position) > swingingMinRange)
        {
            TryToPierce(controller);
        }
        else
        {
            SwingCloseToEnemy(controller);
        }

        Timers(controller);
    }
    #endregion
    void Timers(AI_StateController controller)
    {
        ActionData_Combat_Swordsman_Jackky data = (ActionData_Combat_Swordsman_Jackky)controller.actionData[this.name];

        data.swingDirTimer += Time.deltaTime;

        controller.actionData[this.name] = data;
    }
    void TryToPierce(AI_StateController controller)
    {
        ActionData_Combat_Swordsman_Jackky data = (ActionData_Combat_Swordsman_Jackky)controller.actionData[this.name];

        float attackAngle = 10f;

        float maxAngle = 1f;
        float minAngle = .5f;


        Vector3 targetProjection = controller.actor.InverseTransformPoint(controller.currentTarget.position).normalized;
        //Vector3 bladeProjection = controller.actor.InverseTransformPoint(controller.input.weaponObject.transform.position).normalized;
        Vector3 bladeProjection = controller.actor.InverseTransformPoint(controller.input.weaponObject.transform.position.OverrideY(0f)).normalized;

        float angleBetweenBladeAndEnemy = Vector3.SignedAngle(bladeProjection, targetProjection, controller.transform.up);
        //Debug.Log("angleBetweenBladeAndEnemy " + angleBetweenBladeAndEnemy);
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


        controller.actionData[this.name] = data;
    }

    void SwingCloseToEnemy(AI_StateController controller)
    {
        ActionData_Combat_Swordsman_Jackky data = (ActionData_Combat_Swordsman_Jackky)controller.actionData[this.name];

        if (data.swingDirection == 0)
        {
            data.swingDirection = 1;
        }


        if (data.swingDirTimer > data.swingDirInterval)
        {
            data.swingDirection = -data.swingDirection;
            data.swingDirTimer = 0;
        }

        controller.input.Input_WeaponSwing(data.swingDirection);

        controller.actionData[this.name] = data;
    }



}
