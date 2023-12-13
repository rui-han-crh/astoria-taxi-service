using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RepeatObject : MonoBehaviour
{
    [SerializeField]
    private Vector3 offset = Vector3.zero;

    [SerializeField]
    private int repeatCount = 0;

    [SerializeField, HideInInspector]
    private GameObject[] repeatedObjects = new GameObject[0];

    [SerializeField, HideInInspector]
    private int repeatedObjectsCount = 0;

    private void OnValidate()
    {
        // Clamp the repeat count to 0 or greater
        repeatCount = Mathf.Max(0, repeatCount);
    }

    public void UpdateRepeatedObjects()
    {
        int difference = repeatCount - repeatedObjectsCount;

        if (difference > 0)
        {
            for (int i = 0; i < difference; i++)
            {
                ArrayUtility.Add(ref repeatedObjects, CreateRepeatedObject(repeatedObjectsCount));
                repeatedObjectsCount++;
            }
        }
        else if (difference < 0)
        {
            for (int i = 0; i < -difference; i++)
            {
                DestroyImmediate(repeatedObjects[repeatedObjectsCount - 1]);
                ArrayUtility.RemoveAt(ref repeatedObjects, repeatedObjectsCount - 1);
                repeatedObjectsCount--;
            }
        }

        // For every repeated object, update the position
        for (int i = 0; i < repeatedObjectsCount; i++)
        {
            repeatedObjects[i].transform.position = transform.position + offset * (i + 1);

            // Check if the repeated object has all the components of the repeat object
            foreach (Component component in GetComponents<Component>())
            {
                // If the component is a transform, skip it
                if (component is Transform)
                {
                    continue;
                }

                // If the component is a repeat object, skip it
                if (component is RepeatObject)
                {
                    continue;
                }

                UnityEditorInternal.ComponentUtility.CopyComponent(component);

                if (repeatedObjects[i].GetComponent(component.GetType()) == null)
                {
                    UnityEditorInternal.ComponentUtility.PasteComponentAsNew(repeatedObjects[i]);
                }
                else
                {
                    UnityEditorInternal.ComponentUtility.PasteComponentValues(repeatedObjects[i].GetComponent(component.GetType()));
                }
            }
        }
    }

    private GameObject CreateRepeatedObject(int index)
    {
        // Create a new game object
        GameObject repeatedObject = new GameObject();

        // For every component in the repeat object, add the component to the repeated object
        foreach (Component component in GetComponents<Component>())
        {
            // If the component is a transform, copy the transform values
            if (component is Transform transform)
            {

                // Copy the transform values
                repeatedObject.transform.position = transform.position;
                repeatedObject.transform.rotation = transform.rotation;
                repeatedObject.transform.localScale = transform.localScale;

                // Skip the transform component
                continue;
            }

            // If the component is a repeat object, skip it
            if (component is RepeatObject)
            {
                continue;
            }

            // Add the component to the repeated object
            UnityEditorInternal.ComponentUtility.CopyComponent(component);
            UnityEditorInternal.ComponentUtility.PasteComponentAsNew(repeatedObject);
        }

        // Set the parent of the repeated object
        repeatedObject.transform.SetParent(transform);

        // Set the name of the repeated object
        repeatedObject.name = gameObject.name + " " + (index + 1);

        // Return the repeated object
        return repeatedObject;
    }
}
