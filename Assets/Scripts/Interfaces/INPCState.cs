using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INPCState
{
    INPCState DoState(NPCStateMachine npc);

    void ChangeToThisState(NPCStateMachine npc);
}
