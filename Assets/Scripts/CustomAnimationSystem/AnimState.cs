using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class AnimState
{
    public string name;
    public Texture2D spriteSheet;

    [Header("Frames")]
    public int frameLength = 150;
    [Header("Events")]
    public List<string> eventReference = new List<string>();



    [Header("Individual Directions")]
    public int directionsAmount = 8;
    public List<AnimDirection> directions = new List<AnimDirection>();






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
