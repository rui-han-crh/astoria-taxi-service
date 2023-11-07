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

    [SerializeField]
    private Taxi taxi;

    [SerializeField]
    private FareManager fareManager;

    [SerializeField]
    private DriverDashboard driverDashboard;

    private void Awake()
    {
        SaveManager.Instance.OnSaveRequested += Save;
    }

    private void Start()
    {
        RestoreStateFromModel();
    }

    private void Update()
    {
        if (destinationHouse)
        {
            Vector2 vectorToHouseFromTaxi = destinationHouse.DoorTransform.position - taxi.transform.position;
            driverDashboard.UpdateDirection(vectorToHouseFromTaxi.normalized);

            int distanceToHouse = Mathf.RoundToInt(vectorToHouseFromTaxi.magnitude);
            driverDashboard.UpdateDistance(distanceToHouse);
        }
    }

    public void BeginRide()
    {
        driverDashboard.ShowUI();

        destinationHouse = House.GetRandomHouse();

        fareManager.OnComputationTick += driverDashboard.UpdateFare;
        fareManager.StartFareComputation(GetEstimatedFare(destinationHouse.transform.position), 10, 2);

        driverDashboard.UpdateDestination(destinationHouse.name);

        SetDropOffLocation();
    }

    private int GetEstimatedFare(Vector2 destination)
    {
        // Get the distance to the destination.
        float distance = Vector2.Distance(taxi.transform.position, destination);

        float randomMultiplier = Random.Range(3f, 5f);

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
        GameObject dropOffIndicatorPrefab = ResourceManager.Instance.Load<GameObject>(DROP_OFF_INDICATOR_RESOURCE_PATH);

        // Find the closest point on the navmesh road area to the drop off location.
        NavMesh.SamplePosition(dropOffLocation, out NavMeshHit hit, 30f, (AreaMask)NavMeshAreas.Road);

        // Pad the location, so that it is some distance away from the edge of the road.
        Vector3 paddedLocation = hit.position + (hit.position - (Vector3)dropOffLocation).normalized;

        // Instantiate the drop off indicator at the closest point on the navmesh.
        Instantiate(dropOffIndicatorPrefab, paddedLocation, Quaternion.identity);
    }

    public void DropOff()
    {
        fareManager.OnComputationTick -= driverDashboard.UpdateFare;
        fareManager.EndFareComputation();

        driverDashboard.HideUI();
        driverDashboard.UpdateFare(0);
        driverDashboard.UpdateDestination("Narnia >:)");
        driverDashboard.UpdateDirection(Vector2.zero);
        driverDashboard.UpdateDistance(0);

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
            return new TripManagerModel(destinationHouse.UniqueKey);
        }
    }

    public void RestoreStateFromModel()
    {
        TripManagerModel? tripManagerModel = SaveManager.LoadTripManagerModel();

        if (tripManagerModel == null || tripManagerModel.Value.DestinationHouseName == null)
        {
            driverDashboard.HideUI(0);
            return;
        }

        destinationHouse = House.FindByName(tripManagerModel.Value.DestinationHouseName);

        driverDashboard.UpdateDestination(destinationHouse.name);

        SetDropOffLocation();
    }

    public void Save(bool writeImmediately = true)
    {
        SaveManager.SaveTripManager(this, writeImmediately);
    }
}
