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
    public float walkSpeed;
    public float sightRadius;
    [Header("Costs")]
    public float dashCost;

}
