using System.Text.Json.Serialization;

public readonly struct MaintenanceModel
{
    public float MaxMaintenance { get; }
    public float CurrentMaintenance { get; }

    [JsonConstructor]
    public MaintenanceModel(float maxMaintenance, float currentMaintenance)
    {
        MaxMaintenance = maxMaintenance;
        CurrentMaintenance = currentMaintenance;
    }
}
