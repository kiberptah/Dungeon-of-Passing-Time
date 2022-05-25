using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AnimLib))]
public class CustomAnimator : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Edit Animations"))
        {
            CustomAnimatorWindow.Open((AnimLib)target);
        }
        base.DrawDefaultInspector();

    }
}
