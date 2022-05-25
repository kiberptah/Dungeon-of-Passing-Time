using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.Linq;
using System;
public class AnimPlayer : MonoBehaviour
{
    [SerializeField] SpriteRenderer sprite;

    [HideInInspector]
    public List<AnimFrame> sequence;

    [HideInInspector]
    public int frameLength = 150;

    [HideInInspector]
    public bool isLooping = true;
    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
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
                    if (sequence[frameCounter].eventHolder != null)
                    {
                        sequence[frameCounter].eventHolder.events?.Invoke();
                    }

                    // Wait for frame length before going to next frame
                    yield return new WaitForSeconds(frameLength * 0.001f - Time.deltaTime); // IDK why but coroutines take aprx one frame longer to play animations!

                    frameCounter++;
                }
            }
            // Important line for not getting unity freezed in a loop...
            yield return null;
        }
    }

    public void UpdateData(List<AnimFrame> _sequence, int _frameLength, bool _isLooping, bool waitTillSeqEnd = false)
    {
        if (sequence != _sequence || frameLength != _frameLength)
        {
            if (waitTillSeqEnd)
            {
                StartCoroutine(UpdateDataAfterSequence(_sequence, _frameLength, _isLooping));
            }
            else
            {
                isLooping = _isLooping;
                sequence = _sequence;
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
