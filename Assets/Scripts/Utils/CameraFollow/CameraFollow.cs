using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Automatically find the player by tag and follows it, without rotating.
/// </summary>
public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform followTarget;

    [SerializeField]
    private bool maintainInitialOffset = false;

    private Vector3 initialOffset;

    private void Awake()
    {
        if (maintainInitialOffset)
        {
            initialOffset = transform.position - followTarget.position;
            initialOffset.z = 0f;
        }
    }

    private void Update()
    {
        Vector3 playerPosition = followTarget.position;
        playerPosition.z = transform.position.z;

        if (maintainInitialOffset)
        {
            transform.position = playerPosition + initialOffset;
        }
        else
        {
            transform.position = playerPosition;
        }
    }
}
