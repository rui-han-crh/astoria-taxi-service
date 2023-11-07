using UnityEngine;
/// <summary>
/// Manages the wallet balance for the driver.
/// </summary>
public class Wallet : MonoBehaviour, ISaveable<WalletModel>
{
    [SerializeField]
    private StatsUiPanel statsUiPanel;

    private int currentBalance;

    private void Awake()
    {
        SaveManager.Instance.OnSaveRequested += Save;
    }

    private void Start()
    {
        RestoreStateFromModel();
    }

    /// <summary>
    /// Increase the wallet balance by an amount (which can be negative).
    /// </summary>
    /// <param name="amount">The amount to increase the balance by.</param>
    public void IncreaseBalance(int amount)
    {
        currentBalance += amount;
    }

    /// <summary>
    /// Decrease the wallet balance by an amount.
    /// </summary>
    /// <param name="amount">The amount to decrease the balance by.</param>
    public void DecreaseBalance(int amount)
    {
        currentBalance -= amount;
    }

    /// <summary>
    /// Get the current balance in the wallet.
    /// </summary>
    ///  <returns>The current balance in the wallet.</returns>
    public int GetBalance()
    {
        return currentBalance;
    }

    /// <summary>
    /// Update the display of the wallet balance in the UI.
    /// </summary>
    public void UpdateDisplay()
    {
        statsUiPanel.UpdateBalance(currentBalance);
    }

    public WalletModel ToModel()
    {
        return new WalletModel(currentBalance);
    }

    public void RestoreStateFromModel()
    {
        WalletModel? walletModel = SaveManager.GetWalletModel();

        currentBalance = walletModel == null ? 0 : walletModel.Value.CurrentBalance;

        UpdateDisplay();
    }

    public void Save(bool writeImmediately = false)
    {
        SaveManager.SaveWallet(this, writeImmediately);
    }
}
