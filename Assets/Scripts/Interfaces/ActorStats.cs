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

    public float maxHealth;
    public float walkSpeed;
    public float sightRadius;

}
