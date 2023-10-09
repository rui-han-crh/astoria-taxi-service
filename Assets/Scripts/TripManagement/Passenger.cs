using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passenger
{
    public string Name { get; }
    public string ResourceFileReference { get; }

    public Passenger(
        string name,
        string resourceFileReference)
    {
        Name = name;
        ResourceFileReference = resourceFileReference;
    }

    public Passenger()
    {
        Name = "Default Passenger";
        ResourceFileReference = "Clients/Default Passenger";
    }

    public static Passenger GetRandomPassenger()
    {
        GameObject[] clients = ResourceManager.Instance.LoadAll<GameObject>("Clients");

        int randomIndex = Random.Range(0, clients.Length);

        return new Passenger(clients[randomIndex].name, "Clients/" + clients[randomIndex].name);
    }
}
