using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamageController : MonoBehaviour
{
    [SerializeField] Weapon weaponScript;

    [SerializeField] AttackCollisionCallback slashCollider;
    [SerializeField] AttackCollisionCallback pierceCollider;

    List<Transform> recentlySlashedTargets = new List<Transform>();
    List<Transform> recentlyPiercedTargets = new List<Transform>();

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
    private void Awake()
    {

    }
    void BladeSlashHit(Transform target)
    {
        if (!recentlySlashedTargets.Contains(target))
        {
            EventDirector.somebody_TakeDamage?.Invoke(target.transform, weaponScript.currentDamage, transform.parent);
            EventDirector.somebody_Knockback?.Invoke(target.transform, weaponScript.stats.slashKnockbackModifier, weaponScript.currentDamage, transform.parent);
            StartCoroutine(dontSlashRecentTarget(target));
            StartCoroutine(dontPierceRecentTarget(target));
        }
    }

    void BladePierceHit(Transform target)
    {
        if(!recentlyPiercedTargets.Contains(target))
            {
            EventDirector.somebody_TakeDamage?.Invoke(target.transform, weaponScript.stats.pierceDamage, transform.parent);
            EventDirector.somebody_Knockback?.Invoke(target.transform, weaponScript.stats.pierceKnockbackModifier, weaponScript.currentDamage, transform.parent);
            StartCoroutine(dontPierceRecentTarget(target));
            StartCoroutine(dontSlashRecentTarget(target));
        }
    }





    IEnumerator dontSlashRecentTarget(Transform target)
    {
        recentlySlashedTargets.Add(target);
        yield return new WaitForSeconds(weaponScript.stats.slashDamageTickDelay);
        recentlySlashedTargets.Remove(target);

        yield return null;
    }
    IEnumerator dontPierceRecentTarget(Transform target)
    {
        recentlyPiercedTargets.Add(target);
        yield return new WaitForSeconds(weaponScript.stats.pierceDamageTickDelay);
        recentlyPiercedTargets.Remove(target);

        yield return null;
    }

}
