using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentScaleCompensator : MonoBehaviour
{
    private void LateUpdate()
    {
        transform.rotation = Quaternion.identity;
    }
}
