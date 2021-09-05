using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventDirector : MonoBehaviour
{
    public static Action scene_Reset; // reset current scene


    public static Action<Transform, float, Transform> somebody_Heal; // who, amout, from whom
    public static Action<Transform, float, Transform> somebody_TakeDamage; // who, amout, from whom
    public static Action<Transform, float> somebody_LoseStamina; // who, amout
    public static Action<Transform, float> somebody_GainStamina; // who, amout
    public static Action<Transform, Vector2> somebody_UpdateSpriteVector; // who direction animation
    public static Action<Transform, ActorAnimationManager.spriteDirection> somebody_UpdateSpriteDirection; // who direction animation
    public static Action<Transform, ActorAnimationManager.spriteAction> somebody_UpdateSpriteAction; // who direction animation

    public static Action<Transform, float, float, Transform> somebody_Knockback; // who, knockback, damage, from whom
    public static Action<Transform, Transform> somebody_Death; // who, killer
    public static Action player_Death;

    //public static Action player_Attack;

    //public static Action<Transform, WeaponStats.Attack> someBladeAttackStarted; // blade, attack
    //public static Action<Transform> someBladeAttackFinished; // blade
    public static Action<Transform, Transform> someBladeSlashCollision; //blade, target
    public static Action<Transform, Transform> someBladePierceCollision; //blade, target
    
    
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
