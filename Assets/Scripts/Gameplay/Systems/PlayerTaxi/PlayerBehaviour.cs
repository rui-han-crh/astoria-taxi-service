using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField]
    private CollisionEventEmitter carriage;

    [SerializeField]
    private CollisionEventEmitter horses;

    public Vector3 CarriagePosition => carriage.transform.position;

    public Vector3 HorsesPosition => horses.transform.position;

    private PassengerBehaviour passengerBehaviour;

    private Taxi playerTaxi;
    void Start()
    {
        playerTaxi = new Taxi();

        carriage.OnTriggerEnterAction += (action) =>
        {
            action.Invoke(gameObject);
        };

        carriage.OnTriggerEnterGameObject += AssignPassenger;

        horses.OnTriggerEnterAction += (action) =>
        {
            action.Invoke(gameObject);
        };
    }

    private void Update()
    {
        playerTaxi.Update();
    }
}
