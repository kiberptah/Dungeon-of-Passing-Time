using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStateMachine : MonoBehaviour
{
    [HideInInspector] public ActorStats npcStats;
    [HideInInspector] public NPCMovement npcMovement;
    [HideInInspector] public WeaponManager npcWeaponManager;
    [HideInInspector] public WeaponController npcWeaponController;

    [SerializeField]
    private string currentStateName;
    private string previousStateName;
    private INPCState currentState;

    public NPCState_Idle idleState = new NPCState_Idle();
    public NPCState_Chase chaseState = new NPCState_Chase();
    public NPCState_Combat combatState = new NPCState_Combat();


    public Transform currentTarget;
    [HideInInspector] public Vector3 lastKnownTargetPosition;

    /// pathfinding
    [HideInInspector] public Vector3[] path;
    [HideInInspector] public int pathNodeIndex;


    //public float timeInThisState = 0;

    


    private void Awake()
    {
        npcStats = GetComponent<ActorStats>();
        npcMovement = GetComponent<NPCMovement>();
        npcWeaponManager = GetComponent<WeaponManager>();
        npcWeaponController = GetComponent<WeaponController>();

        currentState = idleState;
        currentStateName = currentState.ToString();
    }
    private void Start()
    {
        currentState.ChangeToThisState(this);
        previousStateName = currentStateName;

        StartCoroutine("LookForEnemies");
    }


    private void Update()
    {
        currentState = currentState.DoState(this);
        currentStateName = currentState.ToString();

        //timeInThisState += Time.deltaTime;

        if (previousStateName != currentStateName)
        {
            //timeInThisState = 0;

            currentState.ChangeToThisState(this);
            previousStateName = currentStateName;
        }

    }

    

    public List<Transform> LookForTheObjectsAround()
    {
        List<Transform> sightedObjects = new List<Transform>();

        Collider2D[] sightedColliders = Physics2D.OverlapCircleAll(transform.position, npcStats.sightRadius);
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
            //RaycastHit2D raycast = Physics2D.Raycast(transform.position, target.position, npcStats.sightRadius, LayerMask.GetMask("Geometry", "Actors"));
            RaycastHit2D raycast =
                Physics2D.Raycast(transform.position, transform.TransformDirection(target.position - transform.position),
                npcStats.sightRadius, ~LayerMask.GetMask("Weapon", "Object"));


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
                if (obj.transform.TryGetComponent(out ActorStats stats))
                {
                    // check if there's an eye contact with an object
                    if (EyeContactWithTarget(obj))
                    {
                        // decide if it is an enemy and make it the target if true
                        foreach (ActorStats.Factions hatedFaction in npcStats.hatedFactions)
                        {
                            if (hatedFaction == stats.faction)
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



    public Vector3 shittymouseccord = Vector3.zero;
    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.blue;
        //Gizmos.DrawSphere(shittymouseccord, 1f);
        if (currentTarget != null && npcWeaponController.equippedWeapon != null)
        {
            Vector3 targetProjection = transform.InverseTransformPoint(currentTarget.position).normalized;
            Vector3 bladeProjection = transform.InverseTransformPoint(npcWeaponController.equippedWeapon.transform.position).normalized;

            Gizmos.color = Color.red;

            Gizmos.DrawSphere(transform.TransformPoint(targetProjection), 0.1f);
            Gizmos.color = Color.cyan;

            Gizmos.DrawSphere(transform.TransformPoint(bladeProjection), 0.1f);

        }
    }
}
