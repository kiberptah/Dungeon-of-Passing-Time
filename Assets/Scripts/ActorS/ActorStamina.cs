using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorStamina : MonoBehaviour
{
    ActorStats actorStats;
    [HideInInspector] public float maxStamina;
    [HideInInspector] public float currentStamina;
    [SerializeField] Transform staminaBar;


    float regenPerSecond = 10f;
    private void OnEnable()
    {
        EventDirector.somebody_LoseStamina += SubtractStamina;
        EventDirector.somebody_GainStamina += AddStamina;
    }
    private void OnDisable()
    {
        EventDirector.somebody_LoseStamina -= SubtractStamina;
        EventDirector.somebody_GainStamina -= AddStamina;

    }
    void Start()
    {
        actorStats = GetComponent<ActorStats>();
        maxStamina = actorStats.maxStamina;
        regenPerSecond = actorStats.staminaRegenPerSecond;

        currentStamina = maxStamina;
    }

    // Update is called once per frame
    void Update()
    {
        RegenerateStamina();
    }

    void RegenerateStamina()
    {
        currentStamina += regenPerSecond * Time.deltaTime;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        UpdateHealthBar();
    }

    void SubtractStamina(Transform who, float amount)
    {
        if (who == transform)
        {
            currentStamina -= amount;
            UpdateHealthBar();
        }
    }
    void AddStamina(Transform who, float amount)
    {
        if (who == transform)
        {
            currentStamina += amount;
            UpdateHealthBar();
        }
    }

    void UpdateHealthBar()
    {
        if (staminaBar != null)
        {
            staminaBar.localScale = new Vector3(currentStamina / maxStamina, staminaBar.localScale.y, staminaBar.localScale.z);
        }
    }
}
