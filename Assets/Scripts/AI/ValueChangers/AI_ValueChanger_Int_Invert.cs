using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/ValueChanger/Int_Invert")]
public class AI_ValueChanger_Int_Invert : AI_ValueChanger_Int
{
    protected override void ChangeValue_Int(AI_StateData stateData, AI_ValueData data)
    {
        data.intValue = -data.intValue;

    }
}
