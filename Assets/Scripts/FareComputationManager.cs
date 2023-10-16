using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FareComputationManager : MonoBehaviour
{
    private static FareComputationManager instance;
    public static FareComputationManager Instance { get => instance;}

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one FareComputationManager in the scene.");
            Destroy(this);
            return;
        }

        instance = this;
    }

    private bool isComputingFare = false;
    private int currentFare;
    private int fareFloor;
    private int decrementAmount;

    /// <summary>
    /// Begins the fare computation.
    /// </summary>
    /// <param name="startingFare"> The fare amount to start with, decreasing per second. </param>
    public void StartFareComputatation(int startingFare, int fareFloor, int decrementAmount = 1)
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
}
