using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    ActorStats npcStats;
    NPCStateMachine npc;

    Vector3[] path;
    int targetIndex;

    Vector2 destination;
    float timer = 0;
    private void Awake()
    {
        npcStats = GetComponent<ActorStats>();
        npc = GetComponent<NPCStateMachine>();
    }
    private void Start()
    {
        
    }
    private void FixedUpdate()
    {
        if (destination != Vector2.zero)
        {
            float delay = 0.25f;

            if (Vector3.Distance(transform.position, destination) > Grid.instance.nodeDiameter)
            {
                if (timer >= delay)
                {
                    timer = 0;
                    PathRequestManager.RequestPath(transform.position, destination, OnPathFound);
                }
                timer += Time.fixedDeltaTime;
            }
        }
    }
    public void UpdateMovementDestination(Vector2 _destination)
    {
        destination = _destination;
    }
    public void StopMovement()
    {
        destination = Vector2.zero;
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
                //if (transform.position == currentWaypoint)
                if (Vector3.Distance(transform.position, currentWaypoint) < 0.01f)
                {
                    targetIndex++;
                    if (targetIndex >= path.Length)
                    {
                        //targetIndex = 0;
                        //path = new Vector3[0];
                        print("stop");
                        MoveNPC(transform.position);
                        yield break;
                    }
                    currentWaypoint = path[targetIndex];
                    //print("currentWaypoint = path[targetIndex];");
                }

                MoveNPC(currentWaypoint);



                yield return new WaitForFixedUpdate();
            }
        }
        else
        {
            print("path[0] is null, fix this shit! ");
            MoveNPC(transform.position);
        }
    }

    void MoveNPC(Vector3 currentWaypoint)
    {
        Vector3 movementDirection = (currentWaypoint - transform.position).normalized;

        if (movementDirection != Vector3.zero)
        {
            GetComponent<Rigidbody2D>().AddRelativeForce(movementDirection * npcStats.walkSpeed, ForceMode2D.Force);
            EventDirector.somebody_UpdateSpriteDirection(transform, movementDirection, ActorSpritesDirectionManager.spriteAction.walking);
        }
        else
        {
            EventDirector.somebody_UpdateSpriteDirection(transform, movementDirection, ActorSpritesDirectionManager.spriteAction.idle);
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
