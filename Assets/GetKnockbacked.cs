using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetKnockbacked : MonoBehaviour
{
    IHealth health;
    public float healthCapPercentForMaxKnockback = 0.5f;
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
        if (who == transform)
        {
            StartCoroutine(GetKnocked(who, knockback, damage, byWhom));
        }
    }

    float knockbackDuration = 0.25f;
    IEnumerator GetKnocked(Transform who, float knockback, float damage, Transform byWhom)
    {
        Vector3 movementDirection = who.position - byWhom.position;
        float timer = 0;

        knockback = Mathf.Clamp(knockback * (damage / (health.currentHealth * healthCapPercentForMaxKnockback)), 0, knockback);
        //print("knockback: " + knockback + " damage: " + damage);
        while (timer < knockbackDuration)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + movementDirection, knockback * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }


        
    }
}
