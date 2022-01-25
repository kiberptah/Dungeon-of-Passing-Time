using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NewWeaponStats", menuName = "Add Weapon Stats", order = 1)]
public class WeaponStats : ScriptableObject
{

    [Serializable]
    public class Curve
    {
        public float power = 1.5f;
        public float coefficient = 1;
        public float transitionVelocity = 0.2f;
    }

    [Header("Damage")]
    public damageTypes damageType;
    public enum damageTypes
    {
        physical,
        magical
    }
    [Header("Slashing")]
    public float slashDamage;
    public float slashDamageTickDelay;

    public float velocityDamageModifier = 1f;
    public float slashKnockbackModifier = 1f;


    [Header("Piercing")]
    public float pierceDamage;
    public float pierceDamageTickDelay;
    public float pierceKnockbackModifier = 1f;

    public float pierceStaminaCost = 30f;

    public float pierceAttackSpeed;
    public float pierceHoldTime;
    public float pierceRecoverSpeed;

    public float weaponMaxDistanceFromBody = 2f;

    [Header("Handling")]
    public float minVelocity = 0;
    public float maxVelocity = 100f;

    //public float accelPower = 1.5f;
    //public float accelCoeff = 1;

    public float weaponRotationSpeed = 0;
    public float weaponSensitivityAngle = 90f;
    public float weaponMinDistanceFromBody = 1f;

    public List<Curve> accelerationCurves = new List<Curve>();
}