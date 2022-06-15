using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMine : MonoBehaviour
{
    public ActorAnimManager animManager;

    public void TriggerExplosion()
    {
        animManager.UpdateAnimation("Explosion", null, false, true);
    }
}
