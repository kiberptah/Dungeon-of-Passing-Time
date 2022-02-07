using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class NPC_AI : MonoBehaviour
{


    public ActorStats myStats;
    [HideInInspector] public NPC_Controller npcController;
    //public Transform npc;
    public Transform actor;


    [SerializeField]
    private string currentStateName;
    private string previousStateName;
    private INPCState currentState;


    [HideInInspector] public Transform currentTarget;
    [HideInInspector] public Vector3 lastKnownTargetPosition;


    public INPCState state_skeleton_idle = new NPCState_Skeleton_Idle();
    public INPCState state_skeleton_combat = new NPCState_Skeleton_Combat();
    public INPCState state_skeleton_chase = new NPCState_Skeleton_Chase();

    void Awake()
    {
        npcController = GetComponent<NPC_Controller>();
        actor = npcController.actor.transform;

        currentState = state_skeleton_idle;
    }

    void Start()
    {

        myStats = npcController.actor.actorStats;


        currentState?.ChangeToThisState(this);
        previousStateName = currentStateName;
        //Debug.Log(myStats);

        StartCoroutine("LookForEnemies");
    }
    void Update()
    {
        currentState = currentState?.DoState(this);
        currentStateName = currentState.ToString();

        if (previousStateName != currentStateName)
        {
            currentState.ChangeToThisState(this);
            previousStateName = currentStateName;
        }

    }


    IEnumerator LookForEnemies()
    {
        float interval = 0.5f;

        while (true)
        {
            currentTarget = null;

            List<Transform> sightedObjects = LookForTheObjectsAround();

            foreach (Transform obj in sightedObjects)
            {
                //Debug.Log("I see " + obj);
                // check if object is NPC
                if (obj.transform.TryGetComponent(out ActorStats otherStats))
                {
                    // check if there's an eye contact with an object
                    if (EyeContactWithTarget(obj))
                    {
                        // decide if it is an enemy and make it the target if true
                        foreach (ActorStats.Factions hatedFaction in myStats.hatedFactions)
                        {
                            if (hatedFaction == otherStats.faction)
                            {
                                currentTarget = obj;
                                lastKnownTargetPosition = currentTarget.position;
                            }
                        }
                    }
                }
            }
            //print("look for enemise");
            yield return new WaitForSeconds(interval);
        }
    }

    List<Transform> LookForTheObjectsAround()
    {
        List<Transform> sightedObjects = new List<Transform>();

        //Debug.Log(npc.transform.position);
        //Debug.Log(stats.sightRadius);
        List<Collider2D> sightedColliders = new List<Collider2D>();
        sightedColliders = Physics2D.OverlapCircleAll(actor.transform.position, myStats.sightRadius).Cast<Collider2D>().ToList<Collider2D>();

        foreach (Collider2D obj in sightedColliders)
        {
            if (obj.tag == "Actor" || obj.tag == "Geometry")
            {
                sightedObjects.Add(obj.transform);
            }
        }

        return sightedObjects;
    }


    public List<string> testsight;
    public bool EyeContactWithTarget(Transform target)
    {
        if (target != null)
        {
            /*
            RaycastHit2D raycast =
                Physics2D.Raycast(npc.transform.position, npc.transform.TransformDirection(target.position - npc.transform.position),
                myStats.sightRadius, ~LayerMask.GetMask("Weapon", "Object"));
            */
            RaycastHit2D[] raycast =
                Physics2D.RaycastAll(
                    origin: actor.transform.position,
                    direction: actor.transform.TransformDirection(target.position - actor.transform.position),
                    distance: myStats.sightRadius,
                    layerMask: ~LayerMask.GetMask("Weapon", "Object")
                    );
            /*
            List<RaycastHit2D> raycastResults = new List<RaycastHit2D>();
            raycastResults = raycast.ToList<RaycastHit2D>();
            //sort list
            foreach (var entry in raycastResults)
            {
                if (entry.distance < raycastResults[0].distance)
                {
                    raycastResults.RemoveAt(raycastResults.IndexOf(entry));
                    raycastResults.Insert(0, entry);
                }

            }
            testsight = new List<string>();
            foreach (var entry in raycastResults)
            {
                testsight.Add(entry.transform.name);
            }
            */

            //Debug.Log("list size = " + raycastResults.Count);
            foreach (var result in raycast)
            {
                //Debug.Log(result.transform.name);
                if (result.transform == target)
                {
                    foreach (var potentialObstacle in raycast)
                    {
                        //if (potentialObstacle.transform.gameObject.layer == LayerMask.GetMask("Geometry"))
                        if (potentialObstacle.transform.gameObject.tag == "Geometry")
                        {
                            if (potentialObstacle.distance < result.distance)
                            {
                                return false;
                            }
                        }
                    }

                    return true;
                }

            }
            //Debug.Log("list end");
            return false;
            /*
            if (raycast.transform == target)
            {
                if (raycast.transform == currentTarget)
                {
                    lastKnownTargetPosition = raycast.transform.position;
                }
                return true;
            }
            else
            {
                return false;
            }
            */
        }
        else
        {
            return false;
        }
    }



    [HideInInspector] public Vector3[] testpath;
    [HideInInspector] public int testtargetWaypointIndex;
    [HideInInspector] public Vector3 testNextNode;
    void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = new Color(0, 1, 0, 0.1f);
            Gizmos.DrawSphere(actor.transform.position, myStats.sightRadius);

            Gizmos.color = Color.blue;
            Gizmos.DrawCube(testNextNode, Vector3.one);
            Gizmos.DrawLine(actor.transform.position, testNextNode);

            if (testpath != null)
            {
                for (int i = 0; i < testpath.Length; ++i)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawCube(testpath[i], Vector3.one * 0.5f);

                    if (i == testtargetWaypointIndex)
                    {
                        Gizmos.DrawLine(actor.transform.position, testpath[i]);
                    }
                    else
                    {
                        Gizmos.DrawLine(testpath[i - 1], testpath[i]);
                    }
                }
            }
        }
    }


}
