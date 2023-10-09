using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripState
{
    private Passenger currentPassenger;
    private DestinationLocation destinationLocation;
    private float currentFareAmount;
    public readonly float fareFloor;

    public TripState(Passenger currentPassenger, DestinationLocation destinationLocation, float currentFareAmount, float fareFloor)
    {
        this.currentPassenger = currentPassenger;
        this.destinationLocation = destinationLocation;
        this.currentFareAmount = currentFareAmount;
        this.fareFloor = fareFloor;
    }

    public float CurrentFareAmount
    {
        get => currentFareAmount;
        set => currentFareAmount = Mathf.Max(value, fareFloor);
    }
}
