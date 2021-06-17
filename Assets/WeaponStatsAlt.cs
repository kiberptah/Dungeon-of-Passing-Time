using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStatsAlt : MonoBehaviour
{
    [Header("Damage")]
    public float damage;
    public float velocityDamageModifier = 1f;
    //int typeOfDamageScaling = 0;
    public enum damageCalculationTypes
    {
        no,
        sword1,
        sword2,
        sword3,
        dagger
    }
    public damageCalculationTypes damageCalcType;
    //public float damageVelocityOffset = 10f;
    public float damageTickDelay;

    public enum typeOfDamage
    {
        physical,
        magical
    }
    public typeOfDamage damageType;
    [Header("Handling")]
    public float weaponMinimalFollowSpeed = 0;
    public float weaponFollowSpeedMod = 0;
    public float weaponFollowDeadzone = 0;
    public float weaponRotationSpeed = 0;
    public float weaponSensitivityAngle = 90f;
    public float maxFollowSpeed = 100f;

    [Header("etc")]
    public Collider2D bladeCollider;

    public enum classOfWeapon
    {
        sword,
        dagger
    }
    public classOfWeapon weaponClass;
}
