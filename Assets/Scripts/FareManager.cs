using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FareManager : MonoBehaviour, ISaveable<FareManagerModel>
{
    private static FareManager instance;
    public static FareManager Instance { get => instance;}

    private bool isComputingFare = false;
    private int currentFare;
    private int fareFloor;
    private int decrementAmount;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one FareComputationManager in the scene.");
            Destroy(this);
            return;
        }

        instance = this;

        SaveManager.Instance.OnSaveRequested += Save;
    }

    private void Start()
    {
        RestoreStateFromModel();
    }

    /// <summary>
    /// Begins the fare computation.
    /// </summary>
    /// <param name="startingFare"> The fare amount to start with, decreasing per second. </param>
    public void StartFareComputation(int startingFare, int fareFloor, int decrementAmount = 1)
    {
        isComputingFare = true;
        this.fareFloor = fareFloor;
        this.decrementAmount = decrementAmount;

        StartCoroutine(ComputeFare(startingFare));
    }

    // Not needed for now
    /*
    public void TerminateFareComputation()
    {
        computingFare = false;
        StopCoroutine("FareComputation");
        DriverDashboard.instance.UpdateFare(0);
    }
    */

    /// <summary>
    /// Ends the fare computation and adds the fare to the wallet.
    /// </summary>
    public void EndFareComputation()
    {
        isComputingFare = false;

        // The process of adding the fare to the wallet is done in the coroutine.
    }

    // Ticks every second to calculate the fare
    private IEnumerator ComputeFare(int baseFare)
    {
        // We should not try to retrieve from save file. The data is kept locally until there is a need to save.

        //OldTripState tripState = SaveManager.GetTripState();

        currentFare = baseFare;
        DriverDashboard.instance.UpdateFare(currentFare);
        while (isComputingFare)
        {
            yield return new WaitForSeconds(1);
            
            if (currentFare > fareFloor)
            {
                currentFare = Mathf.Max(currentFare - decrementAmount, fareFloor);
                //SaveManager.UpdateTripState(tripState);
                DriverDashboard.instance.UpdateFare(currentFare);

            }
        }

        // End of fare computation, add the fare to the wallet
        DriverDashboard.instance.UpdateFare(0);

        WalletSystem.Instance.IncreaseBalance(currentFare);
        StatsUIPanel.instance.UpdateBalance(WalletSystem.Instance.GetBalance());
    }

    public FareManagerModel ToModel()
    {
        return new FareManagerModel(isComputingFare, currentFare, fareFloor, decrementAmount);
    }

    public void RestoreStateFromModel()
    {
        FareManagerModel fareManagerModel = SaveManager.LoadFareManagerModel();

        isComputingFare = fareManagerModel.IsComputingFare;
        currentFare = fareManagerModel.CurrentFare;
        fareFloor = fareManagerModel.FareFloor;
        decrementAmount = fareManagerModel.DecrementAmount;

        print($"Restored fare manager state: {isComputingFare}, {currentFare}, {fareFloor}, {decrementAmount}");

        if (isComputingFare)
        {
            StartCoroutine(ComputeFare(currentFare));
        }
    }

    public void Save(bool writeImmediately = true)
    {
        SaveManager.SaveFareManager(this, writeImmediately);
    }
}
