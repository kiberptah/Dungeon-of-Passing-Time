using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "AnimLib_", menuName = "CustomAnimator/AnimLibData", order = 1)]
public class AnimLibData : ScriptableObject
{
    public List<AnimState> states;
    /* 
    public void UpdateState(AnimState _state)
    {
        int i = 0;
        foreach (var st in states)
        {
            if (st.name == _state.name)
            {
                states[i].UpdateState(_state);
            }
            ++i;
        }
    }
 */
    public void test()
    {
        Debug.Log("test");
    }
}
