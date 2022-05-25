using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlashing : MonoBehaviour
{
    #region DEPENDENCIES
    ActorHealth actorHealth;
    #endregion
    public float delay = 0.025f;
    public int flicksAmount = 2;

    public SpriteRenderer sprite;
    public Material whiteMat;

    Material originalMat;
    private void OnEnable()
    {
        //EventDirector.somebody_TakeDamage += Flash;
        actorHealth.TakingDamage += Flash;
    }

    private void OnDisable()
    {
        //EventDirector.somebody_TakeDamage -= Flash;
        actorHealth.TakingDamage -= Flash;
    }
    void Awake()
    {
        actorHealth = GetComponent<ActorHealth>();
    }
    private void Start()
    {
        originalMat = sprite.material;

    }
    void Flash(float amount, float knockback, Transform byWhom)
    {
        StartCoroutine("FlashingCR");
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
