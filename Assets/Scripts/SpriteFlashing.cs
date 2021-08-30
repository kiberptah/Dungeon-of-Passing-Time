using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlashing : MonoBehaviour
{
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
        float delay = 0.05f;
        int flicksAmount = 2;
        int i = 0;
        while (i < flicksAmount)
        {
            ++i;

            sprite.material = whiteMat;

            //yield return null;
            yield return new WaitForSeconds(delay);
            sprite.material = originalMat;
            yield return new WaitForSeconds(delay);
        }
        yield return null;
    }
}
