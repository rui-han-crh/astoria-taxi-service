using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverScriptController : MonoBehaviour
{
    [SerializeField]
    private TMP_Text description;

    public void UpdateHorseDeathDescription()
    {
        description.text = "Your horse died of hunger. :(";
    }

    public void UpdateCarriageDeathDescription()
    {
        description.text = "Your carriage broke down due to the lack of maintenance.";
    }

    public void ReloadLastSave()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    public void StartNewGame()
    {
        SaveManager.DeleteSaveFile();
        ReloadLastSave();
    }
}
