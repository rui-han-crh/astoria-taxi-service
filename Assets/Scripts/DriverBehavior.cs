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

    public Vector2 DestinationVector => destinationVector;

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
    public bool IsMoving => isMoving;

    protected bool isReversing = false;
    public bool IsReversing => isReversing;

    protected bool isRotating = false;
    public bool IsRotating => isRotating;

    private float accelerationRatio = 0f;

    public delegate void MovementStarted(float movementSpeed);
    public event MovementStarted OnMovementStarted;
    public event Action OnMovementStopped;

    protected virtual void Start()
    {
        if (!TryGetComponent(out horseRb))
        {
            Debug.LogError("Driver must have a Rigidbody2D component.");
        }

        destinationVector = transform.up;
        destinationRotation = transform.rotation;
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
            accelerationRatio -= Time.fixedDeltaTime * 2f;
            accelerationRatio = Mathf.Clamp(accelerationRatio, 0f, 1f);
            return;
        }

        accelerationRatio += Time.fixedDeltaTime * 0.5f;
        accelerationRatio = Mathf.Clamp(accelerationRatio, 0f, 1f);

        float scaledMovementSpeed = movementSpeed * Time.fixedDeltaTime;

        // Transform up dictates the forward direction of the vehicle.
        if ((Vector2)transform.up != destinationVector)
        {
            float step = rotationSpeed * Time.fixedDeltaTime;

            Quaternion rotation = Quaternion.RotateTowards(transform.rotation, destinationRotation, step);

            transform.RotateAround(transform.position, Vector3.forward, rotation.eulerAngles.z - transform.rotation.eulerAngles.z);

            float ratio = Quaternion.Angle(transform.rotation, destinationRotation) / 180f;

            // Scale movement speed based on how far the vehicle is from the destination rotation.
            scaledMovementSpeed *= 1 - ratio;
        }

        // Move in the direction of the destination vector.
        horseRb.velocity = transform.up * scaledMovementSpeed * accelerationRatio;
    }
}
