using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "AI/Actions/LookForActors")]
public class AI_Action_LookForActors : AI_Action
{
    #region Default
    public override void InitializeWithBehavior(AI_Controller controller, AI_ActionData actionData)
    {

    }
    public override void Act(AI_Controller controller, AI_StateData stateData, AI_ActionData actionData)
    {
        LookAround(controller);
    }
    #endregion


    void LookAround(AI_Controller controller)
    {

        controller.sightedObjects = LookForTheObjectsAround(controller);

    }
    #region Lookin methods
    List<Transform> LookForTheObjectsAround(AI_Controller controller)
    {
        List<Transform> _sightedObjects = new List<Transform>();

        Collider[] sightedColliders;
        sightedColliders = Physics.OverlapSphere(controller.actor.transform.position, controller.actorStats.sightRadius);


        foreach (Collider obj in sightedColliders)
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


    public bool EyeContactWithTarget(AI_Controller controller, Transform target)
    {
        if (target != null)
        {
            float distanceToTarget = Vector3.Distance(target.position, controller.actor.position);

            RaycastHit[] raycast =
                Physics.RaycastAll(
                    origin: controller.actor.transform.position,
                    direction: controller.actor.transform.TransformDirection(target.position - controller.actor.transform.position),
                    maxDistance: controller.actorStats.sightRadius,
                    layerMask: LayerMask.GetMask("Geometry")
                    );

            foreach (var potentialObstacle in raycast)
            {
                //Debug.Log("i see: " + potentialObstacle.transform.name + " in front of the ");
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
