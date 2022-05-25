using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetKnockbacked : MonoBehaviour
{
    #region DEPENDENCIES
    ActorHealth actorHealth;
    #endregion
    public float healthCapPercentForMaxKnockback = 0.1f;
    private void Awake()
    {
        actorHealth = GetComponent<ActorHealth>();
    }
    private void OnEnable()
    {
        //EventDirector.somebody_Knockback += Knockback;
        if (actorHealth != null)
        {
            actorHealth.TakingDamage += Knockback;
        }
    }
    private void OnDisable()
    {
        //EventDirector.somebody_Knockback -= Knockback;
        if (actorHealth != null)
        {
            actorHealth.TakingDamage -= Knockback;
        }
    }

    public void Knockback(float damage, float knockback, Transform fromWhom)
    {
        GetKnockedRb(transform, knockback, damage, fromWhom);
    }
    /* 
    public void DirectKnockback(Transform who, float knockback, float damage, Transform byWhom)
    {
        GetKnockedRb(who, knockback, damage, byWhom);
    } */
    void GetKnockedRb(Transform who, float knockback, float damage, Transform byWhom)
    {
        transform.TryGetComponent(out Rigidbody2D rb);
        if (rb != null)
        {
            Vector3 knockbackDirection = (who.position - byWhom.position).normalized;

            //knockback = Mathf.Clamp(knockback * (damage / (health.currentHealth * healthCapPercentForMaxKnockback)), 0, knockback);

            if (damage < 1)
            {
                knockback = 0;
            }

            rb.AddRelativeForce(knockbackDirection * knockback, ForceMode2D.Impulse);
        }
    }
}
