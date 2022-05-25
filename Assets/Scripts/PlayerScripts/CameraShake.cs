using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Transform mainCamera;
    private void OnEnable()
    {
        //EventDirector.somebody_TakeDamage += CatchHitInfo;
        EventDirector.somebody_TookDamage += CatchHitInfo;
    }
    private void OnDisable()
    {
        //EventDirector.somebody_TakeDamage -= CatchHitInfo;
        EventDirector.somebody_TookDamage -= CatchHitInfo;
    }

    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetButtonDown("test"))
        {
            StartCoroutine(Shaking(0.1f, 0.1f));
        }
    }

    void CatchHitInfo(Transform who, float damage, Transform byWhom)
    {
        //Debug.Log("hit by " + byWhom + " to " + who);

        if (byWhom.GetComponent<ActorConnector>().isPlayerControlled == true)
        {
            float intensity = 0.05f;
            float duration = 0.1f;

            StartCoroutine(Shaking(intensity, duration));

        }
        if (who.GetComponent<ActorConnector>().isPlayerControlled == true)
        {
            float intensity = 0.2f;
            float duration = 0.025f;


            StartCoroutine(Shaking(intensity, duration));

        }
    }

    IEnumerator Shaking(float intensity, float duration)
    {
        Vector3 originalPosition;
        Vector3 offset;
        float timer = 0;
        while (timer < duration)
        {
            originalPosition = mainCamera.position;
            float x = System.Convert.ToInt32(Random.value > 0.5f) * intensity;
            float y = System.Convert.ToInt32(Random.value > 0.5f) * intensity;

            offset = new Vector3(x, y, 0);
            mainCamera.position += offset;

            timer += Time.deltaTime;
            yield return null;
            //yield return WaitForSeconds(Time.deltaTime); // scales with time
            mainCamera.position = originalPosition;
        }


        yield return null;
    }
}
