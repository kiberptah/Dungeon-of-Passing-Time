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
    }
    public INPCState DoState(NPCStateMachine _npc)
    {




        if (npc.EyeContactWithTarget(npc.currentTarget) == false)
        {
            npc.npcMovement.StopAllCoroutines();
            return npc.chaseState;
        }
        return npc.combatState;
    }

    void ComeCloser()
    {

    }


    
}
