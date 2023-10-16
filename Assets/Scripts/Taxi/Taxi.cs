using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents the player taxi.
/// 
/// This class stores the current target passenger (regardless of whether they are on board or approaching),
/// and a reference to the trip manager to keeps track of the destination.
/// 
/// If a save is present, this class will consume the save file to restore the state of the taxi.
/// </summary>
public class Taxi : MonoBehaviour
{
    [SerializeField]
    private PassengerBehaviour passengerBehaviour; 
    // If there is no passenger approaching, this will be null.

    private TaxiTripManager manager;

    private void Awake()
    {
        manager = GetComponent<TaxiTripManager>();

        if (manager == null)
        {
            throw new Exception("Taxi must have a TaxiTripManager component.");
        }
    }

    public bool Hail()
    {
        return passengerBehaviour != null;
    }

    public bool HasPassengerApproaching()
    {
        return passengerBehaviour != null;
    }

    public bool HasPassenger()
    {
        return passengerBehaviour != null && passengerBehaviour.state == PassengerState.Boarded;
    }

    /// <summary>
    /// Boards the passenger if the passenger is the current passenger approaching.
    /// </summary>
    /// <param name="hailTaxiBehaviour"> The passenger to board. </param>
    /// <exception cref="Exception"> If the passenger is not the current passenger approaching. </exception>
    public void Board(PassengerBehaviour hailTaxiBehaviour)
    {
        if (passengerBehaviour != hailTaxiBehaviour)
        {
            throw new Exception("Cannot board a passenger that is not the current passenger approaching.");
        }

        manager.BeginRide(passengerBehaviour.Passenger);
    }

    /// <summary>
    /// Drops off the current passenger on board the taxi.
    /// 
    /// This will first reactivate the passenger's game object, then make the passenger path to
    /// the house's door.
    /// </summary>
    /// <param name="passengerPathTarget"> The target position for the passenger to path to. </param>
    public void DropOffPassenger(Vector3 passengerPathTarget)
    {
        passengerBehaviour.gameObject.SetActive(true);

        passengerBehaviour.transform.parent = null;

        passengerBehaviour.SwitchToDropOffState(passengerPathTarget);
    }

    public void CancelHail(PassengerBehaviour hailTaxiBehaviour)
    {
        if (passengerBehaviour == hailTaxiBehaviour)
        {
            passengerBehaviour = null;
        }
    }

    /**
     * Assigns the passenger whose radius indicator collided with this object to the passenger.
     */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Passenger"))
        {
            passengerBehaviour = collision.transform.parent.GetComponent<PassengerBehaviour>();
            passengerBehaviour.SwitchToApproachState();
        }
    }
}
