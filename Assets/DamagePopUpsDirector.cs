using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePopUpsDirector : MonoBehaviour
{
    public GameObject damagePopUp;

    private void OnEnable()
    {
        EventDirector.somebody_TakeDamage += SpawnDamagePopUp;
    }
    private void OnDisable()
    {
        EventDirector.somebody_TakeDamage -= SpawnDamagePopUp;
    }

    void SpawnDamagePopUp(Transform who, float amount, Transform fromWhom)
    {
        GameObject newPopUp = Instantiate(damagePopUp, who.position, Quaternion.identity);
        newPopUp.GetComponent<DamagePopUp>().Initialize(who, amount, fromWhom);
    }
}
