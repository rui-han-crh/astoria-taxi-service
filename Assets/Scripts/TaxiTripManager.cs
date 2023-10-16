using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class TaxiTripManager : MonoBehaviour
{
    private static Dictionary<Vector2, TaxiPoint> taxiPointMap;

    // The body of the carriage where the passengers are suppose to go.
    [SerializeField]
    private Transform carriageBody;

    private House destinationHouse;

    private Taxi taxi;

    private TripState tripState;

    private House[] houseGameObjects;

    private const NavMeshAreas roadArea = NavMeshAreas.Road;

    private void Awake()
    {
        taxi = GetComponent<Taxi>();

        CacheTaxiPoints();

        LoadPassengers();

        houseGameObjects = GameObject.FindGameObjectsWithTag("House")
            .Select(house => house.GetComponent<House>()).ToArray();
    }

    private static void CacheTaxiPoints()
    {
        // Need to recache taxi points when scene changes.
        taxiPointMap = new Dictionary<Vector2, TaxiPoint>();

        foreach (TaxiPoint taxiPoint in FindObjectsOfType<TaxiPoint>())
        {
            taxiPointMap.Add(taxiPoint.transform.position, taxiPoint);
        }
    }

    private void LoadPassengers()
    {
        // Retrieve the passengers from the save file.
        List<Passenger> passengers = SaveManager.GetTripState().Passengers;

        // For each passenger load the prefab from the resources folder.
        foreach (Passenger passenger in passengers)
        {
            GameObject passengerPrefab = Resources.Load<GameObject>(passenger.ResourceFileReference);

            // Instantiate the passenger prefab.
            GameObject passengerGameObject = Instantiate(passengerPrefab, carriageBody);

            // Set the passenger data into the PassengerBehaviour component.
            passengerGameObject.GetComponent<OldPassengerBehaviour>().SetPassenger(passenger);

            // Disable the passenger object. (On board, so not visible)
            passengerGameObject.SetActive(false);

            // Add the passenger to the list of passenger game objects.
            //passengerGameObjects.Add(passengerGameObject);
        }
    }

    public void BeginRide(Passenger passenger)
    {
        if (tripState != null)
        {
            throw new System.Exception("Cannot begin a ride when a ride is already in progress.");
        }

        FareComputationManager.Instance.StartFareComputatation(500, 10, 2);

        destinationHouse = houseGameObjects[Random.Range(0, houseGameObjects.Length)];
        SetDropOffLocation();
    }

    private void SetDropOffLocation()
    {
        // Create a drop off indicator at the house.
        CreateDropOffIndicator(destinationHouse.DoorTransform.position);
    }

    private static void CreateDropOffIndicator(Vector2 dropOffLocation)
    {
        GameObject dropOffIndicatorPrefab = ResourceManager.Instance.Load<GameObject>("DropOffIndicator");

        // Find the closest point on the navmesh road area to the drop off location.
        NavMesh.SamplePosition(dropOffLocation, out NavMeshHit hit, 30f, (AreaMask)roadArea);

        // Instantiate the drop off indicator at the closest point on the navmesh.
        Instantiate(dropOffIndicatorPrefab, hit.position, Quaternion.identity);
    }

    public void DropOff()
    {
        FareComputationManager.Instance.EndFareComputation();

        taxi.DropOffPassenger(destinationHouse.DoorTransform.position);

        // Reset the drop off location.
        destinationHouse = null;

        // Reset the trip state.
        tripState = null;
    }
}
