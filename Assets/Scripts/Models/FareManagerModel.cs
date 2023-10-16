using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using UnityEngine;

public readonly struct FareManagerModel
{
    public bool IsComputingFare { get; }

    public int CurrentFare { get; }

    public int FareFloor { get; }

    public int DecrementAmount { get; }

    [JsonConstructor]
    public FareManagerModel(bool isComputingFare, int currentFare, int fareFloor, int decrementAmount)
    {
        IsComputingFare = isComputingFare;
        CurrentFare = currentFare;
        FareFloor = fareFloor;
        DecrementAmount = decrementAmount;
    }
}
