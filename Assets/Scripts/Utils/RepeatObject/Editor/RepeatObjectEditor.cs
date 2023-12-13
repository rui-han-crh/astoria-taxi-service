using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Drawing.Printing;

/// <summary>
/// Repeats the object in the scene hierarchy along an axis.
/// 
/// Offsets can be specified to offset the repeated objects
/// </summary>
[CustomEditor(typeof(RepeatObject)), System.Serializable]
public class RepeatObjectEditor : Editor
{
    private RepeatObject repeatObject;

    private void OnEnable()
    {
        repeatObject = (RepeatObject)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        // Only when the values change, update the repeated objects
        if (GUI.changed || GUILayout.Button("Update Repeated Objects"))
        {
            // Update the repeated objects
            repeatObject.UpdateRepeatedObjects();

            // Mark the scene as dirty
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }

        // Apply the changes to the serialized object
        serializedObject.ApplyModifiedProperties();
    }
}
