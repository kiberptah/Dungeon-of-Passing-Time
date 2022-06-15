using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public class AnimFrame
{
    //public AnimEvents eventHolder;
    public string frameName;
    public Sprite sprite;
    public AnimFrame()
    {

    }
    public AnimFrame(Sprite _sprite)
    {
        sprite = _sprite;
    }
}
