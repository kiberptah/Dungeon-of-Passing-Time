using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.Linq;
using System;
public class AnimPlayer : MonoBehaviour
{
    [SerializeField] SpriteRenderer sprite;
    AnimLib animLib;

    [HideInInspector]
    public List<AnimFrame> sequence;
    [HideInInspector]
    public List<string> events;

    [HideInInspector]
    public int frameLength = 150;

    [HideInInspector]
    public bool isLooping = true;
    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        animLib = GetComponent<AnimLib>();
    }

    int frameCounter = 0;

    IEnumerator PlayAnimation()
    {
        frameCounter = 0;

        while (true)
        {
            if (sequence != null && sequence.Count > 0)
            {
                // Reset counter to loop through list
                if (frameCounter >= sequence.Count)
                {
                    if (!isLooping)
                    {
                        break;
                    }

                    frameCounter = 0;
                }
                // Don't bother changing if it is the same sprite!
                if (sprite != sequence[frameCounter].sprite)
                {
                    // Change Frame
                    sprite.sprite = sequence[frameCounter].sprite;

                    // Fire all events attached to frame
                    /*
                    if (sequence[frameCounter].eventHolder != null)
                    {
                        sequence[frameCounter].eventHolder.events?.Invoke();
                    }
                    */

                    //Debug.Log("events " + events.Count);
                    //Debug.Log("frameCounter " + frameCounter);
                    if (events.Count > frameCounter)
                    {
                        if (events[frameCounter] != string.Empty)
                        {
                            //Debug.Log("animation event");

                            animLib.eventsLib[events[frameCounter]].events?.Invoke();
                            //events[frameCounter].events?.Invoke();
                        }
                    }


                    // Wait for frame length before going to next frame
                    yield return new WaitForSeconds(frameLength * 0.001f - Time.deltaTime); // IDK why but coroutines take aprx one frame longer to play animations!
                    
                    //float timer = frameLength * 0.001f;
                    //while(timer > 0)
                    //{
                    //    yield return new WaitForEndOfFrame(); // IDK why but coroutines take aprx one frame longer to play animations!
                    //    timer -= Time.deltaTime;
                    //}

                    frameCounter++;
                }
            }
            // Important line for not getting unity freezed in a loop...
            yield return null;
        }
    }

    public void UpdateData(List<AnimFrame> _sequence, List<string> _events, int _frameLength, bool _isLooping, bool waitTillSeqEnd = false)
    {
        

        //foreach(var aaa in _events)
        //{
        //    Debug.Log(aaa);

        //}

        if (sequence != _sequence || frameLength != _frameLength || events != _events)
        {
            //List<AnimEvents> eventsTemp = new List<AnimEvents>();
            //eventsTemp = animLib.FindEvents(_events);

            if (waitTillSeqEnd)
            {
                StartCoroutine(UpdateDataAfterSequence(_sequence, _frameLength, _isLooping));
            }
            else
            {
                isLooping = _isLooping;
                sequence = _sequence;
                events = _events;
                frameLength = _frameLength;

                StopCoroutine("PlayAnimation");
                StartCoroutine("PlayAnimation");
            }
        }
    }
    IEnumerator UpdateDataAfterSequence(List<AnimFrame> _sequence, int _frameLength, bool _isLooping)
    {
        while (frameCounter < sequence.Count)
        {
            yield return null;
        }

        isLooping = _isLooping;
        sequence = _sequence;
        frameLength = _frameLength;

        StopCoroutine("PlayAnimation");
        StartCoroutine("PlayAnimation");

        yield return null;
    }
}
