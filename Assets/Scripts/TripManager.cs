using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This class is a manager that helps to select a destination.
/// </summary>
public class TripManager : MonoBehaviour, ISaveable<TripManagerModel>
{
    private static readonly string DROP_OFF_INDICATOR_RESOURCE_PATH = "DropOffIndicator";

    private House destinationHouse;

    private Taxi taxi;

    private void Awake()
    {
        taxi = GetComponent<Taxi>();

        SaveManager.Instance.OnSaveRequested += Save;
    }

    private void Start()
    {
        RestoreStateFromModel();
    }

    public void BeginRide()
    {
        FareManager.Instance.StartFareComputation(500, 10, 2);

        destinationHouse = House.GetRandomHouse();

        SetDropOffLocation();
    }

    private void SetDropOffLocation()
    {
        // Create a drop off indicator at the house.
        CreateDropOffIndicator(destinationHouse.DoorTransform.position);
    }

    private static void CreateDropOffIndicator(Vector2 dropOffLocation)
    {
        GameObject dropOffIndicatorPrefab = ResourceManager.Instance.Load<GameObject>(DROP_OFF_INDICATOR_RESOURCE_PATH);

        // Find the closest point on the navmesh road area to the drop off location.
        NavMesh.SamplePosition(dropOffLocation, out NavMeshHit hit, 30f, (AreaMask)NavMeshAreas.Road);

        // Instantiate the drop off indicator at the closest point on the navmesh.
        Instantiate(dropOffIndicatorPrefab, hit.position, Quaternion.identity);
    }

    public void DropOff()
    {
        FareManager.Instance.EndFareComputation();

        taxi.DropOffPassenger(destinationHouse.DoorTransform.position);

        // Reset the drop off location.
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
            return new TripManagerModel(destinationHouse.name);
        }
    }

    public void RestoreStateFromModel()
    {
        TripManagerModel tripManagerModel = SaveManager.LoadTripManagerModel();

        if (tripManagerModel.DestinationHouseName == null)
        {
            return;
        }

        destinationHouse = House.FindByName(tripManagerModel.DestinationHouseName);
    }

    public void Save(bool writeImmediately = true)
    {
        SaveManager.SaveTripManager(this, writeImmediately);
    }
}
