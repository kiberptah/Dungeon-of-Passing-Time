using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corpse : MonoBehaviour
{
    [HideInInspector] public AnimLib animLib;
    ActorAnimManager animManager;
    GetKnockbacked getKnockbacked;

    public void Initialize(Transform actor, float dmg, float kb, Transform killer)
    {
        animManager = GetComponent<ActorAnimManager>();
        getKnockbacked = GetComponent<GetKnockbacked>();

        Vector3 dir = (killer.position - actor.position).normalized;

        animManager.UpdateAnimation("death", dir, false);
        getKnockbacked.Knockback(dmg, kb, killer);
    }
}
