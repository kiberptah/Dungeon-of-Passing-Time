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

    [SerializeField] SpriteRenderer sprite;
    [SerializeField] Material whiteMat;

    Material originalMat;
    
    #region Init
    void Awake()
    {
        actorHealth = GetComponent<ActorHealth>();
    }
    private void OnEnable()
    {
        actorHealth.TakingDamage += Flash;
    }

    private void OnDisable()
    {
        actorHealth.TakingDamage -= Flash;
    }
    private void Start()
    {
        originalMat = sprite.material;

    }
    #endregion
    
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
