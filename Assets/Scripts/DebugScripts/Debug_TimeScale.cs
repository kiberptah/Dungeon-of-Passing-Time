using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug_TimeScale : MonoBehaviour
{
    public float timescale = 1f;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = timescale;
    }

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = timescale;
    }
}
