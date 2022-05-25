using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AI_StateController : MonoBehaviour
{
    public Dictionary<string, AI_ActionData> actionData = new Dictionary<string, AI_ActionData>();
    public bool aiActive = true;

    [HideInInspector] public AI_Input input;

    public Transform actor;
    [HideInInspector] public ActorStats actorStats;
    [HideInInspector] public ActorHealth actorHealth;


    #region StateMachine
    public AI_State currentState;
    [HideInInspector] public float timeInState = 0;
    #endregion

    #region Looking
    [HideInInspector] public List<Transform> sightedObjects = new List<Transform>();
    [HideInInspector] public Transform currentTarget;
    #endregion



    #region MonoBehavior
    void Awake()
    {
        currentState.EnterState(this);

        input = GetComponent<AI_Input>();
        actorStats = actor.GetComponent<ActorStats>();
        actorHealth = actor.GetComponent<ActorHealth>();
    }
    void Update()
    {
        if (!aiActive)
        { return; }

        currentState.UpdateState(this);
        timeInState += Time.deltaTime;



    }
    #endregion







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



    void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            /* 
            Gizmos.color = Color.red;
            Gizmos.DrawLine(actor.position, destination);
            */

            float sphereRadius = 1f;
            if (currentState != null)
            {
                currentState.debug_Color.a = 0.33f;
                Gizmos.color = currentState.debug_Color;

                Gizmos.DrawSphere(actor.position, sphereRadius);
            }
        }
    }

}
