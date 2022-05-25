using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corpse : MonoBehaviour
{
    public AnimLib animLib;
    ActorAnimManager animManager;
    GetKnockbacked getKnockbacked;
    void OnEnable()
    {

    }


    public void Initialize(Transform actor, float dmg, float kb, Transform killer)
    {
        animManager = GetComponent<ActorAnimManager>();
        getKnockbacked = GetComponent<GetKnockbacked>();


        Vector2 dir = (killer.position - actor.position).normalized;
        //Debug.Log(dir);
        animManager.UpdateAnimation("death", dir, false);
        /* 
        animManager.UpdateDirection(dir);
        animManager.UpdateAction(AnimManager.spriteAction.death);
 */
        getKnockbacked.Knockback(dmg, kb, killer);

    }

    void Update()
    {
        //animManager.UpdateAction(AnimManager.spriteAction.death);

    }
}
