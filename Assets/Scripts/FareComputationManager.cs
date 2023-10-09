using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FareComputationManager : MonoBehaviour
{
    public static FareComputationManager instance;

    private void Awake()
    {
        instance = this;
    }

    private bool computingFare = false;
    private int fare;

    public void StartFareComputatation(int baseFare)
    {
        computingFare = true;
        StartCoroutine(FareComputation(baseFare));
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

    public void EndFareComputation()
    {
        computingFare = false;
    }

    // Ticks every second to calculate the fare
    private IEnumerator FareComputation(int baseFare)
    {
        OldTripState tripState = SaveManager.GetTripState();

        fare = baseFare;
        DriverDashboard.instance.UpdateFare(fare);
        while (computingFare)
        {
            yield return new WaitForSeconds(1);
            
            if (fare > tripState.FareFloor)
            {
                fare--;
                tripState.CurrentFareAmount = fare;
                SaveManager.UpdateTripState(tripState);
                DriverDashboard.instance.UpdateFare(fare);

            }
        }
        DriverDashboard.instance.UpdateFare(0);

        WalletSystem.Instance.IncreaseBalance(fare);
        StatsUIPanel.instance.UpdateBalance(WalletSystem.Instance.GetBalance());
    }
}
