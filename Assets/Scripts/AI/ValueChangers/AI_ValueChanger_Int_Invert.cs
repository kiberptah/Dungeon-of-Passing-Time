using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/ValueChanger/Int_Invert")]
public class AI_ValueChanger_Int_Invert : AI_ValueChanger_Int
{
    public override void ChangeValue(AI_ValueData data)
    {
        data.intValue = -data.intValue;
    }
}
