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
        currentDamage = weaponStats.damage * velocity * weaponStats.velocityModifier;
    }
}
