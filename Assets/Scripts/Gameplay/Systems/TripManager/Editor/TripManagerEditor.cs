using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TripManager))]
public class TripManagerEditor : Editor
{
    private TripManager tripManager;
    private SerializedProperty taxi;
    private SerializedProperty fareManager;

    public void OnEnable()
    {
        tripManager = (TripManager)target;

        taxi = serializedObject.FindProperty("taxi");
        fareManager = serializedObject.FindProperty("fareManager");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Autofill Fields"))
        {
            taxi.objectReferenceValue = FindComponent<PlayerTaxi>();
            fareManager.objectReferenceValue = FindComponent<FareManager>();
        }

        serializedObject.ApplyModifiedProperties();
    }

    private Component FindComponent<T>() where T : Component
    {
        T[] components = FindObjectsOfType<T>();

        if (components.Length == 0)
        {
            throw new System.Exception("No component of type " + typeof(T).Name + " was found in the scene.");
        }

        if (components.Length > 1)
        {
            Debug.Log("There was more than one component found, using first found component. Please make sure there is only one component of type " + typeof(T).Name + " in the scene.");
        }

        return components[0];
    }
}
