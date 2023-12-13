using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Pin))]
public class PinEditor : Editor
{
    private Pin pin;
    private SerializedProperty pinnedParent;

    private void OnEnable()
    {
        pin = (Pin)target;
        pinnedParent = serializedObject.FindProperty("pinnedParent");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Move to Pinned Parent"))
        {
            pin.transform.position = ((Transform)pinnedParent.objectReferenceValue).position;
        }
    }
}
