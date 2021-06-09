using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStatsAlt : MonoBehaviour
{
    public float damage;
    public float velocityModifier = 1f;
    public float damageTickDelay;

    public enum typeOfDamage
    {
        physical,
        magical
    }
    public typeOfDamage damageType;

    public Collider2D bladeCollider;

}
