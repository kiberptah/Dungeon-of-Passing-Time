using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DamageReciever : MonoBehaviour
{
    public List<DamageTypes.types> immunityToDamageTypes = new List<DamageTypes.types>();
    public event Action<float, float, Transform> takingDamage;


    List<Transform> recentAttackers = new List<Transform>();
    float delayBeforeTakingDamageAgain = 0.1f;

    public void TakeDamage(float damage, float knockback, Transform attacker, DamageTypes.types damageType = DamageTypes.types.physical)
    {
        if (!immunityToDamageTypes.Contains(damageType))
        {
            takingDamage?.Invoke(damage, knockback, attacker);

            object[] prmtrs = new object[2] { attacker, delayBeforeTakingDamageAgain };
            StartCoroutine(DontTakeDamageFromRecentAttacker(prmtrs));
        }
    }




    IEnumerator DontTakeDamageFromRecentAttacker(object[] prmtrs)
    {
        Transform attacker = (Transform)prmtrs[0];
        float duration = (float)prmtrs[1];

        recentAttackers.Add(attacker);
        yield return new WaitForSeconds(duration);
        recentAttackers.Remove(attacker);

        yield return null;
    }
}
