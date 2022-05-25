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

    /*     public void UpdateDirection(AnimDirection _dir)
        {
            int i = 0;
            foreach (var fr in _dir.frames)
            {
                if (frames.Count <= i)
                {
                    frames.Add(new AnimFrame());
                }
                frames[i].sprite = _dir.frames[i].sprite;
            }
        } */
}
