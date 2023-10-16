using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using UnityEngine;

public readonly struct TripManagerModel
{
    public string DestinationHouseName { get; }

    [JsonConstructor]
    public TripManagerModel(string destinationHouseName)
    {
        DestinationHouseName = destinationHouseName;
    }
}
