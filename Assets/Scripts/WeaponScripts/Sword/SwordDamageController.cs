using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordDamageController : MonoBehaviour
{
    [SerializeField] Sword swordScript;

    [SerializeField] AttackCollisionCallback slashCollider;
    [SerializeField] AttackCollisionCallback pierceCollider;

    List<DamageReciever> recentlySlashedTargets = new List<DamageReciever>();
    List<DamageReciever> recentlyPiercedTargets = new List<DamageReciever>();



    private void OnEnable()
    {
        slashCollider.collisionCallback += BladeSlashHit;
        pierceCollider.collisionCallback += BladePierceHit;
    }
    private void OnDisable()
    {
        slashCollider.collisionCallback -= BladeSlashHit;
        pierceCollider.collisionCallback -= BladePierceHit;
    }

    void BladeSlashHit(DamageReciever target)
    {
        if (!recentlySlashedTargets.Contains(target))
        {
            if (swordScript.currentSlashDamage > 0)
            {
                //EventDirector.somebody_TakeDamage?.Invoke(target.transform, weaponScript.currentDamage, transform.parent);
                //EventDirector.somebody_Knockback?.Invoke(target.transform, weaponScript.stats.slashKnockbackModifier, weaponScript.currentDamage, transform.parent);
                //Debug.Log("target: " + target);

                target.TakeDamage(
                    swordScript.currentSlashDamage,
                    swordScript.swordStats.slashKnockbackModifier,
                    swordScript.connector.actor.transform);
            }

            object[] prmtrs = new object[2] { target, swordScript.swordStats.slashDamageTickDelay };
            //StartCoroutine("DontSlashRecentTarget", prmtrs);
            //StartCoroutine("DontPierceRecentTarget", prmtrs);
            StartCoroutine(DontSlashRecentTarget(prmtrs));
            StartCoroutine(DontPierceRecentTarget(prmtrs));

        }
    }

    void BladePierceHit(DamageReciever target)
    {
        if (!recentlyPiercedTargets.Contains(target))
        {

            if (swordScript.swordStats.pierceDamage > 0)
            {
                //EventDirector.somebody_TakeDamage?.Invoke(target.transform, weaponScript.stats.pierceDamage, transform.parent);
                //EventDirector.somebody_Knockback?.Invoke(target.transform, weaponScript.stats.pierceKnockbackModifier, weaponScript.stats.pierceDamage, transform.parent);

                //EventDirector.somebody_TakeDamageAlt?.Invoke(target.transform, weaponScript.stats.pierceDamage, weaponScript.stats.pierceKnockbackModifier, transform.parent);

                target.TakeDamage(
                    swordScript.swordStats.pierceDamage,
                    swordScript.swordStats.pierceKnockbackModifier,
                    swordScript.connector.actor.transform);
            }

            float pierceTime = (swordScript.swordStats.piercingDistance - swordScript.weaponStats.minDistanceFromBody) / swordScript.swordStats.pierceAttackSpeed * Time.fixedDeltaTime;
            float recoveryTime = (swordScript.swordStats.piercingDistance - swordScript.weaponStats.minDistanceFromBody) / swordScript.swordStats.pierceRecoverSpeed * Time.fixedDeltaTime;

            float delay = pierceTime + swordScript.swordStats.pierceHoldTime + recoveryTime;

            object[] prmtrs = new object[2] { target, delay };
            //StartCoroutine("DontPierceRecentTarget", prmtrs);
            //StartCoroutine("DontSlashRecentTarget", prmtrs);
            StartCoroutine(DontPierceRecentTarget(prmtrs));
            StartCoroutine(DontSlashRecentTarget(prmtrs));
        }
    }





    IEnumerator DontSlashRecentTarget(object[] prmtrs)
    {
        DamageReciever target = (DamageReciever)prmtrs[0];
        float duration = (float)prmtrs[1];

        recentlySlashedTargets.Add(target);
        yield return new WaitForSeconds(duration);
        recentlySlashedTargets.Remove(target);

        yield return null;
    }
    IEnumerator DontPierceRecentTarget(object[] prmtrs)
    {
        DamageReciever target = (DamageReciever)prmtrs[0];
        float duration = (float)prmtrs[1];

        recentlyPiercedTargets.Add(target);
        //yield return new WaitForSeconds(weaponScript.stats.pierceDamageTickDelay);
        yield return new WaitForSeconds(duration);
        recentlyPiercedTargets.Remove(target);

        yield return null;
    }

}
