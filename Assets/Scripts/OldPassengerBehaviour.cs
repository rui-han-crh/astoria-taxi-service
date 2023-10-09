using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavmeshAgentMovement))]
public class OldPassengerBehaviour : MonoBehaviour
{
    private Vector3 initialPosition;

    [SerializeField]
    private float distanceThreshold = 10f;

    private NavmeshAgentMovement navmeshAgentMovement;

    private Passenger passenger;

    public event Action OnWalkDestinationReached;

    public event Action OnPickUpFailed;

    public Passenger Passenger => passenger;

    public void SetPassenger (Passenger passenger)
    {
        this.passenger = passenger;
    }

    private void Awake()
    {
        navmeshAgentMovement = GetComponent<NavmeshAgentMovement>();

        // Save the initial position of the passenger.
        // If the passenger walks too far away, the passenger will return to the initial position.
        initialPosition = transform.position;
    }

    public void WalkToPickUpPosition(Transform pickUpTransform)
    {
        navmeshAgentMovement.SetDestination(pickUpTransform.position);

        void InvokePickUpReachedAndUnsubscribe(NavMeshAgent agent)
        {
            OnWalkDestinationReached?.Invoke();
            navmeshAgentMovement.OnDestinationReached -= InvokePickUpReachedAndUnsubscribe;
            OnWalkDestinationReached = null;
        }

        float initialDistance = Vector2.Distance(initialPosition, pickUpTransform.position);

        //StartCoroutine(LimitWalkDistance(InvokePickUpReachedAndUnsubscribe, initialDistance, pickUpTransform));

        navmeshAgentMovement.OnDestinationReached += InvokePickUpReachedAndUnsubscribe;
    }

    private IEnumerator LimitWalkDistance(
        Action onDestinationReached,
        float initialDistance,
        Transform pickUpTransform)
    {
        while (true)
        {
            float currentDistance = Vector2.Distance(initialPosition, pickUpTransform.position);

            if (currentDistance > initialDistance + distanceThreshold)
            {
                navmeshAgentMovement.SetDestination(initialPosition);

                //navmeshAgentMovement.OnDestinationReached -= onDestinationReached;
                OnWalkDestinationReached = null;

                OnPickUpFailed?.Invoke();

                OnPickUpFailed = null;

                break;
            }

            yield return null;
        }
    }

    public void WalkToClientPoint(ClientPoint clientPoint)
    {
        navmeshAgentMovement.SetDestination(clientPoint.transform.position);

        void InvokeDestinationReachedAndUnsubscribe()
        {
            OnWalkDestinationReached?.Invoke();
            //navmeshAgentMovement.OnDestinationReached -= InvokeDestinationReachedAndUnsubscribe;
        }

        //navmeshAgentMovement.OnDestinationReached += InvokeDestinationReachedAndUnsubscribe;
    }
}
