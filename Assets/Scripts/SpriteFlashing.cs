using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlashing : MonoBehaviour
{
    public float delay = 0.025f;
    public int flicksAmount = 2;

    public SpriteRenderer sprite;
    public Material whiteMat;

    Material originalMat;
    private void OnEnable()
    {
        EventDirector.somebody_TakeDamage += Flash;
    }

    private void OnDisable()
    {
        EventDirector.somebody_TakeDamage -= Flash;
    }
    private void Start()
    {
        originalMat = sprite.material;

    }
    void Flash(Transform who, float amount, Transform byWhom)
    {
        if (who == transform && amount > 0)
        {
            StartCoroutine("FlashingCR");
        }
    }

    IEnumerator FlashingCR()
    {
        int i = 0;
        while (i < flicksAmount)
        {
            ++i;

            sprite.material = whiteMat;

            //yield return null;
            yield return new WaitForSecondsRealtime(delay);
            sprite.material = originalMat;
            yield return new WaitForSecondsRealtime(delay);
        }
        yield return null;
    }
}
