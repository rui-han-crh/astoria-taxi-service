using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Manages all the UI updates in the stats panel.
/// </summary>
public class StatsUIPanel : MonoBehaviour
{
    public static StatsUIPanel instance;

    private void Awake()
    {
        instance = this;
    }

    public TMP_Text timeText;
    public TMP_Text balanceText;

    /// <summary>
    /// Sets the time displayed in the stats panel.
    /// </summary>
    /// <param name="timeString">The new time string.</param>
    public void UpdateTime(string timeString)
    {
        timeText.text = timeString;
    }

    /// <summary>
    /// Sets the wallet balance displayed in the stats panel.
    /// </summary>
    /// <param name="balance">The wallet balance.</param>
    public void UpdateBalance(int balance)
    {
        balanceText.text = "$" + balance.ToString();
    }
}
