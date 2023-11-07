using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Manages all the UI updates in the stats panel.
/// </summary>
public class StatsUiPanel : MonoBehaviour
{
    [SerializeField]
    private TMP_Text timeText;

    [SerializeField]
    private TMP_Text balanceText;

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
