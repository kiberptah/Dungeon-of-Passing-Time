using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSlowdownManager : MonoBehaviour
{
    public float OnHitDuration = 0.1f;
    public float OnGettingHitDuration = 0.5f;
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
        //print(who.name + " got hit by " + fromWhom.name);
        if (who.tag != "Player" && fromWhom.tag == "Player")
        {
            StartCoroutine(TimeStop(OnHitDuration));
        }
        if (who.tag == "Player" && fromWhom.tag != "Player")
        {
            StartCoroutine(TimeStop(OnGettingHitDuration));
        }

    }

    IEnumerator TimeStop(float duration)
    {
        //print("time stop");
        Time.timeScale = 0;

        yield return new WaitForSecondsRealtime(duration);

        //print("time resume");
        Time.timeScale = 1f;

        yield return null;
    }
}
