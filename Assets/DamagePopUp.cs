using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class DamagePopUp : MonoBehaviour
{
    public TextMeshPro tmp;

    public float lifeTime = 1f;
    float lifeTimeCounter = 1f;
    public float floatingSpeed = 1f;
    float damage;
    Transform damageTaker;
    Transform attacker;

    Vector3 floatingDirection = Vector3.up;

    private void Start()
    {
        
    }

    IEnumerator Dissolving()
    {
        //float defaultColorAlpha = tmp.color.a;
        float coefficient = tmp.color.a / 100f;
        while (true)
        {
            float subtraction = Time.deltaTime / lifeTime;
            //transform.localScale = new Vector3(transform.localScale.x - subtraction, transform.localScale.y - subtraction, transform.localScale.z - subtraction);
            tmp.color = new Color (tmp.color.r, tmp.color.g, tmp.color.b, tmp.color.a - subtraction);
            lifeTimeCounter -= subtraction;

            if (lifeTimeCounter <= 0)
            {
                //StopCoroutine(Dissolving()); ?
                Destroy(gameObject);
            }

            yield return null;
        }
    }
    IEnumerator FloatingAway()
    {
        while (true)
        {
            //print("adsda");
            transform.position = Vector3.MoveTowards(transform.position, transform.position + floatingDirection, floatingSpeed);

            yield return null;
        }
    }


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
}
