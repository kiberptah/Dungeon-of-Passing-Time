using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IHealth
{
    public float maxHealth = 100;
    float currentHealth;

    [SerializeField]
    Transform healthBar;
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
        currentHealth = maxHealth;
    }
    private void Start()
    {
        //EventDirector.player_updateHealth(currentHealth);
    }

    public void OnGettingHealth(Transform who, float amount, Transform fromWhom)
    {
        if (who == transform)
        {
            currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
            UpdateHealthBar();
        }
    }
    public void OnLoosingHealth(Transform who, float amount, Transform fromWhom)
    {
        if (who == transform)
        {
            currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
            UpdateHealthBar();
        }
    }
    public void OnNoHealth(Transform who, Transform killer)
    {

    }
    void UpdateHealthBar()
    {
        healthBar.localScale = new Vector3(currentHealth / maxHealth, healthBar.localScale.y, healthBar.localScale.z);
    }
}
