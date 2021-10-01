using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCState_Combat : INPCState
{
    NPCStateMachine npc;

    int swingDirection;
    Vector3 npcMousePosition;

    float timer = 0;
    float ChangeDirectionTimer = 0;
    float PierceTimer = 0;
    float PiercingInterval = 2f;
    float ChangeDirectionInterval = 1f;

    float timeWithThisTactic = 0;
    float TimeLimitForThisTactic = 5f;

    enum tactics
    {
        longSwings,
        shorterSwings,
        pierce
    }
    tactics currentTactic;

    public void ChangeToThisState(NPCStateMachine _npc)
    {
        npc = _npc;

        swingDirection = (int)(Mathf.Round(Random.value) * 2f - 1f);
        //npc.npcWeaponController.UpdateMousePosition(Vector3.left);

        npc.npcWeaponManager.Input_DrawWeapon();

        ChooseRandomTactic();
    }
    public INPCState DoState(NPCStateMachine _npc)
    {
        
        if (npc.EyeContactWithTarget(npc.currentTarget) == false)
        {
            npc.npcWeaponManager.Input_SheathWeapon();
            //Debug.Log("lost eye contact!");
            npc.npcMovement.StopAllCoroutines();
            return npc.chaseState;
        }

        npc.npcMovement.UpdateMovementDestination(npc.currentTarget.position);

        /*
        if (timer > 1f)
        {
            ChooseRandomTactic();
            timer = 0;
        }
        */
        switch(currentTactic)
        {
            case tactics.longSwings:
                swingDirection = -1;
                ChangeDirectionInterval = 3f;

                //timeWithThisTactic = 0;
                TimeLimitForThisTactic = 5f;

                AttackPattern_LongSwings();
                break;

            case tactics.shorterSwings:
                swingDirection = 1;
                ChangeDirectionInterval = 1f;

                //timeWithThisTactic = 0;
                TimeLimitForThisTactic = 5f;

                AttackPattern_LongSwings();
                break;

            case tactics.pierce:

                //timeWithThisTactic = 0;
                TimeLimitForThisTactic = 7f;

                AttackPattern_PierceEnemy();
                break;
        }



        

        timer += Time.deltaTime;
        return npc.combatState;
    }


    void ChooseRandomTactic()
    {
        Debug.Log("rolling for randon tactic");
        int rand = Mathf.RoundToInt(Random.Range(1, 4));

        switch (rand)
        {
            default:
                currentTactic = tactics.longSwings;
                break;
            case 1:
                currentTactic = tactics.longSwings;
                break;
            case 2:
                currentTactic = tactics.shorterSwings;
                break;

            case 3:
                currentTactic = tactics.pierce;
                break;
            case 4:
                currentTactic = tactics.pierce;
                break;
        }
        Debug.Log(currentTactic);

    }

    void AttackPattern_LongSwings()
    {
        npcMousePosition = npc.npcWeaponController.equippedWeapon.transform.TransformPoint(new Vector3(swingDirection * 10f, 0, 0));
        npc.npcWeaponController.UpdateMousePosition(npcMousePosition);


        if (ChangeDirectionTimer >= ChangeDirectionInterval)
        {
            //swingDirection = (int)(Mathf.Round(Random.value) * 2f - 1f);
            Debug.Log("swingDirection was " + swingDirection);
            swingDirection = -swingDirection;
            ChangeDirectionTimer = 0;

            Debug.Log("swingDirection is " + swingDirection);
        }
        ChangeDirectionTimer += Time.deltaTime;

        if (timeWithThisTactic > TimeLimitForThisTactic)
        {
            timeWithThisTactic = 0;
            ChooseRandomTactic();
        }
        timeWithThisTactic += Time.deltaTime;
        //Debug.Log("ChangeDirectionTimer: " + ChangeDirectionTimer);
        //Debug.Log("timeWithThisTactic: " + timeWithThisTactic);
    }


    void AttackPattern_ShortSwings()
    {
        float maxAngle = 80f;
        Vector3 targetProjection = npc.transform.InverseTransformPoint(npc.currentTarget.position).normalized;
        Vector3 bladeProjection = npc.transform.InverseTransformPoint(npc.npcWeaponController.equippedWeapon.transform.position).normalized;

        float angleBetweenBladeAndEnemy = Vector3.SignedAngle(bladeProjection, targetProjection, -npc.transform.forward);
        if (angleBetweenBladeAndEnemy > maxAngle)
        {
            swingDirection = 1;
            //Debug.Log(">>>");
        }
        if (angleBetweenBladeAndEnemy < -maxAngle)
        {
            swingDirection = -1;
            //Debug.Log("<<<");
        }
        //Debug.Log("angleBetweenBladeAndEnemy: " + angleBetweenBladeAndEnemy);
        npcMousePosition = npc.npcWeaponController.equippedWeapon.transform.TransformPoint(new Vector3(swingDirection * 10f, 0, 0));
        npc.npcWeaponController.UpdateMousePosition(npcMousePosition);

        npc.shittymouseccord = npcMousePosition;

    }


    void AttackPattern_PierceEnemy()
    {
        float thresholdAngle = 90f; // angle after which enemy changes piercing to swinging
        float maxAngle = 10f;
        float minAngle = 1f;
        Vector3 targetProjection = npc.transform.InverseTransformPoint(npc.currentTarget.position).normalized;
        Vector3 bladeProjection = npc.transform.InverseTransformPoint(npc.npcWeaponController.equippedWeapon.transform.position).normalized;

        float angleBetweenBladeAndEnemy = Vector3.SignedAngle(bladeProjection, targetProjection, -npc.transform.forward);

        if (Mathf.Abs(angleBetweenBladeAndEnemy) > thresholdAngle)
        {
            //currentTactic = tactics.longSwings; // or maybe rand
        }


        if (angleBetweenBladeAndEnemy > maxAngle)
        {
            swingDirection = 1;
        }
        if (angleBetweenBladeAndEnemy < -maxAngle)
        {
            swingDirection = -1;
        }
        if (Mathf.Abs(angleBetweenBladeAndEnemy) <= minAngle)
        {
            swingDirection = 0;
        }

        if (PierceTimer >= PiercingInterval)
        {
            npc.npcWeaponController.WeaponPiercing();

            PierceTimer = 0;
        }
        



        //Debug.Log("angleBetweenBladeAndEnemy: " + angleBetweenBladeAndEnemy);
        npcMousePosition = npc.npcWeaponController.equippedWeapon.transform.TransformPoint(new Vector3(swingDirection * 10f, 0, 0));
        npc.npcWeaponController.UpdateMousePosition(npcMousePosition);

        npc.shittymouseccord = npcMousePosition;

        PierceTimer += Time.deltaTime;

        timeWithThisTactic += Time.deltaTime;
        if (timeWithThisTactic > TimeLimitForThisTactic)
        {
            timeWithThisTactic = 0;
            ChooseRandomTactic();
        }
    }

    
}
