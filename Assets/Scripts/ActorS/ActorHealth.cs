using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class ActorHealth : MonoBehaviour
{
    ActorControllerConnector conntollerConnector;
    ActorStats actorStats;

    public event Action <float, float> updateHealthInfo; // current, max

    public float maxHealth = 100;
    public float currentHealth;

    /*
    [SerializeField]
    Transform healthBar;
    */
    void OnEnable()
    {
        EventDirector.somebody_Heal += OnGettingHealth;
        EventDirector.somebody_TakeDamage += OnLoosingHealth;
        EventDirector.somebody_Death += OnNoHealth;


    }
    private void OnDisable()
    {
        EventDirector.somebody_Heal -= OnGettingHealth;
        EventDirector.somebody_TakeDamage -= OnLoosingHealth;
        EventDirector.somebody_Death -= OnNoHealth;

    }
    void Awake()
    {
        actorStats = GetComponent<ActorStats>();

        maxHealth = actorStats.maxHealth;
        currentHealth = maxHealth;
    }
    public void OnGettingHealth(Transform who, float amount, Transform fromWhom)
    {
        if (who == transform)
        {
            currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
            updateHealthInfo?.Invoke(currentHealth, maxHealth);
            //UpdateHealthBar();
        }
    }
    public void OnLoosingHealth(Transform who, float amount, Transform fromWhom)
    {
        if (who == transform)
        {
            currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
            updateHealthInfo?.Invoke(currentHealth, maxHealth);
            //UpdateHealthBar();
        }
    }
    public void OnNoHealth(Transform who, Transform killer)
    {

    }

    
}
