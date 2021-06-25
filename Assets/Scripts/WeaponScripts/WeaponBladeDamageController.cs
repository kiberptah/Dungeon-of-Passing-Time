using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBladeDamageController : MonoBehaviour
{
    float currentDamage;
    WeaponStats.Attack currentAttack;
    Collider2D bladeCollider;

    List<Transform> recentlyHitTargets = new List<Transform>();

    private void OnEnable()
    {
        //EventDirector.someBladeAttackStarted += ActivateBlade;
        //EventDirector.someBladeAttackFinished += DeactivateBlade;
        EventDirector.someBladeCollision += bladeHit;
    }
    private void OnDisable()
    {
        //EventDirector.someBladeAttackStarted -= ActivateBlade;
        //EventDirector.someBladeAttackFinished -= DeactivateBlade;
        EventDirector.someBladeCollision -= bladeHit;
    }

    void bladeHit(Transform blade, Transform target)
    {
        if (!recentlyHitTargets.Contains(target))
        {
            EventDirector.somebody_TakeDamage(target.transform, currentDamage, transform);
            StartCoroutine(dontHitRecentTarget(target, currentAttack));
        }

    }

    IEnumerator dontHitRecentTarget(Transform target, WeaponStats.Attack attack)
    {
        recentlyHitTargets.Add(target);
        yield return new WaitForSeconds(attack.damageTickDelay);
        recentlyHitTargets.Remove(target);

        yield return null;
    }
}
