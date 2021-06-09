using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    public GameObject equippedWeapon;
    public int weaponSlot;
    public float weaponDistanceFromBody = 1.25f;
    //float weaponAttackRange;
    bool isAttackInCooldown;
    [Header("Weapon Alignment")]
    public float weaponMinimalFollowSpeed = 1f;
    public float weaponFollowSpeedMod = 1f;
    public float weaponFollowDeadzone = 1f;
    float weaponFollowSpeed = 0f;
    public float weaponRotationSpeed = 1f;

    Vector3 mouseDirection = Vector3.up;


    Vector3 previousWeaponPosition;
    float weaponVelocity;


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
            AdjustWeaponPosition();
            AdjustWeaponAngle();

            CalculateBladeVelocity();
            AssignSpeedToTheBlade();
            AdjustFollowSpeedAccordingToCurrentVelocity();
        }

        
    }

    void AdjustWeaponPosition()
    {
        /// --- kinda project  mouse position on a circle around center of the player
        /// in local coordinates!!!
        Vector3 mousePosition = Vector3.zero;
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
     
        mouseDirection = mousePosition - transform.position; // result is local coordinates!!!
        mouseDirection.z = 0;
        mouseDirection = mouseDirection.normalized;

        /// --- calculate angle between direction to mouse and the player 
        /// still in local coordinates!!!
        float directionalAngle = -Vector3.SignedAngle(equippedWeapon.transform.localPosition, mouseDirection, transform.forward);
       
        /// choose direction to move weapon towards, represented by fake mouse cursor 
        /// now working in world coordinates!!! i dont know why but this fucken shit doesnt work in local coordinates after now!
        Vector3 fakeMousePosition;
        fakeMousePosition = equippedWeapon.transform.TransformPoint(new Vector3(Mathf.Clamp(directionalAngle, -1f, 1f), 0, 0));

        /// --- actually move the weapon
        /// IN WORLD COORDINATES otherwise this crap doesnt work
        /// also move only if mouse not in the deadzone
        if (Mathf.Abs(directionalAngle) > Mathf.Abs(weaponFollowDeadzone))
        {
            equippedWeapon.transform.position 
                = Vector3.MoveTowards(equippedWeapon.transform.position, fakeMousePosition, weaponFollowSpeed);
        }


        /// --- to set weapon on a fixed distance from the player (or limit how far is max)
        equippedWeapon.transform.localPosition = equippedWeapon.transform.localPosition.normalized;
        //equippedWeapon.transform.localPosition = Vector3.ClampMagnitude(equippedWeapon.transform.localPosition, weaponDistanceFromBody);
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
        weaponFollowSpeed = weaponMinimalFollowSpeed + weaponFollowSpeedMod * weaponVelocity;
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


    void CalculateBladeVelocity()
    {
        if (equippedWeapon != null)
        {
            if (previousWeaponPosition != null)
            {
                weaponVelocity = Vector3.Magnitude(equippedWeapon.transform.localPosition - previousWeaponPosition);
                print("weapon velocity: " + weaponVelocity);
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
