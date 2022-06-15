using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


[CreateAssetMenu(menuName = "AI/Actions/Wander")]
public class AI_Action_Wander : AI_Action
{
    class ActionData_Wander : AI_ActionData
    {
        [HideInInspector] public List<Vector3> path = new List<Vector3>();
        [HideInInspector] public Vector3 destination;
    }

    #region Default
    public override void StateEntered(AI_StateController controller)
    {
        controller.actionData.Add(this.name, new ActionData_Wander());
    }
    public override void StateExited(AI_StateController controller)
    {
        controller.actionData.Remove(this.name);
    }
    public override void Act(AI_StateController controller)
    {
        Wonder(controller);
    }
    #endregion

    void Wonder(AI_StateController controller)
    {
        ActionData_Wander data = (ActionData_Wander)controller.actionData[this.name];

        if (data.path.Count() == 0)
        {
            float offsetX = Random.Range(-1f, 1f);
            float offsetZ = Random.Range(-1f, 1f);
            Vector3 randomCoordinateNearby = controller.actor.position + new Vector3(offsetX, 0, offsetZ);
            PathRequestManager.RequestPath(controller.actor.position, randomCoordinateNearby, OnPathFound);
        }
        else
        {
            controller.input.Input_Movement((data.destination - controller.actor.position).normalized);

            float minDistanceToDest = 0.1f;
            if (Vector3.Distance(controller.actor.position, data.destination) < minDistanceToDest)
            {
                data.path.RemoveAt(0);
                if (data.path.Count() != 0)
                {
                    data.destination = data.path[0];
                }
            }
        }

        void OnPathFound(Vector3[] newPath, bool pathSuccessful)
        {
            if (pathSuccessful)
            {
                data.path = newPath.Cast<Vector3>().ToList();
                data.destination = data.path[0];

                controller.debug_path = data.path;
                controller.debug_destination = data.destination;
            }
        }
        controller.actionData[this.name] = data;

    }

    

}
