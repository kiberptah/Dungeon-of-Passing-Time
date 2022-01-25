using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ActorStamina : MonoBehaviour
{
    ActorStats actorStats;
    float maxStamina;
    public float currentStamina;
    float regenPerSecond;


    public float minimalSpeedMod = .5f;
    public float speedDebuffThreshold = .9f;

    public event Action<float, float> updateStaminaInfo; // current, max
    public event Action<float> updateStaminaWalkspeedMod;
    
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
    }

    void ChangeWalkspeedBasedOnStamina()
    {
        // it is kinda shitty in terms of compatibility with multiple speed modifiers

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
        updateStaminaInfo?.Invoke(currentStamina, maxStamina);
    }

    void SubtractStamina(Transform who, float amount)
    {
        if (who == transform)
        {
            currentStamina -= amount;
            updateStaminaInfo?.Invoke(currentStamina, maxStamina);
        }
    }
    void AddStamina(Transform who, float amount)
    {
        if (who == transform)
        {
            currentStamina += amount;
            updateStaminaInfo?.Invoke(currentStamina, maxStamina);
        }
    }


}
