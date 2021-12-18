using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu]
[Serializable]
public class StoryEventsContainer : ScriptableObject
{
    public static string storyEventsPath = "StoryData/StoryEvents";

    public List<StoryEvent> eventsList = new List<StoryEvent>();

    public static void FireEvent(List<StoryEvent> _eventsList, string _eventName)
    {
        int? number = FindEventNumberInList(_eventsList, _eventName);
        if (number != null)
        {
            _eventsList[(int)number].storyEvent?.Invoke();
        }
    }

    public static int FindEventNumberInList(List<StoryEvent> _eventsList, string _eventName)
    {
        for (int i = 0; i < _eventsList.Count; ++i)
        {
            if (_eventsList[i].eventName == _eventName)
            {
                return i;
            }
        }

        return -999999;
    }

}