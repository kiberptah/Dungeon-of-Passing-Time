using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasVisibilityManager : MonoBehaviour
{
    Canvas thisCanvas;

    private void Awake()
    {
        thisCanvas = GetComponent<Canvas>();
    }

    private void OnEnable()
    {
        EventDirector.dialogue_start += disableCanvas;
        EventDirector.dialogue_end += enableCanvas;
    }
    private void OnDisable()
    {
        EventDirector.dialogue_start -= disableCanvas;
        EventDirector.dialogue_end -= enableCanvas;
    }

    void enableCanvas()
    {
        thisCanvas.enabled = true;
    }
    void disableCanvas()
    {
        thisCanvas.enabled = false;
    }
}
