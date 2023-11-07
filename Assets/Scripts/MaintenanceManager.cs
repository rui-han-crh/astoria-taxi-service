using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaintenanceManager : MonoBehaviour, ISaveable<MaintenanceModel>
{
    private float maxMaintenance = 100f;
    private float currentMaintenance = 100f;

    [SerializeField]
    private MeterController meterController;

    [SerializeField]
    private CollisionEventEmitter horse;

    [SerializeField]
    private CollisionEventEmitter carriage;

    [SerializeField]
    private GameOverScriptController gameOverScriptController;

    private Rigidbody2D horseRb;

    private void Start()
    {
        horse.OnCollisionGameObject += CheckCollision;
        carriage.OnCollisionGameObject += CheckCollision;

        horseRb = horse.GetComponent<Rigidbody2D>();

        SaveManager.Instance.OnSaveRequested += Save;
    }

    private void CheckCollision(GameObject other)
    {
        if (other.transform.root == carriage.transform.root || other.transform.root == horse.transform.root)
        {
            return;
        }

        Rigidbody2D otherRb = other.GetComponent<Rigidbody2D>();

        float otherSpeed = otherRb == null ? 0 : otherRb.velocity.magnitude;

        currentMaintenance -= Mathf.Max(0, (horseRb.velocity.magnitude + otherSpeed));
        meterController.ChangeFill(currentMaintenance / maxMaintenance);

        if (currentMaintenance <= 0f)
        {
            gameOverScriptController.gameObject.SetActive(true);
            gameOverScriptController.UpdateCarriageDeathDescription();
        }
    }

    public void Repair()
    {
        currentMaintenance = Mathf.Min(currentMaintenance + 1, maxMaintenance);
        meterController.ChangeFill(currentMaintenance / maxMaintenance);
    }

    public bool IsMaxedOut()
    {
        return currentMaintenance >= maxMaintenance;
    }

    public void RestoreStateFromModel()
    {
        MaintenanceModel? model = SaveManager.GetMaintenanceModel();

        if (model == null)
        {
            return;
        }

        maxMaintenance = model.Value.MaxMaintenance;
        currentMaintenance = model.Value.CurrentMaintenance;
        meterController.ChangeFill(currentMaintenance / maxMaintenance);
    }

    public void Save(bool writeImmediately = false)
    {
        SaveManager.SaveMaintenance(this, writeImmediately);
    }

    public MaintenanceModel ToModel()
    {
        return new MaintenanceModel(maxMaintenance, currentMaintenance);
    }
}
