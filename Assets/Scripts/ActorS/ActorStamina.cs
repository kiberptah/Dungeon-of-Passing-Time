using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ActorStamina : MonoBehaviour
{
    public float maxStamina;
    float currentStamina;
    public float regenPerSecond;


    public float minimalSpeedMod = .5f;
    public float speedDebuffThreshold = .9f;

    public event Action<float, float> updateStaminaInfo; // current, max
    //public event Action<float> updateStaminaWalkspeedMod;

    void Awake()
    {
        currentStamina = maxStamina;
    }

    private void Update()
    {

        RegenerateStamina();
        //ChangeWalkspeedBasedOnStamina();
    }

    /* void ChangeWalkspeedBasedOnStamina()
    {
        // it is kinda shitty in terms of compatibility with multiple speed modifiers

        actorStats.walkspeed_StaminaMod = minimalSpeedMod + (1 - minimalSpeedMod) * (currentStamina / maxStamina);

        if (currentStamina / maxStamina > speedDebuffThreshold)
        {
            actorStats.walkspeed_StaminaMod = 1f;
        }
    } */



    public bool TrySubtractingStamina(float amount)
    {
        if (currentStamina >= amount)
        {
            SubtractStamina(amount);
            return true;
        }
        else
        {
            return false;
        }
    }

    #region Stamina Management
    void RegenerateStamina()
    {
        currentStamina += regenPerSecond * Time.deltaTime;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        updateStaminaInfo?.Invoke(currentStamina, maxStamina);
    }

    void SubtractStamina(float amount)
    {
        currentStamina -= amount;
        updateStaminaInfo?.Invoke(currentStamina, maxStamina);
    }
    void AddStamina(float amount)
    {
        currentStamina += amount;
        updateStaminaInfo?.Invoke(currentStamina, maxStamina);
    }
    #endregion


}
