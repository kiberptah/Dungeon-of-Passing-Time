using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventDirector : MonoBehaviour
{
    public static Action scene_Reset; // reset current scene
    public static Action dialogue_start; // starts the dialogue
    public static Action<Dialogue> dialogue; // content of the dialogue
    public static Action dialogue_end; // ends the dialogue

    public static Action<Transform, float, Transform> somebody_TookDamage; // who, amout, from whom  
    public static Action player_TookDamage;
    public static Action player_DealtDamage;
    public static Action player_Death;
}
