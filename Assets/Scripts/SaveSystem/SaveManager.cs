using UnityEngine;
using System.IO;
using System.Text.Json;

public class SaveManager
{
    public static readonly string SAVE_FILE_PATH = Application.persistentDataPath + "/save.json";

    private static SaveManager instance;

    public static SaveManager Instance
    {
        get
        {
            instance ??= new SaveManager();
            return instance;
        }
    }

    private readonly SaveFileModel saveFileModel;

    public SaveManager()
    {
        // Check if save file exists.
        if (!File.Exists(SAVE_FILE_PATH))
        {
            // Create new save file
            saveFileModel = new SaveFileModel();
        } else
        {
            // Load save file
            saveFileModel = JsonSerializer.Deserialize<SaveFileModel>(File.ReadAllText(SAVE_FILE_PATH));
        }
    }

    public static TripState GetTripState()
    {
        return Instance.saveFileModel.TripState;
    }

    public static void UpdateTripState(TripState tripState)
    {
        Instance.saveFileModel.TripState = tripState;
    }

    public static void Save()
    {
        File.WriteAllText(SAVE_FILE_PATH, JsonSerializer.Serialize(Instance.saveFileModel));
    }
}
