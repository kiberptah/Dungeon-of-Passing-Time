using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NewWeaponStats", menuName = "Weapons/Add Weapon Stats", order = 1)]
public class WeaponStats : ScriptableObject
{
    /* 
    public float testAcceleration = 0.5f;
    [Range(0, 1f)]
    public float testFriction = 0.5f;
    [Range(0, 0.2f)]
    public float testDeadzone = 0.1f;
 */
    [Range(0, 1f)]
    public float friction;
    [Serializable]
    public class SwingStage
    {
        public float acceleration = 0;
        public float deadzone = 0;
        public float velocityThreshold = 0.1f;
    }
    public List<SwingStage> swingStages = new List<SwingStage>();

    /* 
    public class Curve
    {
        public float power = 1.5f;
        public float coefficient = 1;
        public float transitionVelocity = 0.2f;
    }
 */
    [Header("Handling")]
    public float minVelocity = 0;
    public float maxVelocity = 100f;

    //public float accelPower = 1.5f;
    //public float accelCoeff = 1;

    public float angleSpeed = 999f;
    public float sensitivityAngle = 45f;
    public float minDistanceFromBody = 1f;
    /* 

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

        public float piercingDistance = 2f;

     */

    //public List<Curve> accelerationCurves = new List<Curve>();
}