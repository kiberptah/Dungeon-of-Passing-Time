using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NPCState_Skeleton_Chase : INPCState
{
    NPC_AI npc;

    public void ChangeToThisState(NPC_AI _npc)
    {
        npc = _npc;

        npc.npcController.actor.Input_SheathWeapon();
    }
    public INPCState DoState(NPC_AI _npc)
    {
        if (npc.currentTarget != null)
        {
            return npc.state_skeleton_combat;
        }

        return npc.state_skeleton_idle;
    }

    void WanderAround()
    {

    }

}