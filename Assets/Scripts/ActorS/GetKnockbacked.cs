using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetKnockbacked : MonoBehaviour
{
    //IHealth health;
    public float healthCapPercentForMaxKnockback = 0.1f;
    private void Awake()
    {
        //health = GetComponent<IHealth>();
    }
    private void OnEnable()
    {
        EventDirector.somebody_Knockback += Knockback;
    }
    private void OnDisable()
    {
        EventDirector.somebody_Knockback -= Knockback;
    }

    void Knockback(Transform who, float knockback, float damage, Transform byWhom)
    {        
        if (who == transform)
        {
            GetKnockedRb(who, knockback, damage, byWhom);
        }
    }

   void GetKnockedRb(Transform who, float knockback, float damage, Transform byWhom)
   {      
        transform.TryGetComponent(out Rigidbody2D rb);
        if (rb != null)
        {
            Vector3 movementDirection = who.position - byWhom.position;
                      
            //knockback = Mathf.Clamp(knockback * (damage / (health.currentHealth * healthCapPercentForMaxKnockback)), 0, knockback);

            if (damage < 1)
            {
                knockback = 0;
            }

            rb.AddRelativeForce(movementDirection * knockback, ForceMode2D.Impulse);
        }
   }
}
