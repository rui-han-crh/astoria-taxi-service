/// <summary>
/// Manages the wallet balance for the driver.
/// </summary>
public class WalletSystem
{
    private static WalletSystem instance;

    public static WalletSystem Instance
    {
        get
        {
            instance ??= new WalletSystem();
            return instance;
        }
    }

    private int currentBalance;

    /// <summary>
    /// Creates a wallet with an initial balance.
    /// </summary>
    /// <param name="initialBalance">Set the initial balance in the wallet.</param>
    public WalletSystem(int initialBalance = 0)
    {
        currentBalance = initialBalance;
        StatsUIPanel.instance.UpdateBalance(initialBalance);
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
}
