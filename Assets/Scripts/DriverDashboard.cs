using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Manages all the UI updates in the driver dashboard
/// </summary>
public class DriverDashboard : MonoBehaviour
{
    public TMP_Text destinationText;
    public TMP_Text fareText;
    public TMP_Text distanceText;
    public Image directionPointer;

    public TMP_Text vacantText, hiredText, offText;

    /// <summary>
    /// Updates the destination text in driver dashboard UI
    /// </summary>
    /// <param name="destination">String name of the destination</param>
    public void UpdateDestination(string destination) => destinationText.text = destination;

    /// <summary>
    /// Updates the fare text in driver dashboard UI
    /// </summary>
    /// <param name="fare">Integer fare</param>
    public void UpdateFare(int fare) => fareText.text = "$" + fare.ToString();

    public enum CabStatus { vacant, hired, off }

    /// <summary>
    /// Updates the status of the cab in driver dashboard UI
    /// </summary>
    /// <param name="status">A custom enumerator with values vacant/hired/off</param>
    public void UpdateStatus(CabStatus status)
    {
        // Can change the RGB values
        Color white = Color.white;
        Color gray = Color.gray;

        // Hard coded for now, it doesnt really matter cause we probably wont be reusing this UpdateStatus anyways
        switch (status)
        {
            case CabStatus.vacant:
                vacantText.color = white;
                hiredText.color = gray;
                offText.color = gray;
                break;

            case CabStatus.hired:
                vacantText.color = gray;
                hiredText.color = white;
                offText.color = gray;
                break;

            case CabStatus.off:
                vacantText.color = gray;
                hiredText.color = gray;
                offText.color = white;
                break;
        }
    }

    /// <summary>
    /// Updates the dsitance text in driver dashboard UI
    /// </summary>
    /// <param name="distance">Integer distance</param>
    public void UpdateDistance(int distance) => distanceText.text = distance.ToString() + "m";

    /// <summary>
    /// Updates the direction arrow in driver dashboard UI
    /// </summary>
    /// <param name="direction">The normalized direction of the carriage</param>
    public void UpdateDirection(Vector2 direction)
    {
        directionPointer.rectTransform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(direction.y ,direction.x));
    }
}
