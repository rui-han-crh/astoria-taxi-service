using DG.Tweening;
using DG.Tweening.Core;
using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// This class represents the behavior of the driver.
/// 
/// TODO: Make this class prettier, it's a mess.
/// </summary>
public abstract class DriverBehavior : MonoBehaviour
{
    /// <summary>
    /// Represents the destination vector of the vehicle.
    /// </summary>
    protected Vector2 destinationVector;
    protected Quaternion destinationRotation;

    protected Rigidbody2D horseRb;

    /// <summary>
    /// Represents the speed at which the vehicle rotates.
    /// </summary>
    [SerializeField, Tooltip("Speed in degrees per second")]
    protected float rotationSpeed = 30f;

    /// <summary>
    /// Represents the speed at which the vehicle moves.
    /// </summary>
    [SerializeField, Tooltip("Speed in units per second")]
    protected float movementSpeed = 100f;

    [SerializeField]
    protected GameObject carriageBody;

    public GameObject CarriageBody => carriageBody;

    protected bool isMoving = false;
    protected bool isReversing = false;
    protected bool isRotating = false;

    public delegate void MovementStarted(float movementSpeed);
    public event MovementStarted OnMovementStarted;
    public event Action OnMovementStopped;

    protected virtual void Start()
    {
        if (!TryGetComponent(out horseRb))
        {
            Debug.LogError("Driver must have a Rigidbody2D component.");
        }
    }

    protected void TriggerMovement(Vector2 movement)
    {
        // Signal to begin movement.
        isMoving = true;
        destinationVector = movement;
        destinationRotation = Quaternion.LookRotation(Vector3.forward, destinationVector);

        OnMovementStarted?.Invoke(movementSpeed);
    }

    protected void StopMoving()
    {
        isMoving = false;

        OnMovementStopped?.Invoke();
    }

    private void FixedUpdate()
    {
        if (isReversing)
        {
            horseRb.velocity = -transform.up * movementSpeed * 0.5f * Time.fixedDeltaTime;
            return;
        }

        if (!isMoving)
        {
            // If not moving, stop moving.
            horseRb.velocity = Vector2.zero;
            return;
        }

        float scaledMovementSpeed = movementSpeed * Time.fixedDeltaTime;

        // Transform up dictates the forward direction of the vehicle.
        if ((Vector2)transform.up != destinationVector)
        {
            float step = rotationSpeed * Time.fixedDeltaTime;

            Quaternion rotation = Quaternion.RotateTowards(transform.rotation, destinationRotation, step);

            transform.RotateAround(transform.position, Vector3.forward, rotation.eulerAngles.z - transform.rotation.eulerAngles.z);

            float ratio = Quaternion.Angle(transform.rotation, destinationRotation) / 180f;

            scaledMovementSpeed *= 1  - ratio;
        }

        // Move in the direction of the destination vector.
        horseRb.velocity = transform.up * scaledMovementSpeed;
    }
}
