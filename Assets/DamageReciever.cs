using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageReciever : MonoBehaviour
{
    public IHealth health;

    void Awake()
    {
        transform.parent.TryGetComponent<IHealth>(out health);
    }
}
