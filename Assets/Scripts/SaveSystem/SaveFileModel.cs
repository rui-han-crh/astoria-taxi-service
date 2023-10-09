using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveFileModel
{
    public OldTripState TripState { get; set; }
    public SceneState SceneState { get; set; }

    public SaveFileModel()
    {
        // Default trip state when no save file exists.
        TripState = new OldTripState(new List<Passenger>(), null, null, 0, 50);
        SceneState = new SceneState(null, null);
    }
}
