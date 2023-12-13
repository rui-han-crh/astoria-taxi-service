using System;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This class is the entry point of the player system.
/// </summary>
public class Taxi : ISaveable<TaxiModel>
{
    private TripManager tripManager;

    public event Action OnBoard;
    public event Action OnDropOff;
    public event Action OnUpdate;

    public Wallet wallet;

    public Taxi()
    {
        wallet = new Wallet();
        tripManager = new TripManager(this);
    }

    public void Update()
    {
        OnUpdate?.Invoke();
    }

    /// <summary>
    /// Hails the taxi and returns whether the taxi is can be hailed.
    /// </summary>
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

        tripManager.BeginRide();

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

        Vector3 carriagePosition = taxiModel.Value.CarriagePosition;
        Quaternion carriageRotation = taxiModel.Value.CarriageRotation;
        Vector3 horsesPosition = taxiModel.Value.HorsesPosition;
        Quaternion horsesRotation = taxiModel.Value.HorsesRotation;

        carriage.transform.SetPositionAndRotation(carriagePosition, carriageRotation);
        horses.transform.SetPositionAndRotation(horsesPosition, horsesRotation);
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
