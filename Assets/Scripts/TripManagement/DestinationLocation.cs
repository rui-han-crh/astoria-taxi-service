using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text.Json.Serialization;

/// <summary>
/// This class encapsulates the destination location.
/// A destination has a point and a scene it belongs to.
/// The destination location can be null
/// </summary>
public class DestinationLocation
{
    private Vector2? destinationPoint;

    private string destinationSceneName;

    public Vector2? DestinationPoint => destinationPoint;
    public string DestinationSceneName => destinationSceneName;

    public DestinationLocation(Vector2 destinationPoint, string destinationSceneName)
    {
        this.destinationPoint = destinationPoint;
        this.destinationSceneName = destinationSceneName;
    }

    public DestinationLocation(Vector2? destinationPoint)
    {
        this.destinationPoint = destinationPoint;
        this.destinationSceneName = SceneManager.GetActiveScene().name;
    }

    public DestinationLocation()
    {
        this.destinationPoint = null;
        this.destinationSceneName = null;
    }

    public void SetDestinationPoint(Vector2? destinationPoint)
    {
        this.destinationPoint = destinationPoint;
    }
}