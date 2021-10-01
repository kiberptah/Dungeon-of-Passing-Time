using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorStaminaNew : MonoBehaviour
{
    public ActorStats actorStats;
    public ActorModifiers actorModifiers;
    [HideInInspector] public float maxStamina;
    [HideInInspector] public float currentStamina;
    [SerializeField] Transform staminaBar;

    public float minimalSpeedMod = .5f;


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
        //actorModifiers = GetComponent<ActorModifiers>();
        //TryGetComponent(out actorModifiers);
        maxStamina = actorStats.maxStamina;
        regenPerSecond = actorStats.staminaRegenPerSecond;

        currentStamina = maxStamina;
    }

    // Update is called once per frame
    private void Update()
    {

        RegenerateStamina();
        ChangeWalkspeedBasedOnStamina();

        string testa = transform.GetComponent<UnitySucksTest>().unitySux;
        Debug.Log(transform.GetComponent<UnitySucksTest>().unitySux);
    }

    void ChangeWalkspeedBasedOnStamina()
    {
        // it is kinda shitty in terms of compatibility with multiple speed modifiers
        //actorStats.walkSpeed = actorStats.defaultWalkSpeed * minimalSpeedMod + actorStats.defaultWalkSpeed * (1 - minimalSpeedMod) * (currentStamina / maxStamina);

        //actorModifiers.walkspeed_StaminaMod = minimalSpeedMod + (1 - minimalSpeedMod) * (currentStamina / maxStamina);


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
