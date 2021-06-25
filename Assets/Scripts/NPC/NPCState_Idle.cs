using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCState_Idle : INPCState
{
    NPCStateMachine npc;
    
    public void ChangeToThisState(NPCStateMachine _npc)
    {
        npc = _npc;


    }
    public INPCState DoState(NPCStateMachine _npc)
    {


        npc.LookForEnemies();
        if (npc.currentTarget != null)
        {
            return npc.combatState;
        }

        return npc.idleState;
    }

    void WanderAround()
    {

    }


    
}
