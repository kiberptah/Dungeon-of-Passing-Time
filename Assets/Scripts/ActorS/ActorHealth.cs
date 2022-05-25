using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class ActorHealth : MonoBehaviour, IHealth
{
    [SerializeField] Transform actorHealthBar;

    public event Action<float, float, Transform> TakingDamage; // damage, knockback, fromwhom
    public event Action<float, float> updateHealthInfo; // current, max
    public event Action<float, float, Transform> noHealth;

    public float maxHealth = 100f;
    float currentHealth;

    public float MaxHealth
    {
        get { return maxHealth; }
    }
    public float CurrentHealth
    {
        get { return currentHealth; }
    }


    void OnEnable()
    {

    }
    private void OnDisable()
    {

    }
    void Awake()
    {
        currentHealth = maxHealth;
    }





    public void TakeDamage(float amount, float knockback, Transform fromWhom)
    {
        EventDirector.somebody_TookDamage?.Invoke(transform, amount, fromWhom);


        OnLoosingHealth(amount, knockback, fromWhom);
        TakingDamage?.Invoke(amount, knockback, fromWhom);
    }


    void OnGettingHealth(Transform who, float amount, Transform fromWhom)
    {
        if (who == transform)
        {
            currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
            //updateHealthInfo?.Invoke(currentHealth, maxHealth);
            UpdateHealthBar();
        }
    }
    void OnLoosingHealth(float amount, float kb, Transform fromWhom)
    {
        currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
        //updateHealthInfo?.Invoke(currentHealth, maxHealth);
        UpdateHealthBar();

        if (currentHealth == 0)
        {
            OnNoHealth(amount, kb, fromWhom);
        }
    }


    public void OnNoHealth(float dmg, float kb, Transform killer)
    {
        noHealth?.Invoke(dmg, kb, killer);
    }

    void UpdateHealthBar()
    {
        if (actorHealthBar != null)
        {
            actorHealthBar.localScale
                = new Vector2(currentHealth / maxHealth,
                    actorHealthBar.localScale.y);
        }
    }
}
