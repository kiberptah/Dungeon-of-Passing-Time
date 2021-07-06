using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IHealth
{
    public float maxHealth = 100;

    
    public float currentHealth { get; set; }

    ActorStats npcStats;

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
        npcStats = GetComponent<ActorStats>();
        maxHealth = npcStats.maxHealth;

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

            if (currentHealth == 0)
            {
                EventDirector.somebody_Death(transform, fromWhom);
            }
        }
    }
    public void OnNoHealth(Transform who, Transform killer)
    {
        if (who == transform)
        {
            Destroy(gameObject);
        }
    }

    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.localScale = new Vector3(currentHealth / maxHealth, healthBar.localScale.y, healthBar.localScale.z);
        }
    }
}
