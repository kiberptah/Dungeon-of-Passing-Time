using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerWeaponController : MonoBehaviour
{
    public GameObject equippedWeapon;
    WeaponStatsAlt weaponStats;
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
    

    Vector3 mouseProjection = Vector3.up;


    //float weaponVelocity;
    //float charVelocity;
    Vector3 weaponVelocity = Vector3.zero;
    Vector3 charVelocity = Vector3.zero;

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
        Vector3 fakeMousePosition;

        /// --- kinda project  mouse position on a circle around center of the player
        /// in local coordinates!!!
        Vector3 mousePosition = Vector3.zero;
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        mouseProjection = transform.InverseTransformPoint(mousePosition); // result is local coordinates!!!
        mouseProjection.z = 0;
        mouseProjection = mouseProjection.normalized;

        /// --- calculate angle between direction to mouse and the player 
        /// still in local coordinates!!!
        float directionalAngle = Vector3.SignedAngle(equippedWeapon.transform.localPosition, mouseProjection, -transform.forward);
        //print(directionalAngle);

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


        ///
        ///
        /*
        // convert into world space from equippedweapon space
        Vector3 weaponDirectionTendencyProjection = equippedWeapon.transform.TransformPoint(new Vector3(weaponDirectionTendency * 1f, 0, 0));
        Vector3 mouseDirectionWorldSpace = transform.TransformPoint(mouseDirection);

        if (Vector3.Distance(equippedWeapon.transform.position, weaponDirectionTendencyProjection) 
            > Vector3.Distance(equippedWeapon.transform.position, mouseDirectionWorldSpace))
        {
            fakeMousePosition = transform.TransformPoint(mouseDirection);
            //print("close follow mode");
        }
        else
        {
            fakeMousePosition = equippedWeapon.transform.TransformPoint(new Vector3(weaponDirectionTendency * weaponFollowSpeed, 0, 0));
            //print("far follow mode");

        }
        */
        if (true)
        {
            fakeMousePosition = equippedWeapon.transform.TransformPoint(new Vector3(weaponDirectionTendency * 10f, 0, 0));

            Vector3 fakeMouseProjection;
            fakeMouseProjection = transform.InverseTransformPoint(fakeMousePosition).normalized;

            if (Vector3.Distance(equippedWeapon.transform.position, transform.TransformPoint(fakeMouseProjection))
            > Vector3.Distance(equippedWeapon.transform.position, transform.TransformPoint(mouseProjection)))
            {
                //if (weaponFollowSpeed < weaponMaxFollowSpeed / 2f)
                if (true)
                {
                    /// there's a a thing 
                    /// when you stop swinging blade still has high speed and it snaps to mouse in the end
                    /// and does extra damage
                    /// it can be fixed probably but idk how rn (UPDATE: BY IF STATEMENT ABOVE) --- if (weaponFollowSpeed < weaponMaxFollowSpeed / 2f)
                    /// but it can be a cool feature to have last swing deal extra damage
                    fakeMousePosition = transform.TransformPoint(mouseProjection);
                    //weaponFollowSpeed = weaponMinimalFollowSpeed;

                }


            }
        }
        else
        {
            fakeMousePosition = transform.TransformPoint(mouseProjection);
            //weaponFollowSpeed = weaponMinimalFollowSpeed;
        }

       // fakeMousePosition = transform.TransformPoint(mouseDirection);

        /*
        float angle = (weaponDirectionTendency * 360f * weaponFollowSpeed)
            / (2 * Mathf.PI * weaponDistanceFromBody);
        float x = equippedWeapon.transform.localPosition.x * Mathf.Cos(angle) - equippedWeapon.transform.localPosition.y * Mathf.Sin(angle);
        float y = equippedWeapon.transform.localPosition.x * Mathf.Sin(angle) + equippedWeapon.transform.localPosition.y * Mathf.Cos(angle);
        fakeMousePosition = transform.TransformPoint(new Vector3(x, y, 0));
        debugPoint = fakeMousePosition;

        print(angle);
        */

        /* //amazing glitches
        float angle = (weaponDirectionTendency * 180f * weaponFollowSpeed) 
            / (Mathf.PI * weaponDistanceFromBody);
        transform.RotateAround(mouseDirection, transform.position, angle);
        */
        /*
        float angle = (weaponDirectionTendency * 180f * weaponFollowSpeed)
            / (Mathf.PI * weaponDistanceFromBody);
        equippedWeapon.transform.RotateAround(transform.position, -transform.forward, angle);
        */
        /// --- actually move the weapon
        /// IN WORLD COORDINATES otherwise this crap doesnt work
        /// also move only if mouse not in the deadzone
        if (true)
        {
            //debugPoint = fakeMousePosition;
            //print("mov");
            //print(fakeMousePosition);
            //print(weaponFollowSpeed);

            equippedWeapon.transform.position
                = Vector3.MoveTowards(equippedWeapon.transform.position, fakeMousePosition, weaponFollowSpeed);
        }
        /// --- to set weapon on a fixed distance from the player (or limit how far is max)
        equippedWeapon.transform.localPosition = equippedWeapon.transform.localPosition.normalized * weaponDistanceFromBody;
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
        //float mouseDirAngle = Vector3.Angle(Vector3.up, mouseDirection);
        float weaponDirAngle = Vector3.SignedAngle(equippedWeapon.transform.up, equippedWeapon.transform.localPosition, Vector3.forward);
        equippedWeapon.transform.Rotate(0, 0, weaponDirAngle * weaponRotationSpeed);

        //equippedWeapon.transform.rotation = new Quaternion(0, 0, 0, 0);
    }
    void AdjustFollowSpeedAccordingToCurrentVelocity()
    {
        if (weaponFollowSpeedMod != 0)
        {
            /// --- basically a function for acceleration
            /// 
            ///
            if (weaponVelocity.magnitude == 0)
            {
                weaponFollowSpeed = weaponMinimalFollowSpeed;
                return;
            }

            weaponFollowSpeed -= weaponMinimalFollowSpeed;

            //weaponFollowSpeed = weaponFollowSpeed + weaponFollowSpeedMod * weaponVelocity;
            weaponFollowSpeed = weaponFollowSpeed + weaponFollowSpeedMod * Mathf.Sqrt(weaponVelocity.magnitude);
            //print("follow speed: " + weaponFollowSpeed);
            //print("weaponVelocity: " + weaponVelocity);



            weaponFollowSpeed = Mathf.Clamp(weaponFollowSpeed, 0, weaponMaxFollowSpeed);
            weaponFollowSpeed += weaponMinimalFollowSpeed;
        }
        else
        {
            weaponFollowSpeed = weaponMinimalFollowSpeed;
        }
    }

    public void PlaceWeaponInHand(GameObject weapon, int _weaponSlot)
    {
        equippedWeapon = Instantiate(weapon, transform);
        equippedWeapon.SetActive(true);
        weaponSlot = _weaponSlot;

        weaponStats = weapon.GetComponent<WeaponStatsAlt>();

        weaponMinimalFollowSpeed = weaponStats.weaponMinimalFollowSpeed;
        weaponFollowSpeedMod = weaponStats.weaponFollowSpeedMod;
        weaponFollowDeadzone = weaponStats.weaponFollowDeadzone;
        weaponRotationSpeed = weaponStats.weaponRotationSpeed;
        weaponSensitivityAngle = weaponStats.weaponSensitivityAngle;

        weaponMaxFollowSpeed = weaponStats.maxFollowSpeed;
    }

    void Inputs()
    {
        if (equippedWeapon != null)
        {
           
        }
    }


    Vector3 previousCharacterWorldPosition;
    Vector3 previousWeaponLocalPosition;
    void CalculateBladeVelocity()
    {
        if (equippedWeapon != null)
        {
            if (previousCharacterWorldPosition != null && previousWeaponLocalPosition != null)
            {
                charVelocity = transform.position - previousCharacterWorldPosition;
                weaponVelocity = equippedWeapon.transform.localPosition - previousWeaponLocalPosition;

                //print("world velocity: " + charVelocity);
                //print("weapon velocty: " + weaponVelocity);
            }

            previousCharacterWorldPosition = transform.position;
            previousWeaponLocalPosition = equippedWeapon.transform.localPosition;
        }
    }
    void AssignSpeedToTheBlade()
    {
        if (equippedWeapon != null)
        {
            EventDirector.someBladeUpdateVelocity(equippedWeapon.transform, weaponVelocity, charVelocity);
            //print("welocity: " + weaponVelocity);
        }
    }


}
