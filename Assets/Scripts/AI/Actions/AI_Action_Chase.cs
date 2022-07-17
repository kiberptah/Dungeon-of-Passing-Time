using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using System.Linq;

[CreateAssetMenu(menuName = "AI/Actions/Chase")]
public class AI_Action_Chase : AI_Action
{
    [Range(0, 10)]
    public float minDistance = 1.5f;
    public float chaseOffsetDistance = 0f;

    class ActionData_Chase : AI_ActionData
    {
        [HideInInspector] public List<Vector3> path = new List<Vector3>();
        [HideInInspector] public Vector3 destination;


        public Vector3 chaseOffset = Vector3.zero;
        public float destinationOffsetInterval = 5f * Random.Range(0.8f, 1f);
        public float destinationOffseetTimer = 0;
    }



    public override void InitializeWithBehavior(AI_Controller controller, AI_ActionData actionData)
    {
        //throw new System.NotImplementedException();
    }

    public override void Act(AI_Controller controller, AI_StateData stateData, AI_ActionData actionData)
    {
        if (Vector3.Distance(controller.actor.position, controller.currentTarget.position) > minDistance)
        {
            Chase(controller, actionData);
        }
    }

    void Chase(AI_Controller controller, AI_ActionData actionData)
    {
        ActionData_Chase data = (ActionData_Chase)actionData;

        #region Pathfinding
        Vector3 goHere = controller.currentTarget.position;
        
        if (data.destinationOffseetTimer > data.destinationOffsetInterval)
        {
            data.destinationOffseetTimer = 0;

            if (chaseOffsetDistance > 0)
            {
                data.chaseOffset = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
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

                controller.debug_path = data.path;
                controller.debug_destination = data.destination;
            }
            else
            {
                data.path.Clear();

                // To try another offset next frame
                data.destinationOffseetTimer = data.destinationOffsetInterval;


                controller.debug_path = data.path;
                controller.debug_destination = data.destination;
            }
        }
        #endregion

        #region Going places
        if (data.path.Count > 0)
        {
            controller.input.Input_Movement(data.destination - controller.actor.position);


            // Remove waypoints after reaching them
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



        actionData = data;
    }



}
