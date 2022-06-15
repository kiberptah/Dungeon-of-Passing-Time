using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class DamagePopUp : MonoBehaviour
{
    [SerializeField] TextMeshPro tmp;


    [SerializeField] float lifeTime = 1f;
    float lifeTimeCounter = 1f;

    [SerializeField] float floatingSpeed = 1f;


    float damage;
    Transform damageTaker;
    Transform attacker;

    Vector3 floatingDirection = Vector3.forward;

    #region Init
    public void Initialize(Transform _damageTaker, float _damage, Transform _attacker)
    {
        tmp.text = Mathf.RoundToInt(_damage).ToString();
        damage = _damage;
        damageTaker = _damageTaker;
        attacker = _attacker;

        floatingDirection = damageTaker.position - attacker.position;
        floatingDirection = floatingDirection.normalized;


        StartCoroutine(Dissolving());
        StartCoroutine(FloatingAway());
    }
    #endregion

    IEnumerator Dissolving()
    {
        float coefficient = tmp.color.a / 100f;
        while (true)
        {
            float subtraction = Time.deltaTime / lifeTime;

            tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, tmp.color.a - subtraction);
            lifeTimeCounter -= subtraction;

            if (lifeTimeCounter <= 0)
            {
                Destroy(gameObject);
            }

            yield return null;
        }
    }
    IEnumerator FloatingAway()
    {
        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + floatingDirection, floatingSpeed);

            yield return null;
        }
    }


   
}
