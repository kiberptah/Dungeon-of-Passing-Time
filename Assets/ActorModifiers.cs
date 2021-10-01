using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorModifiers : MonoBehaviour
{
    public float walkspeed_Modifiers = 1f;
    public float walkspeed_StaminaMod = 1f;

    private void Update()
    {
        walkspeed_Modifiers = walkspeed_StaminaMod;

    }
}
