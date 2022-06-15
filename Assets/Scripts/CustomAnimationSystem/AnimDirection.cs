using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnimDirection
{
    public directions name;
    public enum directions
    {
        S,
        SW,
        W,
        NW,
        N,
        NE,
        E,
        SE
    }
    public List<AnimFrame> frames = new List<AnimFrame>();
}
