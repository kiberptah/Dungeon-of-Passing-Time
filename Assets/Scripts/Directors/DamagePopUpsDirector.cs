using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePopUpsDirector : MonoBehaviour
{
    public GameObject damagePopUp;

    private void OnEnable()
    {
        //EventDirector.somebody_TakeDamage += SpawnDamagePopUp;
        EventDirector.somebody_TookDamage += SpawnDamagePopUp;
    }
    private void OnDisable()
    {
        //EventDirector.somebody_TakeDamage -= SpawnDamagePopUp;
        EventDirector.somebody_TookDamage -= SpawnDamagePopUp;
    }

    void SpawnDamagePopUp(Transform who, float amount, Transform fromWhom)
    {
        if (amount > 0)
        {
            GameObject newPopUp = Instantiate(damagePopUp, who.position, Quaternion.identity);
            newPopUp.GetComponent<DamagePopUp>().Initialize(who, amount, fromWhom);

        }

    }
}
