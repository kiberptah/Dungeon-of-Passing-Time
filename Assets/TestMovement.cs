using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMovement : MonoBehaviour
{
    public Transform target;
    public float speed = 2;
    Vector3[] path;
    int targetWaypointIndex = 0;

    void Start()
    {
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
    }
    void Update()
    {
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);

        if (path != null && path.Length > 0 && targetWaypointIndex < path.Length)
        {
            Vector3 currentWaypoint = path[targetWaypointIndex];
            if (transform.position == currentWaypoint)
            {
                targetWaypointIndex++;
            }


            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            //Debug.Log("currentWaypoint = " + currentWaypoint);
            //Debug.Log("npc.transform.position = " + npc.transform.position);
        }
    }

    void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            //Debug.Log("OnPathFound");
            path = newPath;
        }
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetWaypointIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one);

                if (i == targetWaypointIndex)
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
