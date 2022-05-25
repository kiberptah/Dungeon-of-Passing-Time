using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Weapon : MonoBehaviour
{

    WeaponConnector connector;
    public WeaponStats stats;


    [HideInInspector] public float totalVelocity = 0;

    int swingDirection = 0; // -1, 0, 1
    float maxSwingOffset = 1;
    float swingDuration = 0;
    [HideInInspector] public bool accelerationBlocked = false;


    #region Initialization and stuff
    void OnEnable()
    {
        connector.Update_Swing_Direction += UpdateSwingDirection;
    }
    void OnDisable()
    {
        connector.Update_Swing_Direction -= UpdateSwingDirection;
    }
    void Awake()
    {
        connector = GetComponent<WeaponConnector>();
    }
    public void Initialize()
    {
        // Initialize velocity on weapon spawn
        totalVelocity = stats.minVelocity;

        // Correct Position to avoid visual artifacts
        transform.localPosition = transform.localPosition.normalized * stats.minDistanceFromBody;

        // Set up correct rotation
        float weaponDirAngle = Vector3.SignedAngle(transform.up, transform.localPosition, Vector3.forward);
        transform.Rotate(0, 0, weaponDirAngle);

        gameObject.SetActive(true);
    }
    #endregion

    private void FixedUpdate()
    {
        if (accelerationBlocked)
        {
            ResetSwing();
        }
        CalculateWeaponVelocity();
        WeaponSwinging();
        AdjustWeaponAngle();
    }

    void UpdateSwingDirection(int _direction, float _maxSwingOffset = 1)
    {
        // Updates with Action trigger
        if (swingDirection != _direction)
        {
            ResetSwing();
        }
        swingDirection = _direction;
        maxSwingOffset = _maxSwingOffset;
    }


    #region Swing & Angle
    void WeaponSwinging()
    {
        // --- Project direction vector
        Vector2 swingProjection = 10 * -Vector2.Perpendicular(transform.position - connector.weaponHolder.position);
        swingProjection = connector.weaponHolder.TransformPoint(swingProjection);

        // --- Actually move the weapon
        transform.position
                = Vector3.MoveTowards(transform.position, swingProjection, totalVelocity);

        // --- Set weapon on a fixed distance from the player
        transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y);
        transform.localPosition = transform.localPosition.normalized * stats.minDistanceFromBody;
    }
    void AdjustWeaponAngle()
    {
        float threshhold = 2f;
        float weaponDirAngle = Vector3.SignedAngle(transform.up, transform.localPosition, Vector3.forward);

        if (Mathf.Abs(weaponDirAngle) > threshhold)
        {
            transform.Rotate(0, 0, Mathf.Sign(weaponDirAngle) * Mathf.Clamp(stats.angleSpeed, 0, Mathf.Abs(weaponDirAngle)));
        }
    }
    #endregion


    float CalculateTotalVelocity(float _totalVelocity)
    {
        if (stats.swingStages.Count < 1)
        {
            return 0;
        }
        #region Stage
        int stageCounter = 0;

        // Calculate which curve should be used at current velocity
        foreach (var stage in stats.swingStages)
        {
            if (Mathf.Abs(_totalVelocity) >= stage.velocityThreshold)
            {
                if (stats.swingStages.Count > stageCounter + 1)
                {
                    ++stageCounter;
                }
            }
        }
        WeaponStats.SwingStage currentStage = stats.swingStages[stageCounter];
        //Debug.Log("currentCurveNumber " + stageCounter);

        #endregion

        #region Velocity
        //Debug.Log("swingDirection " + swingDirection);
        float accelCoeff = 1;
        float actorStrength = connector.actor.stats.strength;

        if (maxSwingOffset < currentStage.deadzone && swingDuration < 0.5f)
        {
            accelCoeff = maxSwingOffset;
            //Debug.Log("snappy");
        }
        float totalAcceleration = actorStrength * accelCoeff * currentStage.acceleration * swingDuration * swingDirection;
        float totalFriction = stats.friction;

        // ACCELERATION
        //Debug.Log("totalAcceleration " + totalAcceleration);
        _totalVelocity += totalAcceleration;
        // FRICTION
        if (totalAcceleration == 0)
        {
            //Debug.Log("friction " + _totalVelocity * totalFriction);
            _totalVelocity -= _totalVelocity * totalFriction;
        }

        // CLAMP
        _totalVelocity = Mathf.Clamp(_totalVelocity, -stats.maxVelocity, stats.maxVelocity);
        //Debug.Log("totalVelocity " + _totalVelocity);

        #endregion

        return _totalVelocity;
    }

    void CalculateWeaponVelocity()
    {

        #region Velocity
        if (!accelerationBlocked)
        {
            totalVelocity = CalculateTotalVelocity(totalVelocity);
        }
        else
        {
            totalVelocity = stats.minVelocity * swingDirection;
        }
        #endregion

        #region Timer
        if (swingDirection != 0)
        {
            swingDuration += Time.fixedDeltaTime;
        }
        #endregion

    }

    void ResetSwing()
    {
        swingDuration = 0;
        //Debug.Log("ResetSwing ");
    }


}