using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetKnockbacked : MonoBehaviour
{
    #region DEPENDENCIES
    ActorHealth actorHealth;
    #endregion

    [SerializeField] float healthCapPercentForMaxKnockback = 0.1f;

    #region Init
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
    #endregion

    public void Knockback(float damage, float knockback, Transform fromWhom)
    {
        GetKnockedRb(transform, knockback, damage, fromWhom);
    }

    void GetKnockedRb(Transform who, float knockback, float damage, Transform byWhom)
    {
        transform.TryGetComponent(out Rigidbody rb);
        if (rb != null)
        {
            Vector3 knockbackDirection = (who.position - byWhom.position).normalized;

            if (damage < 1)
            {
                knockback = 0;
            }

            rb.AddRelativeForce(knockbackDirection * knockback, ForceMode.Impulse);
        }
    }
}
