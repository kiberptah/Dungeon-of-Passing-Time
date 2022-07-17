using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "AI/Actions/SlimeAttack")]
public class AI_SlimeAttack : AI_Action
{
    public float attackInterval = 3f;
    public float attackDistance = 2f;


    class ActionData_SlimeAttack : AI_ActionData
    {
        [HideInInspector] public PSlime_Attack slimeAttack;
        [HideInInspector] public float attackIntervalRandomize;
    }
    public override void InitializeWithBehavior(AI_Controller controller, AI_ActionData actionData)
    {
        ActionData_SlimeAttack data = (ActionData_SlimeAttack)actionData;
        data.slimeAttack = controller.actor.GetComponent<PSlime_Attack>();


        actionData = data;
    }

    public override void Act(AI_Controller controller, AI_StateData stateData, AI_ActionData actionData)
    {
        /*
        if (Vector3.Distance(controller.actor.transform.position, controller.currentTarget.position) <= attackDistance)
        {
            ActionData_SlimeAttack data = (ActionData_SlimeAttack)actionData;
            data.slimeAttack.StartAttack(controller.currentTarget);
        }
        */
        ActionData_SlimeAttack data = (ActionData_SlimeAttack)actionData;
        data.slimeAttack.StartAttack(controller.currentTarget);
    }
    





}
