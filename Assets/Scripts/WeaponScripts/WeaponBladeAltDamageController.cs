using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBladeAltDamageController : MonoBehaviour
{
    WeaponStatsAlt weaponStats;
    float currentDamage;
    Vector3 currentBladeVelocity;
    Vector3 currentCharVelocity;
    List<Transform> recentlySlashedTargets = new List<Transform>();
    List<Transform> recentlyPiercedTargets = new List<Transform>();


    private void OnEnable()
    {
        EventDirector.someBladeUpdateVelocity += UpdateBladeVelocity;
        EventDirector.someBladeSlashCollision += BladeSlashHit;
        EventDirector.someBladePierceCollision += BladePierceHit;

    }
    private void OnDisable()
    {
        EventDirector.someBladeUpdateVelocity -= UpdateBladeVelocity;
        EventDirector.someBladeSlashCollision -= BladeSlashHit;
        EventDirector.someBladePierceCollision -= BladePierceHit;

    }
    private void Awake()
    {
        weaponStats = GetComponent<WeaponStatsAlt>();
    }
    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    void BladeSlashHit(Transform blade, Transform target)
    {
        if (blade == transform)
        {
            //print("hit");
            if (target.TryGetComponent(out IHealth targetHealth))
            {
                if (!recentlySlashedTargets.Contains(target))
                {
                    UpdateBladeDamage(blade, target);
                    EventDirector.somebody_TakeDamage?.Invoke(target.transform, currentDamage, transform.parent);
                    EventDirector.somebody_Knockback?.Invoke(target.transform, weaponStats.knockbackModifier, currentDamage, transform.parent);
                    StartCoroutine(dontSlashRecentTarget(target));
                    StartCoroutine(dontPierceRecentTarget(target));

                    //print("damage: " + currentDamage);
                }
            }
        }
    }
    void BladePierceHit(Transform blade, Transform target)
    {
        if (blade == transform)
        {
            //print("hit");
            if (target.TryGetComponent(out IHealth targetHealth))
            {
                if (!recentlyPiercedTargets.Contains(target))
                {
                    UpdateBladeDamage(blade, target);
                    EventDirector.somebody_TakeDamage?.Invoke(target.transform, weaponStats.pierceDamage, transform.parent);
                    EventDirector.somebody_Knockback?.Invoke(target.transform, weaponStats.knockbackModifier, currentDamage, transform.parent);
                    StartCoroutine(dontPierceRecentTarget(target));
                    StartCoroutine(dontSlashRecentTarget(target));

                    //print("damage: " + currentDamage);
                }
            }
        }
    }
    IEnumerator dontSlashRecentTarget(Transform target)
    {
        recentlySlashedTargets.Add(target);
        yield return new WaitForSeconds(weaponStats.slashDamageTickDelay);
        recentlySlashedTargets.Remove(target);

        yield return null;
    }
    IEnumerator dontPierceRecentTarget(Transform target)
    {
        recentlyPiercedTargets.Add(target);
        yield return new WaitForSeconds(weaponStats.pierceDamageTickDelay);
        recentlyPiercedTargets.Remove(target);

        yield return null;
    }
// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    void UpdateBladeVelocity(Transform blade, Vector3 bladeVelocity, Vector3 charVelocity)
    {
        if (blade == transform)
        {
            currentCharVelocity = charVelocity;
            currentBladeVelocity = bladeVelocity;
        }
    }


    void UpdateBladeDamage(Transform blade, Transform target)
    {

        Vector3 bladeToTargetNormal = (target.position - blade.position).normalized;
        Vector3 charVelocityProjection = Vector3.Project(currentCharVelocity, bladeToTargetNormal);

        float charVelocity = currentCharVelocity.magnitude;
        if (Vector3.Distance(blade.position, target.position) < Vector3.Distance(blade.position + charVelocityProjection, target.position))
        {
            charVelocity = -charVelocity;
        }


        //float totalVelocity = currentBladeVelocity.magnitude + charVelocity;
        float totalVelocity = currentBladeVelocity.magnitude;
        if (totalVelocity < 0)
        {
            totalVelocity = 0;
        }

        switch (weaponStats.damageCalcType)
        {
            default:
                currentDamage = weaponStats.slashDamage;
                if ((currentCharVelocity + currentBladeVelocity).magnitude == 0)
                {
                    currentDamage = 0;
                }
                break;
            case WeaponStatsAlt.damageCalculationTypes.no:
                currentDamage = weaponStats.slashDamage;
                if ((currentCharVelocity + currentBladeVelocity).magnitude == 0)
                {
                    currentDamage = 0;
                }
                break;
            

            case WeaponStatsAlt.damageCalculationTypes.sword1:
                currentDamage = weaponStats.slashDamage * Mathf.Pow(totalVelocity * 10f, weaponStats.velocityDamageModifier);
                break;
            case WeaponStatsAlt.damageCalculationTypes.dagger:
                currentDamage = weaponStats.slashDamage;
                if (totalVelocity == 0)
                {
                    currentDamage = 0;
                }
                break;
        }
        //print("currentDamage: " + currentDamage);

        //currentDamage = weaponStats.damage * velocity * weaponStats.velocityDamageModifier;

    }

}
