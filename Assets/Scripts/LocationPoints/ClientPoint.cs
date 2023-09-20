using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientPoint : MonoBehaviour
{
    private static GameObject[] CLIENT_OBJECTS;
    private static readonly string CLIENTS_FOLDER = "Clients";

    [SerializeField]
    private TaxiPoint taxiPoint;

    private PassengerBehaviour passengerBehaviour;

    public TaxiPoint TaxiPoint => taxiPoint;

    public PassengerBehaviour PassengerBehaviour => passengerBehaviour;

    private void Awake()
    {
        CLIENT_OBJECTS = Resources.LoadAll<GameObject>(CLIENTS_FOLDER);
    }

    public void SpawnPotentialClient()
    {
        // Spawn the client at this position
        int randomClientIndex = Random.Range(0, CLIENT_OBJECTS.Length);

        GameObject randomClientPrefab = CLIENT_OBJECTS[randomClientIndex];

        GameObject client = Instantiate(randomClientPrefab, transform.position, Quaternion.identity);

        passengerBehaviour = client.GetComponent<PassengerBehaviour>();

        Passenger currentPassenger = new Passenger("Amanda", $"{CLIENTS_FOLDER}/{randomClientPrefab.name}");
        passengerBehaviour.SetPassenger(currentPassenger);
    }

    public void DespawnClient()
    {
        if (passengerBehaviour == null)
        {
            return;
        }

        Destroy(passengerBehaviour.gameObject);
    }
}
