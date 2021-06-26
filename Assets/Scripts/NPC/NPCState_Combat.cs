using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCState_Combat : INPCState
{
    NPCStateMachine npc;

    public void ChangeToThisState(NPCStateMachine _npc)
    {
        npc = _npc;
        npc.npcMovement.InitiateMovementToCurrentTarget();

        npc.npcWeaponManager.Input_DrawWeapon();
    }
    public INPCState DoState(NPCStateMachine _npc)
    {
        npc.npcWeaponController.weaponDirectionTendency = 1f;



        if (npc.EyeContactWithTarget(npc.currentTarget) == false)
        {
            npc.npcWeaponManager.Input_SheathWeapon();
            Debug.Log("lost eye contact!");
            npc.npcMovement.StopAllCoroutines();
            return npc.chaseState;
        }
        return npc.combatState;
    }

    void ComeCloser()
    {

    }


    
}
