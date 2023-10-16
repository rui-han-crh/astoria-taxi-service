using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

///<summary>
/// Dictates the passenger behaviour at any point in time.
/// 
/// The passenger may either be wandering, hailing a taxi, or approaching a taxi.
/// 
/// The passenger will always wander for a random amount of time, then either hail a taxi or continue wandering.
/// 
/// The chance of hailing a taxi is determined by the PROBABILITY constant.
/// 
/// If the passenger hails a taxi, they will wait for a random amount of time, if the taxi does not enter the
/// radius of the passenger during this time, the passenger will cancel the hail and continue wandering.
/// 
/// Otherwise, if the taxi does enter the radius of the passenger, the passenger will approach the taxi.
/// 
/// At this point, if the taxi continues to move away from the passenger, the passenger will cancel the hail and
/// go back to wandering.
/// 
/// Note that if one passenger is already approaching the taxi, other passengers will cease to hail the taxi, until
/// that passenger has either boarded the taxi or cancelled the hail.
///</summary>
public class PassengerBehaviour : MonoBehaviour
{
    private static readonly float TAXI_PASSENGER_DISTANCE_THRESHOLD = 10f;
    private static readonly float APPROACH_THRESHOLD = 0.1f;
    private static readonly float PROBABILITY = 0.1f;

    private Passenger passenger;

    public Passenger Passenger => passenger;

    public PassengerState state;

    private float timeLeftInState = 0f;

    private Taxi taxi;

    [SerializeField]
    private GameObject greenCircle;

    private NpcRoamBehaviour npcRoamBehaviour;
    private NavmeshAgentMovement navmeshAgentMovement;

    private void Awake()
    {
        taxi = GameObject.FindGameObjectWithTag("Player").GetComponent<Taxi>();
        npcRoamBehaviour = GetComponent<NpcRoamBehaviour>();
        navmeshAgentMovement = GetComponent<NavmeshAgentMovement>();

        greenCircle.SetActive(false);

        passenger = new Passenger(name, $"Clients/{name}");
    }

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

    /// <summary>
    /// Switches the passenger to the drop off state and causes the passenger to path to the house.
    /// 
    /// Once the passenger reaches the house, the passenger will despawn.
    /// </summary>
    /// <param name="targetDestination"></param>
    public void SwitchToDropOffState(Vector3 targetDestination)
    {
        navmeshAgentMovement.SetDestination(targetDestination);
        navmeshAgentMovement.OnDestinationReached += (_) => Destroy(gameObject);
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
