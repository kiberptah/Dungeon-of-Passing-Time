using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug_ObjectDuplicator : MonoBehaviour
{
    public GameObject prefab;

    public int howMany = 1;

    Vector3 pos;
    private void Start()
    {
        pos = transform.position;

        for (int i = 0; i < howMany; i++)
        {
            GameObject go = Instantiate(prefab, transform);
            go.transform.position = pos;

            pos += Vector3.forward;

        }
    }
    private void Update()
    {
        
    }
}
