using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// 
///  PUT THIS ON ACTOR NOT THE WEAPON
/// </summary>
public class WeaponController : MonoBehaviour
{
    public event Action<GameObject, Weapon> updateDrawnWeapon;
    public Weapon weaponScript;
    [HideInInspector] public GameObject weaponObject;

    int swingDirection = 1; // -1, 0, 1
    float maxSwingOffset = 1;
    //[HideInInspector] public float weaponFollowSpeed = 0f;
    float swingDuration = 0;
    bool isSwinging = false;
    //float weaponDistanceFromBody;
    public enum attackMode
    {
        slash,
        pierce
    }
    public attackMode currentAttackMode;
    private void Awake()
    {

    }
    private void FixedUpdate()
    {
        if (weaponObject != null)
        {
            WeaponSwinging();
            AdjustWeaponAngle();

            CalculateWeaponVelocity();
        }
    }
    /// ///////////////////////////////////////////////////////////////////////////////////////
    /// ///////////////////////////////////////////////////////////////////////////////////////
    /// ///////////////////////////////////////////////////////////////////////////////////////

    #region Sheath/Draw
    public void PlaceWeaponInHand(GameObject _weapon, Vector2 _position = new Vector2())
    {
        if (_position == Vector2.zero)
        {
            _position = transform.TransformPoint(Vector2.up);
            //Debug.Log("ASD");
        }
        weaponObject = Instantiate(_weapon, transform);
        weaponObject.transform.position = _position;
        weaponObject.transform.eulerAngles = new Vector3(0, 0, 0);

        weaponScript = weaponObject.GetComponent<Weapon>();



        weaponScript.currentVelocity = weaponScript.stats.minVelocity;

        weaponObject.SetActive(true);

        updateDrawnWeapon?.Invoke(weaponObject, weaponScript);
    }
    public void RemoveWeapon()
    {
        Destroy(weaponObject);
    }
    #endregion

    #region Piercing
    public void PiercingAttack()
    {
        if (weaponObject != null)
        {
            EventDirector.somebody_LoseStamina(transform, weaponScript.stats.pierceStaminaCost);

            weaponScript.pierceCollider.enabled = true;
            weaponScript.slashCollider.enabled = false;
            StartCoroutine("WeaponPiercingCoroutine");
        }
    }

    IEnumerator WeaponPiercingCoroutine()
    {
        ResetSwingVelocity();
        // ----- Move weapon forward
        while (weaponScript.localHolder.transform.localPosition.magnitude < weaponScript.stats.weaponMaxDistanceFromBody - weaponScript.stats.weaponMinDistanceFromBody
            && weaponObject != null)
        {
            if (weaponObject == null)
            {
                break;
            }

            weaponScript.currentVelocity = weaponScript.stats.minVelocity;
            Vector3 direction = weaponScript.localHolder.transform.localPosition + Vector3.up;

            weaponScript.localHolder.transform.localPosition
                = Vector3.MoveTowards(weaponScript.localHolder.transform.localPosition, direction, weaponScript.stats.pierceAttackSpeed);


            yield return new WaitForFixedUpdate();
        }


        // ----- Hold it there
        yield return new WaitForSeconds(weaponScript.stats.pierceHoldTime);


        // ----- Pull weapon back
        while (weaponScript.localHolder.transform.localPosition.magnitude > 0
            && weaponObject != null)
        {
            if (weaponObject == null)
            {
                break;
            }

            weaponScript.localHolder.transform.localPosition
                = Vector3.MoveTowards(weaponScript.localHolder.transform.localPosition, Vector3.zero, weaponScript.stats.pierceRecoverSpeed);


            yield return new WaitForFixedUpdate();
        }

        if (weaponObject != null)
        {
            weaponScript.pierceCollider.enabled = false;
            weaponScript.slashCollider.enabled = true;
        }


        ResetSwingVelocity();
        yield return null;
    }
    #endregion



    public void UpdateSwingDirection(int _direction, float _maxSwingOffset = 1)
    {
        if (swingDirection != _direction)
        {
            swingDirection = _direction;
            ResetSwingVelocity();
        }

        maxSwingOffset = _maxSwingOffset;
    }
    /*
    public void UpdateMaxSwingOffset(float _maxSwingOffset)
    {
        maxSwingOffset = _maxSwingOffset;
        
    }
     */

    void WeaponSwinging()
    {
        //Debug.Log("swingDirection = " + swingDirection);
        // --- Project direction vector
        Vector2 swingDirectionProjection = swingDirection * 10 * -Vector2.Perpendicular(weaponObject.transform.position - transform.position);
        swingDirectionProjection = transform.TransformPoint(swingDirectionProjection);

        // --- Calculate max offset to avoid jitter on high speed


        //float maxOffset = Mathf.Clamp(weaponScript.currentVelocity * maxSwingOffset, 0, weaponScript.currentVelocity);
        float maxOffset = Mathf.Clamp(maxSwingOffset, 0, weaponScript.currentVelocity);
        //maxOffset = Mathf.Clamp(maxOffset, 0, maxSwingOffset);
        if (maxOffset < weaponScript.currentVelocity)
        {
            //Debug.Log("swing vel reset" + Time.time);
            ResetSwingVelocity();
        }

        // --- Actually move the weapon

        weaponObject.transform.position
                = Vector3.MoveTowards(weaponObject.transform.position, swingDirectionProjection, maxOffset);

        // --- Set weapon on a fixed distance from the player
        weaponObject.transform.localPosition = weaponObject.transform.localPosition.normalized * weaponScript.stats.weaponMinDistanceFromBody;

        //
        if (swingDirection == 0)
        {

            //ResetSwingVelocity();
            isSwinging = false;
        }
        else
        {
            isSwinging = true;
        }
    }
    void AdjustWeaponAngle()
    {
        float weaponDirAngle = Vector3.SignedAngle(weaponObject.transform.up, weaponObject.transform.localPosition, Vector3.forward);
        weaponObject.transform.Rotate(0, 0, weaponDirAngle * weaponScript.stats.weaponRotationSpeed);
    }




    void ResetSwingVelocity()
    {
        swingDuration = 0;

        weaponScript.ResetSwing();

    }


    void CalculateWeaponVelocity()
    {
        // Calculate which curve should be used at current velocity
        WeaponStats.Curve currentCurve = weaponScript.stats.accelerationCurves[weaponScript.currentCurve];
        if (weaponScript.currentVelocity >= currentCurve.transitionVelocity)
        {
            if (weaponScript.stats.accelerationCurves.Count > weaponScript.currentCurve + 1)
            {
                ++weaponScript.currentCurve;
                weaponScript.functionXOffset = swingDuration;
                weaponScript.functionYOffset = currentCurve.transitionVelocity;
            }
        }

        // Calculate Velocity
        weaponScript.currentVelocity =
            currentCurve.coefficient
            * Mathf.Pow(swingDuration - weaponScript.functionXOffset, currentCurve.power)
            + weaponScript.functionYOffset;

        // Limit velocity by min and max
        weaponScript.currentVelocity = Mathf.Clamp(weaponScript.currentVelocity, weaponScript.stats.minVelocity, weaponScript.stats.maxVelocity);

        // Calculate DAMAGE based on velocity
        weaponScript.currentDamage = weaponScript.stats.slashDamage * (weaponScript.currentVelocity - weaponScript.stats.minVelocity);

        // Increment swing duration
        if (isSwinging)
        {
            swingDuration += Time.deltaTime;
        }
    }


}


