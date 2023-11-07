using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairPoint : MonoBehaviour
{
    private InputActions inputActions;

    private GameObject gameObjectOnTrigger = null;

    [SerializeField]
    private MaintenanceManager maintenanceManager;

    [SerializeField]
    private Wallet wallet;

    private Coroutine repairCoroutine;

    private void Start()
    {
        inputActions = new InputActions();

        inputActions.Player.Interact.performed += ctx => repairCoroutine = StartCoroutine(Repair());
        inputActions.Player.Interact.canceled += ctx =>
        {
            if (repairCoroutine != null)
            {
                StopCoroutine(repairCoroutine);
            }
        };
    }

    private void OnEnable()
    {
        inputActions?.Enable();
    }

    private void OnDisable()
    {
        inputActions?.Disable();
    }

    private IEnumerator Repair()
    {
        while (true)
        {
            if (wallet.GetBalance() < 5 || maintenanceManager.IsMaxedOut())
            {
                break;
            }

            maintenanceManager.Repair();
            wallet.DecreaseBalance(5);
            wallet.UpdateDisplay();
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObjectOnTrigger != null)
        {
            return;
        }

        if (collision.gameObject.CompareTag(Tags.Horses) || collision.gameObject.CompareTag(Tags.CarriageBody))
        {
            inputActions.Enable();
            gameObjectOnTrigger = collision.gameObject;
        } 
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (gameObjectOnTrigger == collision.gameObject)
        {
            inputActions.Disable();
            gameObjectOnTrigger = null;
        }
    }
}
