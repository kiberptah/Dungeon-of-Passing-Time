using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBladeAltDamageController : MonoBehaviour
{
    WeaponStatsAlt weaponStats;
    float currentDamage;
    List<Transform> recentlyHitTargets = new List<Transform>();


    private void OnEnable()
    {
        EventDirector.someBladeUpdateVelocity += UpdateBladeDamageBasedOnVelocity;
        EventDirector.someBladeCollision += BladeHit;

    }
    private void OnDisable()
    {
        EventDirector.someBladeUpdateVelocity -= UpdateBladeDamageBasedOnVelocity;
        EventDirector.someBladeCollision -= BladeHit;

    }
    private void Awake()
    {
        weaponStats = GetComponent<WeaponStatsAlt>();
    }

    void BladeHit(Transform blade, Transform target)
    {
        if (blade == transform)
        {
            if (!recentlyHitTargets.Contains(target))
            {
                EventDirector.somebody_TakeDamage?.Invoke(target.transform, currentDamage);
                StartCoroutine(dontHitRecentTarget(target));

                //print("damage: " + currentDamage);
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

    void UpdateBladeDamageBasedOnVelocity(Transform blade, float velocity)
    {
        switch(weaponStats.typeOfDamageScaling)
        {
            default:
                currentDamage = weaponStats.damage;
                if (velocity == 0)
                {
                    currentDamage = 0;
                }
                break;
            case 0:
                currentDamage = weaponStats.damage;
                if (velocity == 0)
                {
                    currentDamage = 0;
                }
                break;
            case 1:
                currentDamage = weaponStats.damage * Mathf.Pow(velocity * 100f, weaponStats.velocityDamageModifier) / 100f;
                break;
            case 2:
                currentDamage = weaponStats.damage * velocity * 10f;
                break;



        }
        //print("currentDamage: " + currentDamage);

        //currentDamage = weaponStats.damage * velocity * weaponStats.velocityDamageModifier;

    }
}
