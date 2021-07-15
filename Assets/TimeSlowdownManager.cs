using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSlowdownManager : MonoBehaviour
{
    private void OnEnable()
    {
        EventDirector.somebody_TakeDamage += SlowTimeOnHit;
    }
    private void OnDisable()
    {
        EventDirector.somebody_TakeDamage -= SlowTimeOnHit;
    }

    void SlowTimeOnHit(Transform who, float amount  , Transform fromWhom)
    {
        print(who.name + " got hit by " + fromWhom.name);
        if (who.tag != "Player" && fromWhom.tag == "Player")
        {
            StartCoroutine("TimeSlowdown");
        }
    
    }

    IEnumerator TimeSlowdown()
    {
        print("time slowdown");
        Time.timeScale = 0.1f;
        yield return new WaitForSecondsRealtime(.1f);

        Time.timeScale = 1f;


        yield return null;
    }
}
