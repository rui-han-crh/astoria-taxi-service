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

    public Vector2 CarriagePosition { get; }

    public Quaternion CarriageRotation { get; }

    public Vector2 HorsesPosition { get; }

    public Quaternion HorsesRotation { get; }

    [JsonConstructor]
    public TaxiModel(bool hasPassenger, Vector2 carriagePosition, Quaternion carriageRotation, Vector2 horsesPosition, Quaternion horsesRotation)
    {
        HasPassenger = hasPassenger;
        CarriagePosition = carriagePosition;
        CarriageRotation = carriageRotation;
        HorsesPosition = horsesPosition;
        HorsesRotation = horsesRotation;
    }
}
