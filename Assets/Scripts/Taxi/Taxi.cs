using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Represents the player taxi.
/// 
/// This class stores the current target passenger (regardless of whether they are on board or approaching),
/// and a reference to the trip manager to keeps track of the destination.
/// 
/// If a save is present, this class will consume the save file to restore the state of the taxi.
/// </summary>
public class Taxi : MonoBehaviour, ISaveable<TaxiModel>
{
    [SerializeField]
    private CollisionEventEmitter carriage;

    [SerializeField]
    private CollisionEventEmitter horses;

    public Vector3 CarriagePosition => carriage.transform.position;

    public Vector3 HorsesPosition => horses.transform.position;

    private PassengerBehaviour passengerBehaviour; 
    // If there is no passenger approaching, this will be null.

    private TripManager manager;

    public event Action OnBoard;
    public event Action OnDropOff;

    private void Awake()
    {
        if (!TryGetComponent(out manager))
        {
            throw new Exception("Taxi must have a TaxiTripManager component.");
        }

        SaveManager.Instance.OnSaveRequested += Save;
    }

    private void Start()
    {
        RestoreStateFromModel();

        carriage.OnTriggerEnterAction += (action) =>
        {
            action.Invoke(this.gameObject);
        };

        carriage.OnTriggerEnterGameObject += AssignPassenger;

        horses.OnTriggerEnterAction += (action) =>
        {
            action.Invoke(this.gameObject);
        };
    }

    public bool Hail()
    {
        return passengerBehaviour != null;
    }

    public bool HasPassengerApproaching()
    {
        return passengerBehaviour != null;
    }

    public bool HasPassenger()
    {
        return passengerBehaviour != null && passengerBehaviour.state == PassengerState.Boarded;
    }

    /// <summary>
    /// Boards the passenger if the passenger is the current passenger approaching.
    /// </summary>
    /// <param name="hailTaxiBehaviour"> The passenger to board. </param>
    /// <exception cref="Exception"> If the passenger is not the current passenger approaching. </exception>
    public void Board(PassengerBehaviour hailTaxiBehaviour)
    {
        if (passengerBehaviour != hailTaxiBehaviour)
        {
            throw new Exception("Cannot board a passenger that is not the current passenger approaching.");
        }

        OnBoard?.Invoke();

        manager.BeginRide();

        carriage.GetComponent<NavMeshObstacle>().enabled = true;
    }

    /// <summary>
    /// Drops off the current passenger on board the taxi.
    /// 
    /// This will first reactivate the passenger's game object, then make the passenger path to
    /// the house's door.
    /// </summary>
    /// <param name="passengerPathTarget"> The target position for the passenger to path to. </param>
    public void DropOffPassenger(Vector3 passengerPathTarget)
    {
        OnDropOff?.Invoke();

        passengerBehaviour.gameObject.SetActive(true);

        passengerBehaviour.transform.parent = null;

        passengerBehaviour.SwitchToDropOffState(passengerPathTarget);

        passengerBehaviour = null;
    }

    public void CancelHail(PassengerBehaviour hailTaxiBehaviour)
    {
        if (passengerBehaviour == hailTaxiBehaviour)
        {
            passengerBehaviour = null;
            carriage.GetComponent<NavMeshObstacle>().enabled = true;
        }
    }

    /**
     * Assigns the passenger whose radius indicator collided with this object to the passenger.
     */
    private void AssignPassenger(GameObject passengerPickUpArea)
    {
        if (passengerPickUpArea.CompareTag(Tags.Passenger)) 
        {
            passengerBehaviour = passengerPickUpArea.transform.parent.GetComponent<PassengerBehaviour>();
            passengerBehaviour.SwitchToApproachState();
            carriage.GetComponent<NavMeshObstacle>().enabled = false;
        }
    }

    public TaxiModel ToModel()
    {
        Vector2 carriagePosition = CarriagePosition;
        Quaternion carriageRotation = carriage.transform.rotation;
        Vector2 horsesPosition = HorsesPosition;
        Quaternion horsesRotation = horses.transform.rotation;

        return new TaxiModel(HasPassenger(), carriagePosition, carriageRotation, horsesPosition, horsesRotation);
    }

    public void RestoreStateFromModel()
    {
        TaxiModel? taxiModel = SaveManager.LoadTaxiModel();

        if (taxiModel == null)
        {
            return;
        }

        if (taxiModel.Value.HasPassenger)
        {
            passengerBehaviour = PassengerBehaviour.CreateFromSaveFile();

            passengerBehaviour.transform.parent = transform;
            passengerBehaviour.transform.localPosition = Vector3.zero;
            passengerBehaviour.gameObject.SetActive(false);
        }

        carriage.transform.SetPositionAndRotation(taxiModel.Value.CarriagePosition, taxiModel.Value.CarriageRotation);
        horses.transform.SetPositionAndRotation(taxiModel.Value.HorsesPosition, taxiModel.Value.HorsesRotation);
    }

    public void Save(bool writeImmediately = true)
    {
        SaveManager.SaveTaxi(this, writeImmediately);

        if (passengerBehaviour != null)
        {
            SaveManager.SavePassengerBehaviour(passengerBehaviour, writeImmediately);
        }
    }
    private void OnDestroy()
    {
        SaveManager.Instance.OnSaveRequested -= Save;
    }
}
