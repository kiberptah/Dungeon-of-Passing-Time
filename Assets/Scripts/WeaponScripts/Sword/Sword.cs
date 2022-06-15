using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Sword : MonoBehaviour
{
    public SwordStats swordStats;
    [HideInInspector] public WeaponStats weaponStats;

    [Header("Slashing")]
    public Transform slashCollider;
    public float currentSlashDamage = 0;

    [Header("Piercing")]
    bool isPiercing = false;
    public Transform pierceCollider;
    public Transform localHolder; // required to separate slashing and piercing moving logic by using different coord systems

    [Header("etc")]

    [HideInInspector] public WeaponConnector connector;

    [HideInInspector] public Weapon weaponScript;
    [HideInInspector] public GameObject weaponObject;
    void Awake()
    {
        connector = GetComponent<WeaponConnector>();
        connector.specialAttackCost = swordStats.pierceStaminaCost;

        weaponObject = gameObject;
        weaponScript = GetComponent<Weapon>();
        weaponStats = weaponScript.stats;

    }


    void OnEnable()
    {
        connector.Special_Attack_Start += PiercingAttack;
    }
    void OnDisable()
    {
        connector.Special_Attack_Start -= PiercingAttack;
    }

    void FixedUpdate()
    {
        CalculateSlashDamage();
    }
    void CalculateSlashDamage()
    {
        #region Damage
        // Calculate DAMAGE based on velocity
        currentSlashDamage = swordStats.slashDamage * (Mathf.Abs(weaponScript.totalVelocity) - weaponScript.stats.minVelocity);
        //Debug.Log("currentDamage " + currentDamage);
        if (currentSlashDamage < 0)
        {
            currentSlashDamage = 0;
        }
        #endregion
    }

    #region Piercing
    public void PiercingAttack()
    {
        if (weaponObject != null && !isPiercing)
        {
            //actorStamina.SubtractStamina(weaponScript.stats.pierceStaminaCost);
            connector.SubtractStamina(swordStats.pierceStaminaCost);

            slashCollider.gameObject.SetActive(false);
            pierceCollider.gameObject.SetActive(true);
            StartCoroutine("WeaponPiercingCoroutine");
        }
    }

    IEnumerator WeaponPiercingCoroutine()
    {
        isPiercing = true;
        weaponScript.accelerationBlocked = true;

        //ResetSwingVelocity();
        // ----- Move weapon forward
        while (localHolder.transform.localPosition.magnitude < weaponStats.minDistanceFromBody * swordStats.piercingDistance - weaponStats.minDistanceFromBody
            && weaponObject != null)
        {
            if (weaponObject == null)
            {
                break;
            }

            //weaponScript.currentVelocity = weaponScript.stats.minVelocity;
            Vector3 direction = localHolder.transform.localPosition + Vector3.forward;

            localHolder.transform.localPosition
                = Vector3.MoveTowards(localHolder.transform.localPosition, direction, swordStats.pierceAttackSpeed);


            yield return new WaitForFixedUpdate();
        }


        // ----- Hold it there
        yield return new WaitForSeconds(swordStats.pierceHoldTime);


        // ----- Pull weapon back

        if (weaponObject != null)
        {
            pierceCollider.gameObject.SetActive(false);
            slashCollider.gameObject.SetActive(true);
        }

        while (localHolder.transform.localPosition.magnitude > 0
            && weaponObject != null)
        {
            if (weaponObject == null)
            {
                break;
            }

            localHolder.transform.localPosition
                = Vector3.MoveTowards(localHolder.transform.localPosition, Vector3.zero, swordStats.pierceRecoverSpeed);


            yield return new WaitForFixedUpdate();
        }




        //ResetSwingVelocity();
        isPiercing = false;
        weaponScript.accelerationBlocked = false;

        yield return null;
    }
    #endregion

}
