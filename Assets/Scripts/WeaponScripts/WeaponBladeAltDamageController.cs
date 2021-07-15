using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBladeAltDamageController : MonoBehaviour
{
    WeaponStatsAlt weaponStats;
    float currentDamage;
    Vector3 currentBladeVelocity;
    Vector3 currentCharVelocity;
    List<Transform> recentlyHitTargets = new List<Transform>();


    private void OnEnable()
    {
        EventDirector.someBladeUpdateVelocity += UpdateBladeVelocity;
        EventDirector.someBladeCollision += BladeHit;

    }
    private void OnDisable()
    {
        EventDirector.someBladeUpdateVelocity -= UpdateBladeVelocity;
        EventDirector.someBladeCollision -= BladeHit;

    }
    private void Awake()
    {
        weaponStats = GetComponent<WeaponStatsAlt>();
    }
    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    void BladeHit(Transform blade, Transform target)
    {
        if (blade == transform)
        {
            //print("hit");
            if (target.TryGetComponent(out IHealth targetHealth))
            {
                if (!recentlyHitTargets.Contains(target))
                {
                    UpdateBladeDamage(blade, target);
                    EventDirector.somebody_TakeDamage?.Invoke(target.transform, currentDamage, transform.parent);
                    EventDirector.somebody_Knockback?.Invoke(target.transform, weaponStats.knockbackModifier, currentDamage, transform.parent);
                    StartCoroutine(dontHitRecentTarget(target));

                    //print("damage: " + currentDamage);
                }
            }
        }
    }
    IEnumerator dontHitRecentTarget(Transform target)
    {
        recentlyHitTargets.Add(target);
        yield return new WaitForSeconds(weaponStats.damageTickDelay);
        recentlyHitTargets.Remove(target);

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
                currentDamage = weaponStats.damage;
                if ((currentCharVelocity + currentBladeVelocity).magnitude == 0)
                {
                    currentDamage = 0;
                }
                break;
            case WeaponStatsAlt.damageCalculationTypes.no:
                currentDamage = weaponStats.damage;
                if ((currentCharVelocity + currentBladeVelocity).magnitude == 0)
                {
                    currentDamage = 0;
                }
                break;
            

            case WeaponStatsAlt.damageCalculationTypes.sword1:
                currentDamage = weaponStats.damage * Mathf.Pow(totalVelocity * 10f, weaponStats.velocityDamageModifier);
                break;
            case WeaponStatsAlt.damageCalculationTypes.dagger:
                currentDamage = weaponStats.damage;
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
