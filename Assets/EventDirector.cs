using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventDirector : MonoBehaviour
{

    public static Action<Transform, float> somebody_Heal;
    public static Action<Transform, float> somebody_TakeDamage;
    public static Action<Transform> somebody_Death;
    public static Action player_Death;

    public static Action player_Attack;

    public static Action<Transform, WeaponStats.Attack> someBladeAttackStarted; // blade, attack
    public static Action<Transform> someBladeAttackFinished; // blade
    public static Action<Transform, Transform> someBladeCollision; //blade, target
    
    
    public static Action<Transform, float> someBladeUpdateVelocity;













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
}
