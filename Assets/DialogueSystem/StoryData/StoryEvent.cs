using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using UnityEditor;
using System.Linq;

[Serializable]
public class StoryEvent 
{
    /*
    static StoryEvents storyEvents;
    public StoryEvent()
    {
        storyEvents = Resources.Load<StoryEvents>(path: "GlobalData/StoryEvents");
    }
    */

    public string eventName = "";
    public Action storyEvent = new Action(empty);



    static void empty()
    {

    }
}

