using System.Text.Json.Serialization;

public readonly struct HungerModel
{
    public float MaxSatiation { get; }
    public float CurrentSatiation { get; }

    [JsonConstructor]
    public HungerModel(float maxSatiation, float currentSatiation)
    {
        MaxSatiation = maxSatiation;
        CurrentSatiation = currentSatiation;
    }
}
