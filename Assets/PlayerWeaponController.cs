using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerWeaponController : MonoBehaviour
{
    public GameObject equippedWeapon;
    public int weaponSlot;
    public float weaponDistanceFromBody = 1.25f;
    //float weaponAttackRange;
    bool isAttackInCooldown;
    [Header("Weapon Alignment")]

    float weaponMinimalFollowSpeed = 1f;
    float weaponFollowSpeedMod = 1f;
    float weaponFollowDeadzone = 1f;
    float weaponRotationSpeed = 1f;
    float weaponSensitivityAngle = 45f;
    float weaponMaxFollowSpeed = 100f;


    float weaponFollowSpeed = 0f;
    

    Vector3 mouseDirection = Vector3.up;


    Vector3 previousWeaponPosition;
    float weaponVelocity;

    float weaponDirectionTendency = 0;

    private void Awake()
    {
        
    }

    private void Update()
    {

        /*
        if (equippedWeapon != null)
        {
            AdjustWeaponPosition();
            AdjustWeaponAngle();
        }
        */
        //Inputs();
    }

    private void FixedUpdate()
    {
        if (equippedWeapon != null)
        {
            WeaponSwinging();
            AdjustWeaponAngle();

            CalculateBladeVelocity();
            AssignSpeedToTheBlade();
            AdjustFollowSpeedAccordingToCurrentVelocity();
        }

        
    }
    void WeaponSwinging()
    {
        /// --- kinda project  mouse position on a circle around center of the player
        /// in local coordinates!!!
        Vector3 mousePosition = Vector3.zero;
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //mouseDirection = mousePosition - transform.position; // result is local coordinates!!!
        mouseDirection = transform.InverseTransformPoint(mousePosition); // result is local coordinates!!!
        mouseDirection.z = 0;
        mouseDirection = mouseDirection.normalized;

        /// --- calculate angle between direction to mouse and the player 
        /// still in local coordinates!!!
        float directionalAngle = Vector3.SignedAngle(equippedWeapon.transform.localPosition, mouseDirection, -transform.forward);
        /// check if angle bigger then a deadzone cause we loosing exact value in the next lines and need to know it later
        bool isAngleNotInTheDeadzone = false;
        if (Mathf.Abs(directionalAngle) > weaponFollowDeadzone)
        {
            isAngleNotInTheDeadzone = true;
        }
        
        /// choose direction to move weapon towards, represented by fake mouse cursor 
        /// now working in world coordinates!!! i dont know why but this fucken shit doesnt work in local coordinates after now!
        /// 
        if (Mathf.Abs(directionalAngle) < weaponSensitivityAngle) // if angle is higher it probably means we are swinging and we won;t need to change direction
        {
            directionalAngle = Mathf.Sign(directionalAngle);
            
            if (weaponDirectionTendency != directionalAngle)
            {
                /// reset gained speed when changing direction
                weaponFollowSpeed = weaponMinimalFollowSpeed; 
            }
            weaponDirectionTendency = directionalAngle;
        }
        Vector3 fakeMousePosition;
        fakeMousePosition = equippedWeapon.transform.TransformPoint(new Vector3(weaponDirectionTendency * 1f, 0, 0));

        /// --- actually move the weapon
        /// IN WORLD COORDINATES otherwise this crap doesnt work
        /// also move only if mouse not in the deadzone
        if (isAngleNotInTheDeadzone)
        {
            print("mov");
            //print(fakeMousePosition);
            //print(weaponFollowSpeed);
            equippedWeapon.transform.position
                = Vector3.MoveTowards(equippedWeapon.transform.position, fakeMousePosition, weaponFollowSpeed);
        }
        /// --- to set weapon on a fixed distance from the player (or limit how far is max)
        equippedWeapon.transform.localPosition = equippedWeapon.transform.localPosition.normalized;
        //equippedWeapon.transform.localPosition = Vector3.ClampMagnitude(equippedWeapon.transform.localPosition, weaponDistanceFromBody);
    }


    Vector3 debugPoint;
    Vector3 debugOrigin;
    Vector3 debugLineEnd;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(debugPoint, 0.1f);
        Gizmos.DrawLine(debugOrigin, debugLineEnd);

    }
    void AdjustWeaponAngle()
    {
        float mouseDirAngle = Vector3.Angle(Vector3.up, mouseDirection);
        float weaponDirAngle = Vector3.SignedAngle(equippedWeapon.transform.up, equippedWeapon.transform.localPosition, Vector3.forward);
        equippedWeapon.transform.Rotate(0, 0, weaponDirAngle * weaponRotationSpeed);

        //equippedWeapon.transform.rotation = new Quaternion(0, 0, 0, 0);
    }
    void AdjustFollowSpeedAccordingToCurrentVelocity()
    {
        /// --- basically a function for acceleration
        /// 
        ///
        if (weaponVelocity == 0)
        {
            weaponFollowSpeed = 0;
            return;
        }

        weaponFollowSpeed -= weaponMinimalFollowSpeed;

        //weaponFollowSpeed = weaponFollowSpeed + weaponFollowSpeedMod * weaponVelocity;
        weaponFollowSpeed = weaponFollowSpeed + weaponFollowSpeedMod * Mathf.Sqrt(weaponVelocity);
        //print("follow speed: " + weaponFollowSpeed);
        //print("weaponVelocity: " + weaponVelocity);



        weaponFollowSpeed = Mathf.Clamp(weaponFollowSpeed, 0, weaponMaxFollowSpeed);
        weaponFollowSpeed += weaponMinimalFollowSpeed;
    }

    public void PlaceWeaponInHand(GameObject weapon, int _weaponSlot)
    {
        equippedWeapon = Instantiate(weapon, transform);
        equippedWeapon.SetActive(true);
        weaponSlot = _weaponSlot;

        WeaponStatsAlt weaponstats = weapon.GetComponent<WeaponStatsAlt>();

        weaponMinimalFollowSpeed = weaponstats.weaponMinimalFollowSpeed;
        weaponFollowSpeedMod = weaponstats.weaponFollowSpeedMod;
        weaponFollowDeadzone = weaponstats.weaponFollowDeadzone;
        weaponRotationSpeed = weaponstats.weaponRotationSpeed;
        weaponSensitivityAngle = weaponstats.weaponSensitivityAngle;

        weaponMaxFollowSpeed = weaponstats.maxFollowSpeed;
    }

    void Inputs()
    {
        if (equippedWeapon != null)
        {
           
        }
    }

    void TryToAttackWithTheWeapon(WeaponStats.Attack attack)
    {
        if (!isAttackInCooldown)
        {
            EventDirector.someBladeAttackStarted(equippedWeapon.transform, attack);

            //print("too fast!");

            StartCoroutine(AttackCooldownTimer(attack));
        }
    }
    IEnumerator AttackCooldownTimer(WeaponStats.Attack attack)
    {
        isAttackInCooldown = true;
        yield return new WaitForSeconds(attack.coolDownSeconds);
        isAttackInCooldown = false;


        yield return null;
    }

    float maxRecordedVelocity = 0;
    void CalculateBladeVelocity()
    {
        if (Input.GetButtonDown("Jump"))
        {
            maxRecordedVelocity = 0;
        }
        if (equippedWeapon != null)
        {
            if (previousWeaponPosition != null)
            {
                weaponVelocity = Vector3.Magnitude(equippedWeapon.transform.localPosition - previousWeaponPosition);
                //print("weapon velocity: " + weaponVelocity);
                if (weaponVelocity > maxRecordedVelocity)
                {
                    print("max velocity: " + weaponVelocity);
                    maxRecordedVelocity = weaponVelocity;
                }
            }
            previousWeaponPosition = equippedWeapon.transform.localPosition;
        }
    }
    void AssignSpeedToTheBlade()
    {
        if (equippedWeapon != null)
        {
            EventDirector.someBladeUpdateVelocity(equippedWeapon.transform, weaponVelocity);
        }
    }


}
