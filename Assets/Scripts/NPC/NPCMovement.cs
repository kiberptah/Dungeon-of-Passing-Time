using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    ActorStats npcStats;
    NPCStateMachine npc;

    Vector3[] path;
    int targetIndex;

    Vector3 destination;

    private void Awake()
    {
        npcStats = GetComponent<ActorStats>();
        npc = GetComponent<NPCStateMachine>();
    }
    private void Start()
    {
        
    }
    public void InitiateMovementToCurrentTarget()
    {       
        StopCoroutine(PeriodicallyRequestPath());
        StartCoroutine(PeriodicallyRequestPath());
    }
    public void InitiateMovementToSomePosition(Vector3 _destination)
    {
        PathRequestManager.RequestPath(transform.position, _destination, OnPathFound);
    }
    public void StopMovement(Vector3 destination)
    {
        StopCoroutine(PeriodicallyRequestPath());
        StopCoroutine(FollowPath());

    }


    IEnumerator PeriodicallyRequestPath()
    {
        float delay = 0.25f;


        while (true)
        {
            if (npc.currentTarget.position != destination)
            {
                destination = npc.currentTarget.position;

                if (Vector3.Distance(transform.position, destination) > Grid.instance.nodeDiameter)
                {
                    //print("path requested");
                    //print("distance = " + Vector3.Distance(transform.position, destination));
                    PathRequestManager.RequestPath(transform.position, destination, OnPathFound);

                }
            }
            yield return new WaitForSeconds(delay);
        }
    }
    

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            //print("path successful");
            path = newPath;
            targetIndex = 0;

            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");



        }
    }

    IEnumerator FollowPath()
    {
        //print("followpath started");
        if (path != null && path.Length > 0)
        {
            Vector3 currentWaypoint = path[0];
            //print("currentWaypoint = path[0]");

            while (true)
            {
                if (transform.position == currentWaypoint)
                {
                    targetIndex++;
                    if (targetIndex >= path.Length)
                    {
                        //targetIndex = 0;
                        //path = new Vector3[0];
                        yield break;
                    }
                    currentWaypoint = path[targetIndex];
                    //print("currentWaypoint = path[targetIndex];");
                }

                //print("currentWaypoint = " + currentWaypoint);
                //print("path[targetIndex] = " + path[targetIndex]);
                transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, npcStats.walkSpeed * Time.deltaTime);
                //print("moving " + Time.time);
                yield return new WaitForFixedUpdate();
            }
        }
        else
        {
            print("path[0] is null, fix this shit!");
        }
    }


    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawCube(path[i], Vector3.one * 0.2f);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}
