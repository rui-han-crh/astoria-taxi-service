using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

///<summary>
/// <para>Dictates the passenger behaviour at any point in time.</para>
/// 
/// <para>The passenger may either be wandering, hailing a taxi, or approaching a taxi.</para>
/// 
/// <para>The passenger will always wander for a random amount of time, then either hail a taxi or continue wandering.</para>
/// 
/// <para>The chance of hailing a taxi is determined by the PROBABILITY constant.</para>
/// 
/// <para>If the passenger hails a taxi, they will wait for a random amount of time, if the taxi does not enter the
/// radius of the passenger during this time, the passenger will cancel the hail and continue wandering.</para>
/// 
/// <para>Otherwise, if the taxi does enter the radius of the passenger, the passenger will approach the taxi.</para>
/// 
/// <para>At this point, if the taxi continues to move away from the passenger, the passenger will cancel the hail and
/// go back to wandering.</para>
/// 
/// <para>Note that if one passenger is already approaching the taxi, other passengers will cease to hail the taxi, until
/// that passenger has either boarded the taxi or cancelled the hail.</para>
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

        passenger = new Passenger(name, $"Clients/Default-Passenger");
    }

    public static PassengerBehaviour CreateFromSaveFile()
    {
        PassengerBehaviourModel passengerBehaviourModel = SaveManager.LoadPassengerBehaviourModel();

        string reference = passengerBehaviourModel.Passenger.ResourceFileReference;

        print($"Constructing passenger from reference: {reference}");

        GameObject passengerGameObject = ResourceManager.Instance.Load<GameObject>(reference);

        PassengerBehaviour passengerBehaviour = passengerGameObject.GetComponent<PassengerBehaviour>();

        passengerBehaviour.passenger = passengerBehaviourModel.Passenger;
        passengerBehaviour.state = passengerBehaviourModel.PassengerState;
        passengerBehaviour.timeLeftInState = passengerBehaviourModel.TimeLeftInState;

        return passengerBehaviour;
    }

    public PassengerBehaviourModel ToPassengerBehaviourModel()
    {
        return new PassengerBehaviourModel(passenger, state, timeLeftInState);
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
        navmeshAgentMovement.StopMoving();
        HailTaxi();
        state = PassengerState.Hailing;
        timeLeftInState = 10f;
        npcRoamBehaviour.enabled = false;
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
