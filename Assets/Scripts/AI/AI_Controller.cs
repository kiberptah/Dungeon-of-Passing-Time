using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AI_Controller : MonoBehaviour
{
    //public Dictionary<string, AI_ActionData> actionData = new Dictionary<string, AI_ActionData>();
    public bool aiActive = true;

    [HideInInspector] public AI_Input input;

    public Transform actor;
    //[HideInInspector] public ActorConnector actorConnector;
    [HideInInspector] public ActorStats actorStats;
    [HideInInspector] public ActorHealth actorHealth;


    #region AI
    public AI_Behavior myBehavior;
    public AI_BehaviorData behaviorData;

    //[HideInInspector] public float timeInState = 0;
    #endregion

    #region Looking
    [HideInInspector] public List<Transform> sightedObjects = new List<Transform>();
    [HideInInspector] public Transform currentTarget;
    #endregion



    #region MonoBehavior
    void Awake()
    {
        myBehavior.Initialize(this);
        input = GetComponent<AI_Input>();
        actorStats = actor.GetComponent<ActorStats>();
        actorHealth = actor.GetComponent<ActorHealth>();
        //actorConnector = actor.GetComponent<ActorConnector>();
    }
    void Update()
    {
        if (!aiActive)
        { return; }

        myBehavior.BehaviorLoop(this);

        //timeInState += Time.deltaTime;

        

    }
    #endregion





    /*

    #region States Changing
    public void TransitionToState(AI_State nextState)
    {
        if (nextState != null && nextState != currentState)
        {
            //Debug.Log("CHANGE STATE");
            timeInState = 0;

            currentState.ExitState(this);
            currentState = nextState;
            currentState.EnterState(this);

        }
    }

    #endregion
    */




    [HideInInspector] public List<Vector3> debug_path = new List<Vector3>();
    [HideInInspector] public Vector3 debug_destination;

    /*
    void OnDrawGizmos()
    {
        
        if (Application.isPlaying)
        {

            foreach (var node in debug_path)
            {
                Gizmos.color = Color.white;
                if (node == debug_destination)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(transform.position, node);
                }

                Gizmos.DrawSphere(node, .25f);
            }

            // show state
            float sphereRadius = 1f;
            if (currentState != null)
            {
                currentState.debug_Color.a = 0.33f;
                Gizmos.color = currentState.debug_Color;

                Gizmos.DrawSphere(actor.position, sphereRadius);
            }
        }

    }
    */
}
