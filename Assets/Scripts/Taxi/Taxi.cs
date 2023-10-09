using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents the player taxi.
/// </summary>
public class Taxi : MonoBehaviour
{
    private PassengerBehaviour passenger;

    private TaxiTripManager manager;

    private void Awake()
    {
        manager = GetComponent<TaxiTripManager>();
    }

    public bool Hail()
    {
        return passenger != null;
    }

    public bool HasPassengerApproaching()
    {
        return passenger != null;
    }

    public bool HasPassenger()
    {
        return passenger != null && passenger.state == PassengerState.Boarded;
    }

    /// <summary>
    /// Boards the passenger if the passenger is the current passenger approaching.
    /// </summary>
    /// <param name="hailTaxiBehaviour"> The passenger to board. </param>
    /// <exception cref="Exception"> If the passenger is not the current passenger approaching. </exception>
    public void Board(PassengerBehaviour hailTaxiBehaviour)
    {
        if (passenger != hailTaxiBehaviour)
        {
            throw new Exception("Cannot board a passenger that is not the current passenger approaching.");
        }

        manager.BeginRide(passenger);
    }

    public void CancelHail(PassengerBehaviour hailTaxiBehaviour)
    {
        if (passenger == hailTaxiBehaviour)
        {
            passenger = null;
        }
    }

    /**
     * Assigns the passenger whose radius indicator collided with this object to the passenger.
     */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Passenger"))
        {
            passenger = collision.transform.parent.GetComponent<PassengerBehaviour>();
            passenger.SwitchToApproachState();
        }
    }
}
