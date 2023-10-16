using System;
using System.Text.Json.Serialization;
using UnityEngine;

[Serializable]
public readonly struct Passenger
{
    public string Name { get; }
    public string ResourceFileReference { get; }

    [JsonConstructor]
    public Passenger(
        string name,
        string resourceFileReference)
    {
        Name = name;
        ResourceFileReference = resourceFileReference;
    }

    public static Passenger GetRandomPassenger()
    {
        GameObject[] clients = ResourceManager.Instance.LoadAll<GameObject>("Clients");

        int randomIndex = UnityEngine.Random.Range(0, clients.Length);

        return new Passenger(clients[randomIndex].name, "Clients/" + clients[randomIndex].name);
    }
}
