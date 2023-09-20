using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class RotateOnAxis : MonoBehaviour
{
    public Axis axis = Axis.Z;
    public float rotationSpeed = 20;

    private Vector3 GetAxisVector()
    {
        switch (axis)
        {
            case Axis.X:
                return Vector3.right;
            case Axis.Y:
                return Vector3.up;
            case Axis.Z:
                return Vector3.forward;
            default:
                return Vector3.zero;
        }
    }

    void Update()
    {
        // Rotate the object around its local axis
        transform.RotateAround(transform.position, GetAxisVector(), rotationSpeed * Time.deltaTime);
    }
}
