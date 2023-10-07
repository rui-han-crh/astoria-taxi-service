using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D), typeof(SpriteRenderer))]
public class TaxiPoint : MonoBehaviour
{
    protected static readonly string PLAYER_TAG = "Player";

    [SerializeField]
    private ClientPoint clientPoint;

    [SerializeField]
    private Sprite indicatorSprite;

    protected SpriteRenderer spriteRenderer;
    protected CircleCollider2D trigger;

    public ClientPoint ClientPoint => clientPoint;

    private bool isPickUp = false;

    private Coroutine disableIndicatorCoroutine;

    public bool IsPickUp => isPickUp;

    public event Action onDisablePickUpIndicator;

    public void SetIsPickUp(bool isPickUp)
    {
        this.isPickUp = isPickUp;

        if (isPickUp)
        {
            disableIndicatorCoroutine = StartCoroutine(DisableIndicator());
        }
    }

    private IEnumerator DisableIndicator()
    {
        yield return new WaitForSeconds(5f);

        HideIndicator();
        isPickUp = false;

        ClientPoint.DespawnClient();

        onDisablePickUpIndicator?.Invoke();

        onDisablePickUpIndicator = null;
    }

    public void ShowPickUpIndicator()
    {
        ShowIndicator();
        spriteRenderer.color = Color.green;
    }

    public void ShowDropOffIndicator()
    {
        ShowIndicator();
        spriteRenderer.color = Color.red;
    }

    public void ShowIndicator()
    {
        spriteRenderer.sprite = indicatorSprite;

        spriteRenderer.sortingLayerName = "UI";
        spriteRenderer.enabled = true;

        trigger.enabled = true;
    }

    public void HideIndicator()
    {
        spriteRenderer.enabled = false;

        trigger.enabled = false;
    }

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        trigger = GetComponent<CircleCollider2D>();

        trigger.enabled = false;
        spriteRenderer.enabled = false;

        // Determine if it is a destination point for an ongoing trip
        if (SaveManager.GetTripState()?.TaxiPointLocation != null && 
            SaveManager.GetTripState().TaxiPointLocation.isSameTaxiPoint(this))
        {
            ShowDropOffIndicator();
        }
    }


    public virtual void Update()
    {
        if (spriteRenderer.enabled)
        {
            transform.Rotate(Vector3.forward, 50 * Time.deltaTime);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(PLAYER_TAG))
        {
            // Hide the indicator
            HideIndicator();

            // Find the carriage body transform
            TaxiTripManager taxiTripManager = collision.transform.root
                .GetComponentInChildren<TaxiTripManager>();

            if (isPickUp)
            {
                PerformPickUpEvents(taxiTripManager);
            } else
            {
                PerformDropOffEvents(taxiTripManager);
            }
        }
    }

    private void PerformPickUpEvents(TaxiTripManager taxiTripManager)
    {
        StopCoroutine(disableIndicatorCoroutine);

        // Perform actions when the passenger reaches the pick up position
        clientPoint.PassengerBehaviour.OnWalkDestinationReached += () =>
        {
            // Start the fare counting
            FareComputationManager.instance.StartFareComputatation(51);

            // Add the passenger to the taxi trip manager
            taxiTripManager.AddPassengerGameObject(clientPoint.PassengerBehaviour.gameObject);

            // All active pick up indicators should be hidden
            taxiTripManager.DisablePickUpIndicators();

            TaxiPoint dropOffPoint = taxiTripManager.ChooseRandomDestination(transform.position);

            dropOffPoint.ShowDropOffIndicator();

            // Disable the visibility of the passenger game object
            clientPoint.PassengerBehaviour.gameObject.SetActive(false);
        };

        // Perform actions when the passenger fails to reach the pick up position
        clientPoint.PassengerBehaviour.OnPickUpFailed += () =>
        {
            // Show the pick up indicator
            ShowPickUpIndicator();
            disableIndicatorCoroutine = StartCoroutine(DisableIndicator());
        };

        clientPoint.PassengerBehaviour.WalkToPickUpPosition(taxiTripManager.CarriageBody);
    }

    private void PerformDropOffEvents(TaxiTripManager taxiTripManager)
    {
        foreach (GameObject passengerGameObject in taxiTripManager.PassengerGameObjects)
        {
            // Stop the fare counting
            FareComputationManager.instance.EndFareComputation();

            // Reenable the visibility of the passenger game object
            passengerGameObject.SetActive(true);

            // Move the passenger to the carriage body
            passengerGameObject.transform.position = taxiTripManager.CarriageBody.position;

            PassengerBehaviour passengerBehaviour = passengerGameObject
                .GetComponent<PassengerBehaviour>();

            passengerBehaviour.OnWalkDestinationReached += () =>
            {
                // Destroy the passenger game object
                Destroy(passengerGameObject);
            };

            passengerBehaviour.WalkToClientPoint(clientPoint);

            // Remove the passenger from the taxi trip manager
            taxiTripManager.RemovePassengerGameObject(passengerBehaviour.gameObject);

            // Remove the trip from TripState
            SaveManager.GetTripState().EndTrip();
        }
    }
}
