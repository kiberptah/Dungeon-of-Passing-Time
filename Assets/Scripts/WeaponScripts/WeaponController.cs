using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 
///  PUT THIS ON ACTOR NOT THE WEAPON
/// </summary>
public class WeaponController : MonoBehaviour
{
    WeaponStatsAlt weaponStats;
    ActorStamina actorStamina;

    [HideInInspector] public GameObject equippedWeapon;
    [HideInInspector] public int weaponSlot;
    public float weaponDistanceFromBody = 1f;
    public float weaponMaxDistanceFromBody = 2f;

    [HideInInspector] public float weaponMinimalFollowSpeed = 1f;
    [HideInInspector] public float weaponFollowSpeedMod = 1f;
    [HideInInspector] public float weaponFollowDeadzone = 1f;
    [HideInInspector] public float weaponRotationSpeed = 1f;
    [HideInInspector] public float weaponSensitivityAngle = 45f;
    [HideInInspector] public float weaponMaxFollowSpeed = 100f;
    [HideInInspector] public float weaponKnockbackModifer = 0;
    [HideInInspector] public float weaponPierceCost = 0;
    [HideInInspector] public Transform weaponLocalHolder;


    [HideInInspector] public float weaponFollowSpeed = 0f;

    Vector3 mousePosition = Vector3.up;
    Vector3 mouseProjection = Vector3.up;
    [HideInInspector] public float weaponDirectionTendency = 0;


    Vector3 weaponVelocity = Vector3.zero;
    Vector3 charVelocity = Vector3.zero;

    public enum attackMode
    {
        slash,
        pierce
    }
    public attackMode currentAttackMode;


