using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INPCState
{
    INPCState DoState(NPC_AI npc);

    void ChangeToThisState(NPC_AI npc);
}
