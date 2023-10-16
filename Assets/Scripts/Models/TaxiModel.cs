using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using UnityEngine;

/// <summary>
/// Represents the model data of a taxi.
/// </summary>
[Serializable]
public readonly struct TaxiModel
{
    public bool HasPassenger { get; }

    [JsonConstructor]
    public TaxiModel(bool hasPassenger)
    {
        HasPassenger = hasPassenger;
    }
}
