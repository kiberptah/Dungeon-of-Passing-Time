using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class AnimState
{

    //public UnityEvent testEvent;
    public string name;
    public Texture2D spriteSheet;

    [Header("Frames")]
    //public int framesPerDirection = 1;
    public int frameLength = 150;

    [Header("Individual Directions")]
    public int directionsAmount = 8;
    public List<AnimDirection> directions = new List<AnimDirection>();
    /* 
    public void UpdateState(AnimState _state)
    {
        name = _state.name;
        spriteSheet = _state.spriteSheet;
        framesPerDirection = _state.framesPerDirection;
        frameLength = _state.frameLength;

        int i = 0;
        foreach (var dir in _state.directions)
        {
            if (directions.Count <= i)
            {
                directions.Add(new AnimDirection());
            }
            directions[i].UpdateDirection(_state.directions[i]);
        }
    }
 */
    public List<AnimFrame> FindSequence(string directionName)
    {
        foreach (var dir in directions)
        {
            //Debug.Log("dir " + dir.name.ToString() + "|searching " + directionName);

            if (dir.name.ToString().ToLower() == directionName.ToLower())
            {
                return dir.frames;
            }
        }

        // return, well, at lest something if anim lacks directions
        return directions[0].frames;
    }
}
