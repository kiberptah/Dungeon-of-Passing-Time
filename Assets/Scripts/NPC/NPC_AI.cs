using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class NPC_AI : MonoBehaviour
{
    ActorStats myStats;
    [HideInInspector] public NPC_Controller npcController;
    Transform npc;


    [SerializeField]
    private string currentStateName;
    private string previousStateName;
    private INPCState currentState;


    [HideInInspector] public Transform currentTarget;
    [HideInInspector] public Vector3 lastKnownTargetPosition;


    public INPCState state_skeleton_idle = new NPCState_Skeleton_Idle();
    public INPCState state_skeleton_combat;
    public INPCState state_skeleton_chase;

    void Awake()
    {
        npcController = GetComponent<NPC_Controller>();
        npc = npcController.actor.transform;

        myStats = npcController.actor.actorStats;

        currentState = state_skeleton_idle;
    }

    void Start()
    {
        currentState?.ChangeToThisState(this);
        previousStateName = currentStateName;
        //Debug.Log(myStats);

        //StartCoroutine("LookForEnemies");
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


    List<Transform> LookForTheObjectsAround()
    {
        List<Transform> sightedObjects = new List<Transform>();

        //Debug.Log(npc.transform.position);
        //Debug.Log(stats.sightRadius);
        List<Collider2D> sightedColliders = new List<Collider2D>();
        sightedColliders = Physics2D.OverlapCircleAll(npc.transform.position, myStats.sightRadius).Cast<Collider2D>().ToList<Collider2D>();

        foreach (Collider2D obj in sightedColliders)
        {
            sightedObjects.Add(obj.transform);
        }

        return sightedObjects;
    }


    public bool EyeContactWithTarget(Transform target)
    {
        if (target != null)
        {
            RaycastHit2D raycast =
                Physics2D.Raycast(npc.transform.position, npc.transform.TransformDirection(target.position - npc.transform.position),
                myStats.sightRadius, ~LayerMask.GetMask("Weapon", "Object"));

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
        }
        else
        {
            return false;
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
}
