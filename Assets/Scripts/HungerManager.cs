using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HungerManager : MonoBehaviour, ISaveable<HungerModel>
{
    [SerializeField]
    private DriverBehavior driver;

    [SerializeField]
    private MeterController meterController;

    [SerializeField]
    private GameOverScriptController gameOverScriptController;

    private float maxSatiation = 100f;
    private float currentSatiation = 100f;

    private bool isMoving = false;
    private float movementSpeed = 0f;

    private void Awake()
    {
        SaveManager.Instance.OnSaveRequested += Save;
    }

    private void Start()
    {
        RestoreStateFromModel();

        driver.OnMovementStarted += (float movementSpeed) =>
        {
            this.movementSpeed = movementSpeed;
            isMoving = true;
        };

        driver.OnMovementStopped += () =>
        {
            this.movementSpeed = 0f;
            isMoving = false;
        };
    }

    private void Update()
    {
        if (isMoving)
        {
            currentSatiation -= movementSpeed / 250 * Time.deltaTime;

            if (currentSatiation <= 0f)
            {
                gameOverScriptController.gameObject.SetActive(true);
                gameOverScriptController.UpdateHorseDeathDescription();
                return;
            }
        }

        currentSatiation = Mathf.Clamp(currentSatiation, 0f, maxSatiation);
        meterController.ChangeFill(currentSatiation / maxSatiation);
    }

    public void IncreaseSatiation()
    {
        currentSatiation += 10f;
    }

    public void RestoreStateFromModel()
    {
        HungerModel? hungerModel = SaveManager.GetHungerModel();

        if (hungerModel == null)
        {
            return;
        }

        maxSatiation = hungerModel.Value.MaxSatiation;
        currentSatiation = hungerModel.Value.CurrentSatiation;
    }

    public void Save(bool writeImmediately = false)
    {
        SaveManager.SaveHunger(this, writeImmediately);
    }

    public HungerModel ToModel()
    {
        return new HungerModel(maxSatiation, currentSatiation);
    }
}
