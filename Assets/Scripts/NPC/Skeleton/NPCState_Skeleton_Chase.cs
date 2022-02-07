using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NPCState_Skeleton_Chase : INPCState
{
    NPC_AI npc;
    Transform actor;
    Vector3[] path;
    int targetWaypointIndex = 0;

    public void ChangeToThisState(NPC_AI _npc)
    {
        npc = _npc;
        actor = npc.actor;

        npc.npcController.actor.Input_SheathWeapon();

        if (npc.currentTarget != null)
        {
            PathRequestManager.RequestPath(actor.position, npc.lastKnownTargetPosition, OnPathFound);
        }
    }
    public INPCState DoState(NPC_AI _npc)
    {

        PathRequestManager.RequestPath(actor.position, npc.lastKnownTargetPosition, OnPathFound);
        Chase();
        npc.testpath = path;
        npc.testtargetWaypointIndex = targetWaypointIndex;

        if (npc.currentTarget != null)
        {
            return npc.state_skeleton_combat;
        }

        if (Vector2.Distance(actor.position, npc.lastKnownTargetPosition) < 0.01f)
        {
            return npc.state_skeleton_idle;
        }

        return npc.state_skeleton_chase;
    }

    void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            //Debug.Log("OnPathFound");
            path = newPath;
        }
    }

    void Chase()
    {
        if (path != null && path.Length > 0 && targetWaypointIndex < path.Length)
        {
            Vector3 currentWaypoint = path[targetWaypointIndex];
            if (actor.position == currentWaypoint)
            {
                targetWaypointIndex++;
            }

            npc.testNextNode = currentWaypoint;

            npc.npcController.Input_Movement(currentWaypoint - actor.position);
            //actor.position = Vector3.MoveTowards(actor.position, currentWaypoint, 5 * Time.deltaTime);

        }
    }

}