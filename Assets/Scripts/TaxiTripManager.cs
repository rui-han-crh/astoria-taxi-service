using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TaxiTripManager : MonoBehaviour
{
    private static Dictionary<Vector2, TaxiPoint> taxiPointMap;

    // The body of the carriage where the passengers are suppose to go.
    [SerializeField]
    private Transform carriageBody;

    private TaxiPoint dropOffLocation;

    private HashSet<GameObject> passengerGameObjects = new HashSet<GameObject>();

    public List<TaxiPoint> activePickUpIndicators = new List<TaxiPoint>();

    public Transform CarriageBody => carriageBody;

    public List<GameObject> PassengerGameObjects => passengerGameObjects.ToList();

    private TripState tripState;

    private void Awake()
    {
        CacheTaxiPoints();

        LoadPassengers();

        LoadTaxiPoint();
    }

    public void BeginRide(Passenger passenger)
    {
        if (tripState != null)
        {
            throw new System.Exception("Cannot begin a ride when a ride is already in progress.");
        }

        Debug.Log($"Beginning ride with {passenger.Name}");
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
            passengerGameObjects.Add(passengerGameObject);
        }
    }

    private void LoadTaxiPoint()
    {
        Vector2? destinationLocation = SaveManager.GetTripState().GetDestinationPoint();

        if (destinationLocation != null)
        {
            SetDropOffLocation(GetTaxiPoint(destinationLocation.Value));
        }
    }

    private TaxiPoint GetTaxiPoint(Vector2 position)
    {
        if (!taxiPointMap.TryGetValue(position, out TaxiPoint taxiPoint))
        {
            Debug.LogError($"No taxi point found at position {position}, did you move a taxi point recently?");
        }

        return taxiPoint;
    }
    /**
     * Shows the pickup indicator for any taxi point within a 30 meter radius of the taxi.
     */
    private void RandomizePickUpChoice()
    {
        foreach (TaxiPoint taxiPoint in taxiPointMap.Values)
        {
            if (taxiPoint.IsPickUp)
            {
                // Taxi point already active as pick-up point.
                return;
            }

            if (Vector2.Distance(taxiPoint.transform.position, carriageBody.transform.position) < 30f)
            {
                if (Random.Range(0, 100) < 20)
                {
                    taxiPoint.ShowPickUpIndicator();

                    activePickUpIndicators.Add(taxiPoint);

                    taxiPoint.onDisablePickUpIndicator += () =>
                    {
                        activePickUpIndicators.Remove(taxiPoint);
                    };

                    taxiPoint.SetIsPickUp(true);
                    taxiPoint.ClientPoint.SpawnPotentialClient();
                }
            }
        }
    }


    public void SetDropOffLocation(TaxiPoint dropOffLocation)
    {
        this.dropOffLocation = dropOffLocation;
        OldTripState tripState = SaveManager.GetTripState();
        tripState.SetTaxiPoint(dropOffLocation);
        SaveManager.UpdateTripState(tripState);
    }

    public void AddPassengerGameObject(GameObject passengerGameObject)
    {
        passengerGameObjects.Add(passengerGameObject);

        // Update trip state
        OldTripState tripState = SaveManager.GetTripState();
        tripState.Passengers.Add(passengerGameObject.GetComponent<OldPassengerBehaviour>().Passenger);
        SaveManager.UpdateTripState(tripState);
    }

    public void RemovePassengerGameObject(GameObject passengerGameObject)
    {
        passengerGameObjects.Remove(passengerGameObject);

        // Update trip state
        OldTripState tripState = SaveManager.GetTripState();
        tripState.Passengers.Remove(passengerGameObject.GetComponent<OldPassengerBehaviour>().Passenger);
        SaveManager.UpdateTripState(tripState);
    }

    /**
     * Chooses a random destination for the taxi, apart from the pick up point.
     */
    public TaxiPoint ChooseRandomDestination(Vector2 pickUpPosition)
    {
        Vector2[] choices = taxiPointMap.Keys
            .Where(taxiPoint => taxiPoint != pickUpPosition)
            .ToArray();

        int randomIndex = Random.Range(0, choices.Count());

        TaxiPoint randomTaxiPoint = GetTaxiPoint(choices[randomIndex]);

        SetDropOffLocation(randomTaxiPoint);

        // Update trip state
        OldTripState tripState = SaveManager.GetTripState();
        tripState.SetDestinationPoint(choices[randomIndex]);
        SaveManager.UpdateTripState(tripState);

        return randomTaxiPoint;
    }

    public void DisablePickUpIndicators()
    {
        activePickUpIndicators
            .ForEach(activePickUpIndicator => {
                activePickUpIndicator.HideIndicator();
                activePickUpIndicator.SetIsPickUp(false);
            });   

        activePickUpIndicators.Clear();
    }

    private float timeLeftToNextRandomPickUpChoice = 0f;

    public void Update()
    {
        print($"Number of passengers: {passengerGameObjects.Count}");
        if (passengerGameObjects.Count == 0)
        {
            timeLeftToNextRandomPickUpChoice -= Time.deltaTime;

            if (timeLeftToNextRandomPickUpChoice <= 0f)
            {
                timeLeftToNextRandomPickUpChoice = 3f;
                RandomizePickUpChoice();
            }
        }
    }
}
