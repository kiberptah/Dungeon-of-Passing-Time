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
        [HideInInspector] public float attackTimer;
        [HideInInspector] public float attackIntervalRandomize;
    }

    public override void StateEntered(AI_StateController controller)
    {
        controller.actionData.Add(this.name, new ActionData_SlimeAttack());

        ActionData_SlimeAttack data = (ActionData_SlimeAttack)controller.actionData[this.name];
        /// ---------------------------------------------------------------------------------------------------------
        data.slimeAttack = controller.actor.GetComponent<PSlime_Attack>();
        data.attackTimer = attackInterval; // to enable attack from the start
        data.attackIntervalRandomize = Random.Range(1, 1.5f);
        /// ---------------------------------------------------------------------------------------------------------
        controller.actionData[this.name] = data;
    }
    public override void Act(AI_StateController controller)
    {
        Attack(controller);
        Timer(controller);
    }
    public override void StateExited(AI_StateController controller)
    {
        controller.actionData.Remove(this.name);
    }


    void Attack(AI_StateController controller)
    {
        ActionData_SlimeAttack data = (ActionData_SlimeAttack)controller.actionData[this.name];
        /// ---------------------------------------------------------------------------------------------------------

        if (data.attackTimer > attackInterval * data.attackIntervalRandomize)
        {
            if (Vector3.Distance(controller.actor.transform.position, controller.currentTarget.position) <= attackDistance)
            {
                data.attackTimer = 0;

                data.slimeAttack.StartAttack(controller.currentTarget);
            }
        }





        /// ---------------------------------------------------------------------------------------------------------
        controller.actionData[this.name] = data;
    }
    void Timer(AI_StateController controller)
    {
        ActionData_SlimeAttack data = (ActionData_SlimeAttack)controller.actionData[this.name];
        /// ---------------------------------------------------------------------------------------------------------
        data.attackTimer += Time.deltaTime;
       

        /// ---------------------------------------------------------------------------------------------------------
        controller.actionData[this.name] = data;
    }
}
