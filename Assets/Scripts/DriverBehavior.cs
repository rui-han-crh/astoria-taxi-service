using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Rigidbody2D))]
    internal class DriverBehavior : MonoBehaviour
    {
        private InputActions inputActions;
        /**
         * Represents intended destination to orient the vehicle towards.
         */
        [SerializeField]
        private Vector2 destinationVector;
        private Quaternion destinationRotation;

        private Rigidbody2D rb;

        /**
         * Represents the speed at which the vehicle rotates.
         */
        [SerializeField, Tooltip("Speed in degrees per second")]
        private float rotationSpeed = 30f;

        /**
         * Represents the speed at which the vehicle moves.
         */
        [SerializeField, Tooltip("Speed in units per second")]
        private float movementSpeed = 100f;

        private bool isMoving = false;

        private void Awake()
        {
            inputActions = new InputActions();
        }

        private void Start()
        {
            inputActions?.Enable();

            inputActions.Driver.Movement.performed += ctx => TriggerMovement(ctx.ReadValue<Vector2>());
            inputActions.Driver.Movement.canceled += ctx => isMoving = false;

            rb = GetComponent<Rigidbody2D>();
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
            Debug.DrawLine(transform.position, transform.position + (Vector3)destinationVector, Color.red, 3f);
        }

        private void FixedUpdate()
        {
            if (!isMoving)
            {
                // If not moving, stop moving.
                rb.velocity = Vector2.zero;
                return;
            }

            float scaledMovementSpeed = movementSpeed * Time.fixedDeltaTime;

            // Rotates at a speed of 10 degrees per second from the current
            // facing direction to the destination direction.
            // Rotate around the back of the vehicle.
            if ((Vector2)transform.up != destinationVector)
            {
                float step = rotationSpeed * Time.fixedDeltaTime;

                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    destinationRotation,
                    step);

                float ratio = Quaternion.Angle(transform.rotation, destinationRotation) / 180f;

                scaledMovementSpeed *= 1  - ratio;
            }

            // Move in the direction of the destination vector.
            rb.velocity = transform.up * scaledMovementSpeed;
        }
    }
}
