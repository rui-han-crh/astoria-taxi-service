using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FareComputationManager : MonoBehaviour
{
    // Where do i put the wallet system????
    public WalletSystem walletSystem = new WalletSystem(1200);

    private const int fareFloor = 50;

    private bool computingFare = false;
    private int fare;


    private void Start()
    {
        StartFareComputatation(53);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            EndFareComputation();
        }
    }

    public void StartFareComputatation(int baseFare)
    {
        computingFare = true;
        StartCoroutine(FareComputation(baseFare));
    }

    public void EndFareComputation()
    {
        computingFare = false;
    }

    private IEnumerator FareComputation(int baseFare)
    {
        fare = baseFare;
        DriverDashboard.instance.UpdateFare(fare);
        while (computingFare)
        {
            yield return new WaitForSeconds(1);
            if (fare > fareFloor)
            {
                fare--;
                DriverDashboard.instance.UpdateFare(fare);
            }
        }
        DriverDashboard.instance.UpdateFare(0);

        // need to be changed after clarifying 
        walletSystem.IncreaseBalance(fare);
        StatsUIPanel.instance.UpdateBalance(walletSystem.GetBalance());
    }
}
