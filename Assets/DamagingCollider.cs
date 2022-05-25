using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingCollider : MonoBehaviour
{
    public Transform whoDealsDamage;
    public float damage;
    public float knockback;

    public float damageTickDelay = 0.1f;
    List<Transform> recentlyHitTargets = new List<Transform>();

    void OnTriggerStay2D(Collider2D other)
    {
        if (!recentlyHitTargets.Contains(other.transform))
        {
            if (other.TryGetComponent<IHealth>(out IHealth otherHealth))
            {
                otherHealth.TakeDamage(damage, knockback, whoDealsDamage);
            }

            object[] prmtrs = new object[2] { other.transform, damageTickDelay };
            StartCoroutine("DontHitRecentTargets", prmtrs);
        }
    }

    IEnumerator DontHitRecentTargets(object[] prmtrs)
    {
        Transform target = (Transform)prmtrs[0];
        float duration = (float)prmtrs[1];

        recentlyHitTargets.Add(target);
        yield return new WaitForSeconds(duration);
        recentlyHitTargets.Remove(target);

        yield return null;
    }
}
