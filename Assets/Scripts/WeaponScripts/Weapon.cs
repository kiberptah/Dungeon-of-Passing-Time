using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Weapon : MonoBehaviour
{
    public WeaponStats stats;

    public float currentVelocity = 0;
    public float currentDamage = 0;

    public int currentCurve = 0;
    public float functionXOffset = 0;
    public float functionYOffset = 0;
    public void ResetSwing()
    {
        currentVelocity = 0;
        currentCurve = 0;
        functionXOffset = 0;
        functionYOffset = 0;
    }



    [Header("Dependencies")]
    public Collider2D slashCollider;
    public Collider2D pierceCollider;
    public Transform localHolder; // required to separate slashing and piercing moving logic by using different coord systems


}