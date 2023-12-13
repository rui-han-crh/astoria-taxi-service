using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseSpriteRotator : MonoBehaviour
{
    [SerializeField]
    private PlayerDriverBehaviour playerDriverBehaviour;

    private float maxAngle = 40f;
    private float rotationSpeed = 60f;

    private void Update()
    {
        // Angle between the current direction and the destination direction
        float angle = Vector2.SignedAngle(transform.up, playerDriverBehaviour.DestinationVector);

        float angleToBodyUp = Vector2.SignedAngle(playerDriverBehaviour.transform.up, transform.up);

        bool canRotate = Mathf.Abs(angleToBodyUp) < maxAngle || Mathf.Sign(angleToBodyUp) != Mathf.Sign(angle);

        if (playerDriverBehaviour.IsMoving && canRotate)
        {
            // Rotate the horse towards the destination direction
            transform.Rotate(Vector3.forward, Math.Sign(angle) * rotationSpeed * Time.deltaTime);
        }
    }
}
