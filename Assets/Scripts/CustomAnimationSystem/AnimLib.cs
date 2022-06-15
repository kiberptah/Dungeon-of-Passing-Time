using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
public class AnimLib : MonoBehaviour
{
    public AnimLibData libData;

    public Dictionary<string, AnimEvents> eventsLib = new Dictionary<string, AnimEvents>();
    public List<AnimEvents> animEvents = new List<AnimEvents>();
    private void Awake()
    {
        foreach (var ev in animEvents)
        {
            eventsLib.Add(ev.eventName, ev);
        }
    }
    List<AnimEvents> FindEvents(List<string> names)
    {
        List<AnimEvents> events = new List<AnimEvents>();
        foreach (var n in names)
        {
            foreach (var e in animEvents)
            {
                if (e.eventName == n)
                {
                    events.Add(e);
                    //Debug.Log("found " + e.eventName);
                    break;
                }
                events.Add(null);
            }
        }

        return events;
    }



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

    public List<string> FindStateEvents(string stateName)
    {
        foreach (var state in libData.states)
        {
            if (state.name.ToLower() == stateName.ToLower())
            {
                return state.eventReference;
            }
        }

        return null;
    }

}
