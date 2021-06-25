using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventDirector : MonoBehaviour
{

    public static Action<Transform, float, Transform> somebody_Heal; // who, amout, from whom
    public static Action<Transform, float, Transform> somebody_TakeDamage; // who, amout, from whom
    public static Action<Transform, Transform> somebody_Death; // who, killer
    public static Action player_Death;

    //public static Action player_Attack;

    //public static Action<Transform, WeaponStats.Attack> someBladeAttackStarted; // blade, attack
    //public static Action<Transform> someBladeAttackFinished; // blade
    public static Action<Transform, Transform> someBladeCollision; //blade, target
    
    
    public static Action<Transform, Vector3, Vector3> someBladeUpdateVelocity; // blade, bladespeed, walkspeed
    //public static Action<Transform, float> someBladeUpdateVelocity; // blade, bladespeed



    //public static Action<NPCStateMachine, INPCState> npc_StateChange;




    /*
    public static EventDirector instance;
    void Awake()
    {
        if (EventDirector.instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    */
}
