using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetKnockbacked : MonoBehaviour
{
    IHealth health;
    public float healthCapPercentForMaxKnockback = 0.1f;
    private void Awake()
    {
        health = GetComponent<IHealth>();
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
        /*
        knockback = Mathf.Clamp(knockback * (damage / (health.currentHealth * healthCapPercentForMaxKnockback)), 0, knockback);
        Vector3 movementDirection = (who.position - byWhom.position).normalized * knockback;
        who.GetComponent<Rigidbody2D>().AddRelativeForce(movementDirection, ForceMode2D.Impulse);
        print("knockback");
        */
        
        if (who == transform)
        {
            //StartCoroutine(GetKnocked(who, knockback, damage, byWhom));
            GetKnockedRb(who, knockback, damage, byWhom);



        }

    }

   void GetKnockedRb(Transform who, float knockback, float damage, Transform byWhom)
    {      
        transform.TryGetComponent(out Rigidbody2D rb);
        if (rb != null)
        {
            Vector3 movementDirection = who.position - byWhom.position;
                      
            knockback = Mathf.Clamp(knockback * (damage / (health.currentHealth * healthCapPercentForMaxKnockback)), 0, knockback);
            /*
            if (damage / health.currentHealth < 0.1f)
            {
                knockback = knockback * 0.5f;
            }
            */
            if (damage < 1)
            {
                knockback = 0;
            }

            rb.AddRelativeForce(movementDirection * knockback, ForceMode2D.Impulse);

        }
    }

    float knockbackDuration = 0.2f;
    IEnumerator GetKnocked(Transform who, float knockback, float damage, Transform byWhom)
    {
        who.TryGetComponent(out Rigidbody2D rb);
        if (rb != null)
        {
            Vector3 movementDirection = who.position - byWhom.position;
            float timer = 0;

            knockback = Mathf.Clamp(knockback * (damage / (health.currentHealth * healthCapPercentForMaxKnockback)), 0, knockback);
            //print("knockback: " + knockback + " damage: " + damage);
            while (timer < knockbackDuration)
            {
                transform.position = Vector3.MoveTowards(transform.position, transform.position + movementDirection, knockback * Time.deltaTime);
                rb.MovePosition(transform.position + movementDirection * knockback * Time.fixedDeltaTime);


                timer += Time.deltaTime;
                yield return null;
            }


        }
    }
}
