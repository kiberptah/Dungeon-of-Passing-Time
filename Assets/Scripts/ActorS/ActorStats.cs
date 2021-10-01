using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorStats : MonoBehaviour
{
    public enum Factions
    {
        player,
        enemy
    }

    public Factions faction;
    public Factions[] friendlyFactions;
    public Factions[] hatedFactions;

    [Header("Basic Stats")]
    public float maxHealth;
    public float maxStamina;
    public float staminaRegenPerSecond;
    [HideInInspector]
    public float defaultWalkSpeed;
    public float walkSpeed;
    public float sightRadius;
    [Header("Costs")]
    public float dashCost;

    private void Start()
    {
        defaultWalkSpeed = walkSpeed;
    }



    [Header("Modifiers (do not tweak)")]
    public float walkspeed_Modifiers = 1f;
    public float walkspeed_StaminaMod = 1f;

    private void Update()
    {
        walkspeed_Modifiers = walkspeed_StaminaMod;

    }

}
