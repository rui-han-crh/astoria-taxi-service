using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

///<summary>
/// Hails the player taxi with a certain probability every few seconds.
///</summary>
public class PassengerBehaviour : MonoBehaviour
{
    private static GameObject[] CLIENT_OBJECTS;
    private static readonly float TAXI_PASSENGER_DISTANCE_THRESHOLD = 10f;
    private static readonly float APPROACH_THRESHOLD = 0.1f;
    private static readonly float PROBABILITY = 0.1f;

    private readonly Passenger passenger = new Passenger();

    public PassengerState state;

    private float timeLeftInState = 0f;

    private Taxi taxi;

    [SerializeField]
    private GameObject greenCircle;

    private NpcRoamBehaviour npcRoamBehaviour;
    private NavmeshAgentMovement navmeshAgentMovement;

    private void Awake()
    {
        CLIENT_OBJECTS ??= Resources.LoadAll<GameObject>("Clients");

        taxi = GameObject.FindGameObjectWithTag("Player").GetComponent<Taxi>();
        npcRoamBehaviour = GetComponent<NpcRoamBehaviour>();
        navmeshAgentMovement = GetComponent<NavmeshAgentMovement>();

        greenCircle.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        switch (state)
        {
            case PassengerState.Wandering:
                timeLeftInState -= Time.deltaTime;
                if (timeLeftInState <= 0f)
                {
                    if (Random.Range(0f, 1f) < PROBABILITY && !taxi.HasPassenger())
                    {
                        SwitchToHailState();
                    }
                    else
                    {
                        SwitchToWanderState();
                    }
                }
                break;

            case PassengerState.Hailing:
                if (taxi.HasPassengerApproaching())
                {
                    greenCircle.SetActive(false);
                    break;
                }

                greenCircle.SetActive(true);

                timeLeftInState -= Time.deltaTime;
                if (timeLeftInState <= 0f)
                {
                    greenCircle.SetActive(false);
                    SwitchToWanderState();
                }
                break;

            case PassengerState.Approaching:
                if (Vector2.Distance(transform.position, taxi.transform.position) > TAXI_PASSENGER_DISTANCE_THRESHOLD)
                {
                    taxi.CancelHail(this);
                    SwitchToWanderState();
                    break;
                }

                if (Vector2.Distance(transform.position, taxi.transform.position) < APPROACH_THRESHOLD)
                {
                    BoardTaxi();
                    break;
                }

                navmeshAgentMovement.SetDestination(taxi.transform.position);
                break;
        }
    }

    public void SwitchToHailState()
    {
        npcRoamBehaviour.enabled = false;
        navmeshAgentMovement.StopMoving();
        HailTaxi();
        state = PassengerState.Hailing;
        timeLeftInState = 10f;
    }

    public void SwitchToWanderState()
    {
        npcRoamBehaviour.enabled = true;
        state = PassengerState.Wandering;
        timeLeftInState = 5f;
    }

    public void SwitchToApproachState()
    {
        greenCircle.SetActive(false);
        npcRoamBehaviour.enabled = false;
        state = PassengerState.Approaching;
    }

    private void HailTaxi()
    {
        if (taxi.Hail())
        {
            Debug.Log("Hailling taxi!");
            greenCircle.SetActive(true);
        }
        else
        {
            Debug.Log("Someone else is already approaching the taxi, or is already in the taxi.");
        }
    }

    private void BoardTaxi()
    {
        Debug.Log("Boarding taxi!");

        taxi.Board(this);

        state = PassengerState.Boarded;

        gameObject.transform.SetParent(taxi.transform);
        gameObject.SetActive(false);

        enabled = false;
    }
}
