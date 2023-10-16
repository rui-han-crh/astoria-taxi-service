using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using UnityEngine;

/// <summary>
/// Represents the model data for the passenger behaviour.
/// </summary>
[Serializable]
public readonly struct PassengerBehaviourModel
{
    public Passenger Passenger { get; }
    public PassengerState PassengerState { get; } 

    public float TimeLeftInState { get; }

    [JsonConstructor]
    public PassengerBehaviourModel(Passenger passenger, PassengerState passengerState, float timeLeftInState)
    {
        Passenger = passenger;
        PassengerState = passengerState;
        TimeLeftInState = timeLeftInState;
    }
}
