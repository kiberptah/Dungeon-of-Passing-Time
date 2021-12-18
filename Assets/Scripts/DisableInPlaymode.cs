using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableInPlaymode : MonoBehaviour
{
    void Start()
    {
        transform.gameObject.SetActive(false);
    }


}
