using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerBehaviour))]
public class TaxiEditor : Editor
{
    private SerializedProperty carriage;
    private SerializedProperty horses;

    private void OnEnable()
    {
        carriage = serializedObject.FindProperty("carriage");
        horses = serializedObject.FindProperty("horses");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Autofill Fields"))
        {
            carriage.objectReferenceValue = FindGameObjectComponent<CollisionEventEmitter>(Tags.CarriageBody);
            horses.objectReferenceValue = FindGameObjectComponent<CollisionEventEmitter>(Tags.Horses);
        }

        serializedObject.ApplyModifiedProperties();
    }

    private T FindGameObjectComponent<T>(string tag) where T : Component
    {
        GameObject found = GameObject.FindGameObjectWithTag(tag);
        if (found != null)
        {
            return found.GetComponent<T>();
        }
        else
        {
            Debug.LogWarning($"Could not find any GameObject with tag {tag}.");
            return default;
        }
    }
}
