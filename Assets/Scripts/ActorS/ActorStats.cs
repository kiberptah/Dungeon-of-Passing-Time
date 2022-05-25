using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorStats : MonoBehaviour
{
    //public float maxHealth;
    public float strength = 1f;

    public float sightRadius;

    public enum Factions
    {
        player,
        mage,
        invader,
        witch,

        skeleton,
        slime,
        demon

    }

    public Factions[] belongsToFactions;
    public Factions[] friendedFactions;
    public Factions[] hatedFactions;


}
