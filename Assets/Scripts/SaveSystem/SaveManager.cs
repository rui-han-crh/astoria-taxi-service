using UnityEngine;
using System.IO;
using System.Text.Json;
using System;

/// <summary>
/// <para>This class forms an abstraction over the save file. It is treated as a singleton.</para>
/// 
/// <para>When using this class, treat it as if each operation performs a read/write to the save file.</para>
/// 
/// <para>Therefore, use this class sparingly and please account for the expense of I/O operations.</para>
/// </summary>
public class SaveManager
{
    public static readonly string SAVE_FILE_PATH = Application.persistentDataPath + "/save.json";

    // To allow serialization of Vector2.
    private static readonly JsonSerializerOptions options = new JsonSerializerOptions
    {
        Converters = { new Vector2Converter(), new QuaternionConverter() },
        WriteIndented = true,
        //ReferenceHandler = ReferenceHandler.Preserve,
    };

    private static SaveManager instance;

    public static SaveManager Instance
    {
        get
        {
            instance ??= new SaveManager();
            return instance;
        }
    }

    private SaveFileModel? saveFileModel;
    private readonly InputActions inputActions;

    public delegate void SaveDelegate(bool writeImmediately);
    public event SaveDelegate OnSaveRequested;

    public SaveManager()
    {
        inputActions = new InputActions();
        
        inputActions.Enable();

        inputActions.Player.Save.performed += _ =>
        {
            // Invoke all subscriber actions (without writing to disk first).
            OnSaveRequested(writeImmediately: false);

            // Write as a batch to disk.
            WriteToDisk();
        };

        // Check if save file exists.
        if (File.Exists(SAVE_FILE_PATH))
        {
            // Load save file
            saveFileModel = JsonSerializer.Deserialize<SaveFileModel>(File.ReadAllText(SAVE_FILE_PATH), options);
            
            // Otherwise, we keep the saveFileModel as null.
        }
    }

    public static TaxiModel? LoadTaxiModel()
    {
        return Instance.saveFileModel?.TaxiModel;
    }

    public static void SaveTaxi(Taxi taxi, bool writeImmediately = true)
    {
        SaveFileModel saveFileModel = Instance.saveFileModel ?? new SaveFileModel();
        saveFileModel.TaxiModel = taxi.ToModel();
        Instance.saveFileModel = saveFileModel;

        if (writeImmediately)
        {
            WriteToDisk();
        }
    }

    public static TripManagerModel? LoadTripManagerModel()
    {
        return Instance.saveFileModel?.TripManagerModel;
    }

    public static void SaveTripManager(TripManager taxiTripManager, bool writeImmediately = true)
    {
        SaveFileModel saveFileModel = Instance.saveFileModel ?? new SaveFileModel();
        saveFileModel.TripManagerModel = taxiTripManager.ToModel();
        Instance.saveFileModel = saveFileModel;

        if (writeImmediately)
        {
            WriteToDisk();
        }
    }

    public static void SavePassengerBehaviour(PassengerBehaviour passengerBehaviour, bool writeImmediately = true)
    {
        SaveFileModel saveFileModel = Instance.saveFileModel ?? new SaveFileModel();
        saveFileModel.PassengerBehaviourModel = passengerBehaviour.ToPassengerBehaviourModel();
        Instance.saveFileModel = saveFileModel;

        if (writeImmediately)
        {
            WriteToDisk();
        }
    }

    public static PassengerBehaviourModel? LoadPassengerBehaviourModel()
    {
        return Instance.saveFileModel?.PassengerBehaviourModel;
    }

    public static void SaveFareManager(FareManager fareManager, bool writeImmediately = true)
    {
        SaveFileModel saveFileModel = Instance.saveFileModel ?? new SaveFileModel();
        saveFileModel.FareManagerModel = fareManager.ToModel();
        Instance.saveFileModel = saveFileModel;

        if (writeImmediately)
        {
            WriteToDisk();
        }
    }

    public static FareManagerModel? LoadFareManagerModel()
    {
        return Instance.saveFileModel?.FareManagerModel;
    }

    public static WalletModel? GetWalletModel()
    {
        return Instance.saveFileModel?.WalletModel;
    }

    public static void SaveWallet(Wallet wallet, bool writeImmediately = true)
    {
        SaveFileModel saveFileModel = Instance.saveFileModel ?? new SaveFileModel();
        saveFileModel.WalletModel = wallet.ToModel();
        Instance.saveFileModel = saveFileModel;

        if (writeImmediately)
        {
            WriteToDisk();
        }
    }

    public static HungerModel? GetHungerModel()
    {
        return Instance.saveFileModel?.HungerModel;
    }

    public static void SaveHunger(HungerManager hungerManager, bool writeImmediately = true)
    {
        SaveFileModel saveFileModel = Instance.saveFileModel ?? new SaveFileModel();
        saveFileModel.HungerModel = hungerManager.ToModel();
        Instance.saveFileModel = saveFileModel;

        if (writeImmediately)
        {
            WriteToDisk();
        }
    }

    public static MaintenanceModel? GetMaintenanceModel()
    {
        return Instance.saveFileModel?.MaintenanceModel;
    }

    public static void SaveMaintenance(MaintenanceManager maintenanceManager, bool writeImmediately = true)
    {
        SaveFileModel saveFileModel = Instance.saveFileModel ?? new SaveFileModel();
        saveFileModel.MaintenanceModel = maintenanceManager.ToModel();
        Instance.saveFileModel = saveFileModel;

        if (writeImmediately)
        {
            WriteToDisk();
        }
    }

    public static void WriteToDisk()
    {
        Debug.Log(JsonSerializer.Serialize(Instance.saveFileModel, options));
        File.WriteAllText(SAVE_FILE_PATH, JsonSerializer.Serialize(Instance.saveFileModel, options));
    }

    public static SceneState GetSceneState()
    {
        return null; // Instance.saveFileModel.SceneState;
    }

    public static void UpdateSceneState(SceneState sceneState)
    {
        //Instance.saveFileModel.SceneState = sceneState;
        Debug.Log("Wait... Temporarily removed");
    }

    public static OldTripState GetTripState()
    {
        //return Instance.saveFileModel.TripState;
        return null;
    }

    public static void UpdateTripState(OldTripState tripState)
    {
        //Instance.saveFileModel.TripState = tripState;
        Debug.Log("Wait... Temporarily removed");
    }

    public static void DeleteSaveFile()
    {
        File.Delete(SAVE_FILE_PATH);
        Instance.saveFileModel = null;
    }
}
