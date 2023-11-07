using System.Text.Json.Serialization;

public readonly struct WalletModel
{
    public int CurrentBalance { get; }

    [JsonConstructor]
    public WalletModel(int currentBalance)
    {
        CurrentBalance = currentBalance;
    }
}
