using UnityEngine;

public class MountDismountSystem : MonoBehaviour
{
    private const float eKeyHeldTime = 1f;
    private const float mountRange = 1f;

    public Transform carriageTransform;
    public Transform playerDismountedTransform;
    public DriverBehavior driverScript;
    public GameObject driverDashboardPanel;

    private bool mounted = true;
    private float timeHeld = 0;

    private void Update()
    {
        if (mounted)
        {
            DismountCheck();
        }
        else
        {
            MountCheck();
        }
        
    }

    private void DismountCheck()
    {
        if (Input.GetKey(KeyCode.E))
        {
            timeHeld += Time.deltaTime;
            if(timeHeld > eKeyHeldTime)
            {
                timeHeld = 0;
                mounted = false;

                // Dismount
                playerDismountedTransform.gameObject.SetActive(true);
                playerDismountedTransform.position = carriageTransform.position;
                driverScript.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                driverScript.enabled = false;
                driverDashboardPanel.SetActive(false);
            }
        }
        else
        {
            timeHeld = 0;
        }
    }

    private void MountCheck()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            // Checks for distance between carriage and player
            if (Vector2.Distance(carriageTransform.position, 
                playerDismountedTransform.position) <= mountRange)
            {
                mounted = true;

                // Mount
                playerDismountedTransform.gameObject.SetActive(false);
                driverScript.enabled = true;
                driverDashboardPanel.SetActive(true);
            }
        }
    }
}
