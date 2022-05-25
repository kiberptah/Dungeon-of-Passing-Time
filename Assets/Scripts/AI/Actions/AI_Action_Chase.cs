using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using System.Linq;

[CreateAssetMenu(menuName = "AI/Actions/Chase")]
public class AI_Action_Chase : AI_Action
{
    [Range(0, 10)]
    public float minDistance = 2f;
    public float chaseOffsetDistance = 0f;

    class ActionData_Chase : AI_ActionData
    {
        [HideInInspector] public List<Vector3> path = new List<Vector3>();
        [HideInInspector] public Vector3 destination;


        public Vector3 chaseOffset = Vector2.zero;
        public float destinationOffsetInterval = 5f * Random.Range(0.8f, 1f);
        public float destinationOffseetTimer = 0;
    }

    #region Default methods
    public override void StateEntered(AI_StateController controller)
    {
        controller.actionData.Add(this.name, new ActionData_Chase());
    }
    public override void StateExited(AI_StateController controller)
    {
        controller.actionData.Remove(this.name);
    }


    public override void Act(AI_StateController controller)
    {
        if (Vector2.Distance(controller.actor.position, controller.currentTarget.position) > minDistance)
        {
            Chase(controller);
        }
        Timers(controller);
    }
    #endregion

    void Chase(AI_StateController controller)
    {
        ActionData_Chase data = (ActionData_Chase)controller.actionData[this.name];

        #region Pathfinding
        Vector3 goHere = controller.currentTarget.position;
        if (data.destinationOffseetTimer > data.destinationOffsetInterval)
        {
            data.destinationOffseetTimer = 0;

            if (chaseOffsetDistance > 0)
            {
                data.chaseOffset = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                data.chaseOffset = data.chaseOffset.normalized * chaseOffsetDistance;
            }
        }
        goHere += data.chaseOffset;

        PathRequestManager.RequestPath(controller.actor.position, goHere, OnPathFound);




        void OnPathFound(Vector3[] newPath, bool pathSuccessful)
        {
            if (pathSuccessful)
            {
                data.path = newPath.Cast<Vector3>().ToList();
                data.destination = data.path[0];
            }
            else
            {
                data.path.Clear();

                // To try another offset next frame
                data.destinationOffseetTimer = data.destinationOffsetInterval;
            }
        }
        #endregion

        #region Going places
        if (data.path.Count > 0)
        {
            controller.input.Input_Movement(data.destination - controller.actor.position);


            // Remove waypoints after reaching them, put this in State controller also in wonder action 
            float minDistanceToDest = 0.1f;
            if (Vector3.Distance(controller.actor.position, data.destination) < minDistanceToDest)
            {
                data.path.RemoveAt(0);
                if (data.path.Count != 0)
                {
                    data.destination = data.path[0];
                }
            }
        }
        #endregion



        controller.actionData[this.name] = data;
    }



    void Timers(AI_StateController controller)
    {
        ActionData_Chase data = (ActionData_Chase)controller.actionData[this.name];
        data.destinationOffseetTimer += Time.deltaTime;

        controller.actionData[this.name] = data;
    }
}
