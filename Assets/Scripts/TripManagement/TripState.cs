using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripState
{
    private List<Passenger> passengers;

    private Vector2? destinationLocation;

    private float currentFareAmount;

    private readonly float fareFloor;

    public TripState(
        List<Passenger> passengers,
        Vector2? destinationPoint,
        float currentFareAmount,
        float fareFloor)
    {
        this.passengers = passengers;
        this.destinationLocation = destinationPoint;
        this.currentFareAmount = currentFareAmount;
        this.fareFloor = fareFloor;
    }

    public List<Passenger> Passengers => passengers;
    public Vector2? DestinationLocation => destinationLocation;

    public float CurrentFareAmount
    {
        get => currentFareAmount;
        set => currentFareAmount = Mathf.Max(value, fareFloor);
    }

    public float FareFloor => fareFloor;

    public TaxiPoint TaxiPoint
    {
        get; set;
    }

    public void AddPassenger(Passenger passenger)
    {
        passengers.Add(passenger);
    }

    public void RemovePassenger(Passenger passenger)
    {
        passengers.Remove(passenger);
    }

    public void SetDestinationPoint(Vector2 destinationLocation)
    {
        this.destinationLocation = destinationLocation;
    }
}
