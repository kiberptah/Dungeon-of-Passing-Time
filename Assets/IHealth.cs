using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth 
{
    void OnGettingHealth(Transform who, float amount);
    void OnLoosingHealth(Transform who, float amount);
    void OnNoHealth(Transform who);
}
