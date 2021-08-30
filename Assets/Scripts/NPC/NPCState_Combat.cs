using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCState_Combat : INPCState
{
    NPCStateMachine npc;

    int dir;
    Vector3 npcMousePosition;

    enum tactics
    {
        randomSwing,
        swingAtEnemy,
        pierce
    }
    tactics currentTactic;

    public void ChangeToThisState(NPCStateMachine _npc)
    {
        npc = _npc;


        //npc.npcMovement.InitiateMovementToCurrentTarget();
        npc.npcWeaponManager.Input_DrawWeapon();


        dir = (int)(Mathf.Round(Random.value) * 2f - 1f);
    }
    public INPCState DoState(NPCStateMachine _npc)
    {
        npc.npcMovement.UpdateMovementDestination(npc.currentTarget.position);
        /*
        float diceRoll;
        diceRoll = Random.Range(0, 100f);
        switch(diceRoll)
        {
            default:
                currentTactic = tactics.randomSwing;
                break;
            case float n when (n > 70f):
                currentTactic = tactics.randomSwing;
                break;
            case float n when (n < 70f):
                currentTactic = tactics.swingAtEnemy;
                break;
        }
        */

        /*
        switch (currentTactic)
        {
            default:
                break;

            case tactics.swingAtEnemy:
                AttackPattern_SwingAtEnemy();
                break;

            case tactics.randomSwing:
                AttackPattern_RandomSwinging();
                break;

            case tactics.pierce:
                break;
        }

        */
        AttackPattern_SwingAtEnemy();


        if (npc.EyeContactWithTarget(npc.currentTarget) == false)
        {
            npc.npcWeaponManager.Input_SheathWeapon();
            //Debug.Log("lost eye contact!");
            npc.npcMovement.StopAllCoroutines();
            return npc.chaseState;
        }
        return npc.combatState;
    }

    void ComeCloser()
    {

    }

    void CountTime(float time)
    {
        
    }

    float randomSwingTimer = 0;
    float randomSwingInterwal = 1f;
    void AttackPattern_RandomSwinging()
    {
        if (randomSwingTimer == 0)
        {
            dir = (int)(Mathf.Round(Random.value) * 2f - 1f);

            //Debug.Log("update mouse position");
        }

        npcMousePosition = npc.npcWeaponController.equippedWeapon.transform.TransformPoint(new Vector3(dir * 10f, 0, 0));
        npc.npcWeaponController.UpdateMousePosition(npcMousePosition);


        randomSwingTimer += Time.deltaTime;
        if (randomSwingTimer >= randomSwingInterwal)
        {
            randomSwingTimer = 0;
        }
    }


    void AttackPattern_SwingAtEnemy()
    {
        float maxAngle = 80f;
        Vector3 targetProjection = npc.transform.InverseTransformPoint(npc.currentTarget.position).normalized;
        Vector3 bladeProjection = npc.transform.InverseTransformPoint(npc.npcWeaponController.equippedWeapon.transform.position).normalized;

        float angleBetweenBladeAndEnemy = Vector3.SignedAngle(bladeProjection, targetProjection, -npc.transform.forward);
        if (angleBetweenBladeAndEnemy > maxAngle)
        {
            dir = 1;
            //Debug.Log(">>>");
        }
        if (angleBetweenBladeAndEnemy < -maxAngle)
        {
            dir = -1;
            //Debug.Log("<<<");
        }
        //Debug.Log("angleBetweenBladeAndEnemy: " + angleBetweenBladeAndEnemy);
        npcMousePosition = npc.npcWeaponController.equippedWeapon.transform.TransformPoint(new Vector3(dir * 10f, 0, 0));
        npc.npcWeaponController.UpdateMousePosition(npcMousePosition);

        npc.shittymouseccord = npcMousePosition;

    }

    void AttackPattern_PierceEnemy()
    {

    }

    
}
