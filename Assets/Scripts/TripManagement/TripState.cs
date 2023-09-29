using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.Json.Serialization;

#nullable enable
public class TripState
{
    private List<Passenger> passengers;

    private DestinationLocation? destinationLocation;

    private float currentFareAmount;

    private readonly float fareFloor;

    public TripState(
        List<Passenger> passengers,
        DestinationLocation? destinationLocation,
        TaxiPointLocation? taxiPointLocation,
        float currentFareAmount,
        float fareFloor)
    {
        this.passengers = passengers;
        this.destinationLocation = destinationLocation;
        this.currentFareAmount = currentFareAmount;
        TaxiPointLocation = taxiPointLocation;
        this.fareFloor = fareFloor;
    }

    public List<Passenger> Passengers => passengers;

    public DestinationLocation? DestinationLocation => destinationLocation;

    public float CurrentFareAmount
    {
        get => currentFareAmount;
        set => currentFareAmount = Mathf.Max(value, fareFloor);
    }

    public float FareFloor => fareFloor;

    public TaxiPointLocation? TaxiPointLocation
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

    public void SetDestinationPoint(Vector2? destinationPoint)
    {
        if (destinationLocation == null)
        {
            destinationLocation = new DestinationLocation(destinationPoint);
        } else
        {
            destinationLocation.SetDestinationPoint(destinationPoint);
        }
    }

    public Vector2? GetDestinationPoint()
    {
        return destinationLocation?.DestinationPoint;
    }

    public void SetTaxiPoint(TaxiPoint taxiPoint)
    {
        TaxiPointLocation = new TaxiPointLocation(taxiPoint);
    }

    /// <summary>
    /// Ends the current trip completely, and updates it.
    /// </summary>
    public void EndTrip()
    {
        passengers.Clear();
        destinationLocation = null;
        currentFareAmount = 0;
        TaxiPointLocation = null;
        SaveManager.UpdateTripState(this);
    }
}
#nullable disable
