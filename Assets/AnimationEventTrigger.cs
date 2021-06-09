using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventTrigger : MonoBehaviour
{

    public void FinishAttack()
    {
        EventDirector.someBladeAttackFinished?.Invoke(transform.parent);
        //print(transform.parent);
    }
}
