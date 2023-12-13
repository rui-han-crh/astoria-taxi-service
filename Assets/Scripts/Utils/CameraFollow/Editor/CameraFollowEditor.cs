using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CameraFollow))]
public class CameraFollowEditor : Editor
{
    private CameraFollow cameraFollow;
    private SerializedProperty followTarget;

    private void OnEnable()
    {
        cameraFollow = (CameraFollow)target;
        followTarget = serializedObject.FindProperty("followTarget");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (followTarget.objectReferenceValue != null)
        {
            if (GUILayout.Button("Move Camera To Follow Target"))
            {
                Vector3 destination = ((Transform)followTarget.objectReferenceValue).position;
                destination.z = cameraFollow.transform.position.z;
                cameraFollow.transform.position = destination;
            }
        }
    }
}
