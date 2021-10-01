using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorStamina : MonoBehaviour
{
    ActorStats actorStats;
    [HideInInspector] public float maxStamina;
    [HideInInspector] public float currentStamina;
    [SerializeField] Transform staminaBar;

    public float minimalSpeedMod = .5f;
    public float speedDebuffThreshold = .9f;


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
    private void Update()
    {

        RegenerateStamina();
        ChangeWalkspeedBasedOnStamina();
        /*
        string testa = transform.GetComponent<UnitySucksTest>().unitySux;
        Debug.Log(transform.GetComponent<UnitySucksTest>().unitySux);
        */
    }

    void ChangeWalkspeedBasedOnStamina()
    {
        // it is kinda shitty in terms of compatibility with multiple speed modifiers
        //actorStats.walkSpeed = actorStats.defaultWalkSpeed * minimalSpeedMod + actorStats.defaultWalkSpeed * (1 - minimalSpeedMod) * (currentStamina / maxStamina);

        actorStats.walkspeed_StaminaMod = minimalSpeedMod + (1 - minimalSpeedMod) * (currentStamina / maxStamina);

        if (currentStamina / maxStamina > speedDebuffThreshold)
        {
            actorStats.walkspeed_StaminaMod = 1f;
        }
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
