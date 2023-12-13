using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This class is a manager that helps to select a destination.
/// </summary>
public class TripManager : ISaveable<TripManagerModel>
{
    private static readonly string DROP_OFF_INDICATOR_RESOURCE_PATH = "DropOffIndicator";

    private House destinationHouse;

    public string DestinationName => destinationHouse ? destinationHouse.name : "Narnia";

    private FareManager fareManager;

    public bool IsOnTrip => destinationHouse != null;

    public event Action OnBeginRide;
    public event Action OnEndRide;
    public event Action OnUpdate;

    public TripManager(Taxi playerTaxi)
    {
        fareManager = new FareManager();
    }

    public void BeginRide()
    {
        destinationHouse = House.GetRandomHouse();

        int estimatedFare = GetEstimatedFare(destinationHouse.DoorTransform.position);
        fareManager.StartFareComputation(estimatedFare, 10, 2);

        SetDropOffLocation();

        OnBeginRide?.Invoke();
    }

    private int GetEstimatedFare(Vector2 destination)
    {
        // Get the distance to the destination.
        float distance = Vector2.Distance(taxi.transform.position, destination);

        float randomMultiplier = UnityEngine.Random.Range(3f, 5f);

        // Get the estimated fare.
        return Mathf.RoundToInt(distance * randomMultiplier);
    }

    private void SetDropOffLocation()
    {
        // Create a drop off indicator at the house.
        CreateDropOffIndicator(destinationHouse.DoorTransform.position);
    }

    private static void CreateDropOffIndicator(Vector2 dropOffLocation)
    {
        GameObject dropOffIndicatorPrefab = ResourceManager.Load<GameObject>(DROP_OFF_INDICATOR_RESOURCE_PATH);

        // Find the closest point on the navmesh road area to the drop off location.
        NavMesh.SamplePosition(dropOffLocation, out NavMeshHit hit, 30f, (AreaMask)NavMeshAreas.Road);

        // Pad the location, so that it is some distance away from the edge of the road.
        Vector3 paddedLocation = hit.position + (hit.position - (Vector3)dropOffLocation).normalized;

        // Instantiate the drop off indicator at the closest point on the navmesh.
        Instantiate(dropOffIndicatorPrefab, paddedLocation, Quaternion.identity);
    }

    public void DropOff()
    {
        fareManager.EndFareComputation();

        taxi.DropOffPassenger(destinationHouse.DoorTransform.position);

        destinationHouse = null;
    }

    public TripManagerModel ToModel()
    {
        if (destinationHouse == null)
        {
            return new TripManagerModel(null);
        }
        else
        {
            return new TripManagerModel(destinationHouse.UniqueKey);
        }
    }

    public void RestoreStateFromModel()
    {
        TripManagerModel? tripManagerModel = SaveManager.LoadTripManagerModel();

        if (tripManagerModel == null || tripManagerModel.Value.DestinationHouseName == null)
        {
            return;
        }

        destinationHouse = House.FindByName(tripManagerModel.Value.DestinationHouseName);

        SetDropOffLocation();
    }

    public Vector2 GetDirection()
    {
        if (destinationHouse == null)
        {
            return Vector2.zero;
        }

        return (destinationHouse.DoorTransform.position - taxi.transform.position).normalized;
    }

    public int GetDistance()
    {
        if (destinationHouse == null)
        {
            return 0;
        }

        return Mathf.RoundToInt(Vector2.Distance(destinationHouse.DoorTransform.position, taxi.transform.position));
    }

    public void Save(bool writeImmediately = true)
    {
        SaveManager.SaveTripManager(this, writeImmediately);
    }
}
