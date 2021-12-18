using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorDead : MonoBehaviour
{

    public ActorAnimationManager.spriteDirection currentSpriteDirection = ActorAnimationManager.spriteDirection.S;
    public ActorAnimationManager.spriteAction currentSpriteAction = ActorAnimationManager.spriteAction.dead;



    private void Start()
    {
        EventDirector.somebody_UpdateSpriteDirection(transform, currentSpriteDirection);
        EventDirector.somebody_UpdateSpriteAction(transform, ActorAnimationManager.spriteAction.dead);
    }
}
