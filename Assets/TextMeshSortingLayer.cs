using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextMeshSortingLayer : MonoBehaviour
{
    public string sortingLayer = "UI";

    void Start()
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        renderer.sortingLayerName = sortingLayer;
    }

}
