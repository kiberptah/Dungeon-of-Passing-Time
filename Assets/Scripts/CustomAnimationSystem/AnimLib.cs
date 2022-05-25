using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
public class AnimLib : MonoBehaviour
{
    public AnimLibData libData;




    public List<AnimFrame> FindSequence(string stateName, string directionName = "S")
    {
        foreach (var state in libData.states)
        {
            if (state.name.ToLower() == stateName.ToLower())
            {
                //Debug.Log("SUCC");
                return state.FindSequence(directionName);
            }
        }

        return null;
    }
    public int FindStateFrameLength(string stateName)
    {
        foreach (var state in libData.states)
        {
            if (state.name.ToLower() == stateName.ToLower())
            {
                return state.frameLength;
            }
        }

        return 0;
    }

}
