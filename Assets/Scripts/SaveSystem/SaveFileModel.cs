using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveFileModel
{
    public TripState TripState { get; set; }
    public SceneState SceneState { get; set; }

    public SaveFileModel()
    {
        // Default trip state when no save file exists.
        TripState = new TripState(new List<Passenger>(), null, null, 0, 50);
        SceneState = new SceneState(null, null);
    }
}
