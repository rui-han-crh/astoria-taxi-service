using UnityEngine;

public class DriverBehavior : MonoBehaviour
{
    private InputActions inputActions;
    /// <summary>
    /// Represents the destination vector of the vehicle.
    /// </summary>
    private Vector2 destinationVector;
    private Quaternion destinationRotation;

    private Rigidbody2D horseRb;
    private Rigidbody2D carriageRb;

    /// <summary>
    /// Represents the speed at which the vehicle rotates.
    /// </summary>
    [SerializeField, Tooltip("Speed in degrees per second")]
    private float rotationSpeed = 30f;

    /// <summary>
    /// Represents the speed at which the vehicle moves.
    /// </summary>
    [SerializeField, Tooltip("Speed in units per second")]
    private float movementSpeed = 100f;

    [SerializeField]
    private GameObject carriageBody;

    public GameObject CarriageBody => carriageBody;

    private bool isMoving = false;

    private void Awake()
    {
        inputActions = new InputActions();
    }

    private void Start()
    {
        inputActions?.Enable();

        inputActions.Player.Movement.performed += ctx => TriggerMovement(ctx.ReadValue<Vector2>());
        inputActions.Player.Movement.canceled += ctx => isMoving = false;

        horseRb = GetComponent<Rigidbody2D>();
        carriageRb = carriageBody.GetComponent<Rigidbody2D>();
    }

    public void OnEnable()
    {
        inputActions?.Enable();
    }

    public void OnDisable()
    {
        inputActions?.Disable();
    }

    private void TriggerMovement(Vector2 movement)
    {
        // Signal to begin movement.
        isMoving = true;
        destinationVector = movement;
        destinationRotation = Quaternion.LookRotation(Vector3.forward, destinationVector);

        Debug.DrawLine(transform.position, transform.position + (Vector3)destinationVector, Color.red, 1f);
    }

    private void FixedUpdate()
    {
        if (!isMoving)
        {
            // If not moving, stop moving.
            horseRb.velocity = Vector2.zero;
            return;
        }

        float scaledMovementSpeed = movementSpeed * Time.fixedDeltaTime;

        // Rotates at a speed of 10 degrees per second from the current
        // facing direction to the destination direction.
        // Rotate around the back of the vehicle.
        if ((Vector2)transform.up != destinationVector)
        {
            float step = rotationSpeed * Time.fixedDeltaTime;

            Quaternion rotation = Quaternion.RotateTowards(transform.rotation, destinationRotation, step);

            // Rotate around the pivot point towards the destination rotation.
            Vector3 pivot = transform.position - transform.up;

            Debug.DrawLine(transform.position, pivot, Color.blue, 0.01f);

            transform.RotateAround(pivot, Vector3.forward, rotation.eulerAngles.z - transform.rotation.eulerAngles.z);

            Debug.DrawLine(transform.position, transform.position + (Vector3)transform.up, Color.green, 0.01f);

            float ratio = Quaternion.Angle(transform.rotation, destinationRotation) / 180f;

            scaledMovementSpeed *= 1  - ratio;
        }

        // Move in the direction of the destination vector.
        horseRb.velocity = transform.up * scaledMovementSpeed;
    }
}
