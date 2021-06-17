using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth 
{
    void OnGettingHealth(Transform who, float amount, Transform fromWhom);
    void OnLoosingHealth(Transform who, float amount, Transform fromWhom);
    void OnNoHealth(Transform who, Transform killer);
}
