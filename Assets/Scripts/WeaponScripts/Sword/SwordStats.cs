using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "NewSwordStats", menuName = "Weapons/Add Sword Stats", order = 1)]
public class SwordStats : ScriptableObject
{
    [Header("Slashing")]
    public float slashDamage;
    public float slashDamageTickDelay = 0.25f;

    public float velocityDamageModifier = 1f;
    public float slashKnockbackModifier = 20f;


    [Header("Piercing")]
    public float pierceDamage;
    public float pierceDamageTickDelay = 0.5f;
    public float pierceKnockbackModifier = 10f;

    public float pierceStaminaCost = 30f;

    public float pierceAttackSpeed = 0.2f;
    public float pierceHoldTime = 0.25f;
    public float pierceRecoverSpeed = 0.03f;

    public float piercingDistance = 2f;


}
