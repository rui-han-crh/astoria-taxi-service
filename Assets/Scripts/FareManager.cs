using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FareManager : ISaveable<FareManagerModel>
{
    [SerializeField]
    private Wallet wallet;

    private bool isComputingFare = false;
    private int currentFare;
    private int fareFloor;
    private int decrementAmount;

    /// <summary>
    /// An event that invokes every second when the fare is being computed.
    /// </summary>
    public event Action<int> OnComputationTick;

    private void Awake()
    {
        SaveManager.Instance.OnSaveRequested += Save;

        wallet = GetComponent<Wallet>();
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

    /// <summary>
    /// Ends the fare computation and adds the fare to the wallet.
    /// </summary>
    public void EndFareComputation()
    {
        isComputingFare = false;

        // The process of adding the fare to the wallet is done in the coroutine.
    }

    /// <summary>
    /// Computes the fare per second. This is a coroutine that ticks every second.
    /// </summary>
    /// <param name="baseFare"> The starting fare. </param>
    private IEnumerator ComputeFare(int baseFare)
    {
        currentFare = baseFare;
        
        OnComputationTick?.Invoke(currentFare);

        while (isComputingFare)
        {
            yield return new WaitForSeconds(1);
            
            if (currentFare > fareFloor)
            {
                currentFare = Mathf.Max(currentFare - decrementAmount, fareFloor);
                OnComputationTick?.Invoke(currentFare);

            }
        }

        wallet.IncreaseBalance(currentFare);
        wallet.UpdateDisplay();
    }

    public FareManagerModel ToModel()
    {
        return new FareManagerModel(isComputingFare, currentFare, fareFloor, decrementAmount);
    }

    public void RestoreStateFromModel()
    {
        FareManagerModel? fareManagerModel = SaveManager.LoadFareManagerModel();

        if (fareManagerModel == null)
        {
            return;
        }

        isComputingFare = fareManagerModel.Value.IsComputingFare;
        currentFare = fareManagerModel.Value.CurrentFare;
        fareFloor = fareManagerModel.Value.FareFloor;
        decrementAmount = fareManagerModel.Value.DecrementAmount;

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
