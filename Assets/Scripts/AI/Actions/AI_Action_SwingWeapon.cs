using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName = "AI/Actions/Swing Weapon")]
public class AI_Action_SwingWeapon : AI_Action
{
    void OnValidate()
    { 
        dynamicValues.intValues.Add("swingDirection", 1);
        //dynamicValues.boolValues.Add("testBool", true);
        //dynamicValues.floatValues.Add("testFloat", 1);
        //Debug.Log("validated");
    }

    class ActionData_SwingWeapon : AI_ActionData
    {
        public int swingDirection = 0;
    }

    void UpdateValues(AI_StateData stateData, ActionData_SwingWeapon actionData)
    {
        foreach (var value in actionData.dynamicValues.intValues)
        {
            actionData.dynamicValues.intValues[value.Key] = stateData.valuesData[value.Key].intValue;
        }
    }
    public override void InitializeWithBehavior(AI_Controller controller, AI_ActionData actionData)
    {
        ActionData_SwingWeapon data = (ActionData_SwingWeapon)actionData;
        actionData = data;
    }

    public override void Act(AI_Controller controller, AI_StateData stateData, AI_ActionData actionData)
    {
        //ActionData_SwingWeapon data = (ActionData_SwingWeapon)actionData;
        UpdateValues(stateData, (ActionData_SwingWeapon)actionData);



        if (((ActionData_SwingWeapon)actionData).swingDirection == 0)
        {
            ((ActionData_SwingWeapon)actionData).swingDirection = 1;
        }
        controller.input.Input_WeaponSwing(((ActionData_SwingWeapon)actionData).swingDirection);




        //actionData = data;
    }
}
