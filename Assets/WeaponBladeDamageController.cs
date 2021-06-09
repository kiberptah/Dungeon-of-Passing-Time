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
        EventDirector.someBladeAttackStarted += ActivateBlade;
        EventDirector.someBladeAttackFinished += DeactivateBlade;
        EventDirector.someBladeCollision += bladeHit;
    }
    private void OnDisable()
    {
        EventDirector.someBladeAttackStarted -= ActivateBlade;
        EventDirector.someBladeAttackFinished -= DeactivateBlade;
        EventDirector.someBladeCollision -= bladeHit;
    }

    public void ActivateBlade(Transform whichBlade, WeaponStats.Attack attack)
    {
        if (whichBlade == transform)
        {
            if (bladeCollider != null)
            {
                // disable old collider as we change it
                bladeCollider.enabled = false;
                currentDamage = 0;
            }
            //enable new collider
            bladeCollider = attack.attackCollider;
            bladeCollider.enabled = true;

            currentDamage = attack.damage;
            currentAttack = attack;
            //print("pew pew");
        }   
    }
    void DeactivateBlade(Transform whichBlade)
    {
        if (whichBlade == transform)
        {
            bladeCollider.enabled = false;
            currentDamage = 0;
        }
    }



    void bladeHit(Transform blade, Transform target)
    {
        if (!recentlyHitTargets.Contains(target))
        {
            EventDirector.somebody_TakeDamage(target.transform, currentDamage);
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
