using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ActorStamina : MonoBehaviour
{
    public float maxStamina;
    float currentStamina;

    public float regenPerSecond;


    public event Action<float, float> updateStaminaInfo; // current, max

    #region Init
    void Awake()
    {
        currentStamina = maxStamina;
    }
    #endregion

    void Update()
    {
        RegenerateStamina();
    }

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
