using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class TestEventListener : MonoBehaviour
{
    public StoryEventsContainer storyEvents;// = Resources.Load<StoryEvents>(StoryEvents.storyEventsPath);

    private void OnEnable()
    {
        storyEvents.eventsList[StoryEventsContainer.FindEventNumberInList(storyEvents.eventsList, "Open Door A4")].storyEvent += listnere1;
        storyEvents.eventsList[StoryEventsContainer.FindEventNumberInList(storyEvents.eventsList, "make some noise")].storyEvent += listnere2;
        storyEvents.eventsList[StoryEventsContainer.FindEventNumberInList(storyEvents.eventsList, "Increase REputation with NPC2")].storyEvent += listnere3;
    }
    private void Start()
    {
        StoryEventsContainer.FireEvent(storyEvents.eventsList, "test");

    }


    void listnere1()
    {
        Debug.Log("listner");
    }
    void listnere2()
    {
        Debug.Log("listner2");
    }
    void listnere3()
    {
        Debug.Log("listner3");
    }

}
