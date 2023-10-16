using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class MountDismountSystem : MonoBehaviour
{
    // Distance the player has to be from carriage in order to mount
    private const float mountRange = 1f;

    public Transform carriageTransform;
    public Transform playerDismountedTransform;
    public DriverBehavior driverScript;
    public GameObject driverDashboardPanel;

    private bool mounted = true;

    private InputActions inputActions;

    private void Awake()
    {
        inputActions = new InputActions();
    }

    public void OnEnable()
    {
        inputActions?.Enable();
        inputActions.Player.MountDismount.performed += (ctx) => MountDismountCheck(ctx);
    }

    public void OnDisable()
    {
        inputActions?.Disable();
    }


    private void Start()
    {
        inputActions?.Enable();
    }

    private void MountDismountCheck(InputAction.CallbackContext context)
    {
        if (mounted && context.interaction is HoldInteraction)
        {
            Dismount();
        }
        else if (!mounted && context.interaction is TapInteraction)
        {
            Mount();
        }
    }

    private void Dismount()
    {
        mounted = false;

        playerDismountedTransform.gameObject.SetActive(true);
        playerDismountedTransform.position = carriageTransform.position;
        driverScript.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        driverScript.enabled = false;
        driverDashboardPanel.SetActive(false);
    }

    private void Mount()
    {
        // Checks for distance between carriage and player
        if (Vector2.Distance(carriageTransform.position,
            playerDismountedTransform.position) <= mountRange)
        {
            mounted = true;

            playerDismountedTransform.gameObject.SetActive(false);
            driverScript.enabled = true;
            driverDashboardPanel.SetActive(true);
        }
    }
}
