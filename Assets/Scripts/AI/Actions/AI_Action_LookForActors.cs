using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "AI/Actions/LookForActors")]
public class AI_Action_LookForActors : AI_Action
{
    public float lookAroundInterval = 1f; // optimization!
    class ActionData_Look : AI_ActionData
    {
        public float lookAroundTimer = 0;
    }

    #region Default
    public override void StateEntered(AI_StateController controller)
    {
        controller.actionData.Add(this.name, new ActionData_Look());
    }
    public override void StateExited(AI_StateController controller)
    {
        controller.actionData.Remove(this.name);
    }
    public override void Act(AI_StateController controller)
    {
        LookAround(controller);
        Timers(controller);
    }
    #endregion

    void Timers(AI_StateController controller)
    {
        ActionData_Look data = (ActionData_Look)controller.actionData[this.name];

        data.lookAroundTimer += Time.deltaTime;

        controller.actionData[this.name] = data;
    }

    void LookAround(AI_StateController controller)
    {
        ActionData_Look data = (ActionData_Look)controller.actionData[this.name];

        if (data.lookAroundTimer > lookAroundInterval)
        {
            data.lookAroundTimer = 0;

            controller.sightedObjects = LookForTheObjectsAround(controller);
        }

        controller.actionData[this.name] = data;


    }
    #region Lookin methods
    List<Transform> LookForTheObjectsAround(AI_StateController controller)
    {
        List<Transform> _sightedObjects = new List<Transform>();

        Collider2D[] sightedColliders;
        //sightedColliders = Physics2D.OverlapCircleAll(actor.transform.position, actorStats.sightRadius).Cast<Collider2D>().ToList<Collider2D>();
        sightedColliders = Physics2D.OverlapCircleAll(controller.actor.transform.position, controller.actorStats.sightRadius);


        foreach (Collider2D obj in sightedColliders)
        {

            if (obj.tag == "Actor") //|| obj.tag == "Geometry")
            {
                if (EyeContactWithTarget(controller, obj.transform))
                {
                    //Debug.Log("i see " + obj.name);

                    _sightedObjects.Add(obj.transform);

                    //Debug.Log("i added to list " + obj.name);

                }
            }

        }

        return _sightedObjects;
    }


    public bool EyeContactWithTarget(AI_StateController controller, Transform target)
    {
        if (target != null)
        {
            float distanceToTarget = Vector2.Distance(target.position, controller.actor.position);

            RaycastHit2D[] raycast =
                Physics2D.RaycastAll(
                    origin: controller.actor.transform.position,
                    direction: controller.actor.transform.TransformDirection(target.position - controller.actor.transform.position),
                    distance: controller.actorStats.sightRadius,
                    layerMask: LayerMask.GetMask("Actor", "Geometry")
                    );

            foreach (var potentialObstacle in raycast)
            {
                if (potentialObstacle.transform.gameObject.tag == "Geometry")
                {
                    if (potentialObstacle.distance < distanceToTarget)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion

}
