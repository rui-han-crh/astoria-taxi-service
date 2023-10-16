using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using UnityEngine;

/// <summary>
/// This class encapsulates the data that is stored in the save file. It stores
/// all the other data models that are stored in the save file.
/// </summary>
public struct SaveFileModel
{
    public TaxiModel TaxiModel { get; set; }

    public PassengerBehaviourModel PassengerBehaviourModel { get; set;  }

    public TripManagerModel TripManagerModel { get; set; }

    public FareManagerModel FareManagerModel { get; set; }

    public SaveFileModel(
        TaxiModel taxiModel, 
        PassengerBehaviourModel passengerBehaviourModel,
        TripManagerModel tripManagerModel,
        FareManagerModel fareManagerModel,
        string test)
    {
        TaxiModel = taxiModel;
        PassengerBehaviourModel = passengerBehaviourModel;
        TripManagerModel = tripManagerModel;
        FareManagerModel = fareManagerModel;
    }
}
