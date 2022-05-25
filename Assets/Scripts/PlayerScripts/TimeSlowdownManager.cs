using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSlowdownManager : MonoBehaviour
{
    public float onHitDuration = 0.1f;
    public float onGettingHitDuration = 0.5f;
    private void OnEnable()
    {
        //EventDirector.somebody_TakeDamage += SlowTimeOnHit;
        //EventDirector.somebody_TookDamage += SlowTimeOnHit;
        EventDirector.player_TookDamage += StopTimeOnPlayerGettingHit;
        EventDirector.player_DealtDamage += StopTimeOnPlayerHitting;
    }
    private void OnDisable()
    {
        //EventDirector.somebody_TakeDamage -= SlowTimeOnHit;
        //EventDirector.somebody_TookDamage -= SlowTimeOnHit;
        EventDirector.player_TookDamage -= StopTimeOnPlayerGettingHit;
        EventDirector.player_DealtDamage -= StopTimeOnPlayerHitting;
    }

    void SlowTimeOnHit(Transform who, float amount, Transform fromWhom)
    {
        //print(who.name + " got hit by " + fromWhom.name);
        /*
        if (who.tag != "Player" && fromWhom.tag == "Player")
        {
            StartCoroutine(TimeStop(OnHitDuration));
        }
        if (who.tag == "Player" && fromWhom.tag != "Player")
        {
            StartCoroutine(TimeStop(OnGettingHitDuration));
        }
        */
        /*
        if (who.GetComponent<ActorControllerConnector>().controller.tag != "PlayerController"
        && fromWhom.GetComponent<ActorControllerConnector>().controller.tag == "PlayerController")
        {
            StartCoroutine(TimeStop(OnHitDuration));
        }
        if (who.GetComponent<ActorControllerConnector>().controller.tag == "PlayerController"
        && fromWhom.GetComponent<ActorControllerConnector>().controller.tag != "PlayerController")
        {
            StartCoroutine(TimeStop(OnGettingHitDuration));
        }
        */
    }
    void StopTimeOnPlayerHitting()
    {
        StartCoroutine(TimeStop(onHitDuration));
    }
    void StopTimeOnPlayerGettingHit()
    {
        StartCoroutine(TimeStop(onGettingHitDuration));
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