    [HideInInspector] public Vector3 frozenMousePosition = Vector3.zero;
    private void Start()
    {
        actorStamina = GetComponent<ActorStamina>();
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
    /// ///////////////////////////////////////////////////////////////////////////////////////
    /// ///////////////////////////////////////////////////////////////////////////////////////
    /// ///////////////////////////////////////////////////////////////////////////////////////



    public void PlaceWeaponInHand(GameObject weapon, int _weaponSlot)
    {
        equippedWeapon = Instantiate(weapon, transform);
        //
        //mouseProjection = transform.InverseTransformPoint(mousePosition); // result is local coordinates!!!
        mouseProjection = mousePosition; // idk it is in local anyway cause weapon is npcs child
        mouseProjection.z = 0;
        mouseProjection = mouseProjection.normalized;
        equippedWeapon.transform.position = transform.TransformPoint(mouseProjection);
        //equippedWeapon.transform.position = transform.TransformPoint(mousePosition);

        weaponFollowSpeed = weaponMinimalFollowSpeed;
        //
        equippedWeapon.SetActive(true);
        weaponSlot = _weaponSlot;

        weaponStats = equippedWeapon.GetComponent<WeaponStatsAlt>();

        weaponMinimalFollowSpeed = weaponStats.weaponMinimalFollowSpeed;
        weaponFollowSpeedMod = weaponStats.weaponFollowSpeedMod;
        weaponFollowDeadzone = weaponStats.weaponFollowDeadzone;
        weaponRotationSpeed = weaponStats.weaponRotationSpeed;
        weaponSensitivityAngle = weaponStats.weaponSensitivityAngle;
        weaponKnockbackModifer = weaponStats.knockbackModifier;
        weaponLocalHolder = weaponStats.weaponLocalHolder;
        weaponPierceCost = weaponStats.pierceStaminaCost;

        weaponMaxFollowSpeed = weaponStats.maxFollowSpeed;
    }

    public void UpdateMousePosition(Vector3 _mousePosition)
    {
        mousePosition = _mousePosition;
    }

    public void WeaponPiercing()
    {
        if (equippedWeapon != null)
        {
            if (weaponPierceCost < actorStamina.currentStamina)
            {
                EventDirector.somebody_LoseStamina(transform, weaponPierceCost);

                weaponStats.pierceCollider.enabled = true;
                weaponStats.slashCollider.enabled = false;
                StartCoroutine("WeaponPiercingCoroutine");
            }
        }
    }

    IEnumerator WeaponPiercingCoroutine()
    {
        //print("pierce crt");
        while (weaponLocalHolder.transform.localPosition.magnitude < weaponMaxDistanceFromBody - weaponDistanceFromBody
            && equippedWeapon != null)
        {
            weaponFollowSpeed = weaponMinimalFollowSpeed;
            //print("moving blade forward");
            Vector3 direction = weaponLocalHolder.transform.localPosition + Vector3.up;


            weaponLocalHolder.transform.localPosition
                = Vector3.MoveTowards(weaponLocalHolder.transform.localPosition, direction, weaponStats.pierceAttackSpeed);

            if (equippedWeapon == null)
            {
                break;
            }
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(weaponStats.pierceHoldTime);

        while (weaponLocalHolder.transform.localPosition.magnitude > 0 && equippedWeapon != null)
        {
            //print("moving blade back");

            Vector3 direction = equippedWeapon.transform.position + equippedWeapon.transform.TransformPoint(Vector3.down);
            direction = weaponLocalHolder.transform.localPosition + Vector3.down;

            weaponLocalHolder.transform.localPosition
                = Vector3.MoveTowards(weaponLocalHolder.transform.localPosition, Vector3.zero, weaponStats.pierceRecoverSpeed);

            if (equippedWeapon == null)
            {
                break;
            }
            yield return new WaitForFixedUpdate();
        }

        if (equippedWeapon != null)
        {
            weaponStats.pierceCollider.enabled = false;
            weaponStats.slashCollider.enabled = true;
        }
        
        yield return null;
    }

    
    void WeaponSwinging()
    {
        Vector3 fakeMousePosition;

        /// --- kinda project  mouse position on a circle around center of the player
        /// in local coordinates!!!
        //mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        mouseProjection = transform.InverseTransformPoint(mousePosition); // result is local coordinates!!!
        mouseProjection.z = 0;
        mouseProjection = mouseProjection.normalized;

        /// --- calculate angle between direction to mouse and the player 
        /// still in local coordinates!!!
        float directionalAngle = Vector3.SignedAngle(equippedWeapon.transform.localPosition, mouseProjection, -transform.forward);
        //print(directionalAngle);

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

        //Debug.Log("weapondir: " + weaponDirectionTendency);

        // // // //
        /// ASSIGN FAKE MOUSE POSITION
        //fakeMousePosition = equippedWeapon.transform.TransformPoint(new Vector3(weaponDirectionTendency * 10f, 0, 0));

        fakeMousePosition = transform.TransformPoint(weaponDirectionTendency * 1f * -Vector2.Perpendicular(equippedWeapon.transform.position - transform.position));


        //Vector3 normal = Vector3.Cross(transform.forward, transform.TransformPoint((equippedWeapon.transform.position - transform.position).normalized));


        Vector3 fakeMouseProjection;
        fakeMouseProjection = transform.InverseTransformPoint(fakeMousePosition).normalized;

        if (Vector3.Distance(equippedWeapon.transform.position, transform.TransformPoint(fakeMouseProjection))
        > Vector3.Distance(equippedWeapon.transform.position, transform.TransformPoint(mouseProjection)))
        {
            if 
                (weaponFollowSpeed < weaponMaxFollowSpeed / 2f)
                //(true)
            //if (true)
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

        /// --- actually move the weapon
        /// IN WORLD COORDINATES otherwise this crap doesnt work
        /// also move only if mouse not in the deadzone
        equippedWeapon.transform.position
                = Vector3.MoveTowards(equippedWeapon.transform.position, fakeMousePosition, weaponFollowSpeed);

        /// --- to set weapon on a fixed distance from the player (or limit how far is max)
        /// 

        equippedWeapon.transform.localPosition = equippedWeapon.transform.localPosition.normalized * weaponDistanceFromBody;
        //equippedWeapon.transform.localPosition = Vector3.ClampMagnitude(equippedWeapon.transform.localPosition, weaponDistanceFromBody);
    }

    Vector3 debugPoint;
    Vector3 debugOrigin;
    Vector3 debugLineEnd;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        //Gizmos.DrawSphere(debugPoint, 0.1f);
        //Gizmos.DrawLine(debugOrigin, debugLineEnd);

        Gizmos.DrawSphere(transform.TransformPoint(mouseProjection), 0.25f);

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





    void BladeCollision(Transform otherObject)
    {
        //if (localObject.gameObject.layer == LayerMask.GetMask("Weapon") && otherObject.gameObject.layer == LayerMask.GetMask("Weapon"))

        //print(" collided with " + otherObject.name);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        BladeCollision(collision.transform);
    }






}


