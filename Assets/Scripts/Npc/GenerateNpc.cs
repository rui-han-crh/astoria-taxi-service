using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

/**
 * Generates an NPC in a point that is some distance away from the player.
 * 
 * The NPC will be placed on closest point of the NavMesh area that corresponds
 * to its walkable area.
 * 
 * This script will hold a reference to to the player location so that it can
 * find a point that is some distance away from the player.
 * 
 * The generation takes place once at the start. Then, at random intervals,
 * generations will take place again.
 */
public class GenerateNpc : MonoBehaviour
{
    private static readonly string PLAYER_TAG = "Player";
    private static readonly string PEDESTRIAN_AREA_STRING = "Pavement";

    private Transform playerTransform;

    private float timeToNextGeneration = 3f;

    private int pedestrianAreaIndex;

    private void Awake()
    {
        // Find the player
        playerTransform = GameObject.FindGameObjectWithTag(PLAYER_TAG).transform;

        // Assign the pedestrian mask
        pedestrianAreaIndex = NavMesh.GetAreaFromName(PEDESTRIAN_AREA_STRING);

        if (pedestrianAreaIndex == -1)
        {
            Debug.LogError($"Could not find pedestrian mask with name {PEDESTRIAN_AREA_STRING}");
        }
    }

    private void Start()
    {
        Generate(5, 0, 20);
    }

    // Update is called once per frame
    private void Update()
    {
        if (timeToNextGeneration <= 0f)
        {
            // Generate some number of NPCs
            int numberOfNpcs = Random.Range(1, 6);

            Generate(numberOfNpcs);

            // Set the time to the next generation
            timeToNextGeneration = Random.Range(1f, 10f);
        }
        else
        {
            // Decrease the time to the next generation
            timeToNextGeneration -= Time.deltaTime;
        }
    }

    private void Generate(int numberOfNpcs, float minDistance = 20, float maxDistance = 40)
    {
        for (int i = 0; i < numberOfNpcs; i++)
        {
            // Get a random point that is some distance away from the player
            Vector3 randomPoint = GetRandomPointAwayFromPlayer(minDistance, maxDistance);

            // Get the closest point on the NavMesh to the random point
            Vector3 closestPoint = GetClosestPointOnNavMesh(10, randomPoint);

            Passenger passenger = Passenger.GetRandomPassenger();

            GameObject passengerPrefab = ResourceManager.Instance.Load<GameObject>(passenger.ResourceFileReference);

            // Instantiate the NPC
            Instantiate(passengerPrefab, closestPoint, Quaternion.identity);
        }
    }

    private Vector3 GetRandomPointAwayFromPlayer(float minDistance, float maxDistance)
    {
        // Get a random point in a unit sphere
        Vector3 randomPoint = RandomPointUnitSphere(playerTransform.position);

        // Get the direction from the player to the random point
        Vector3 direction = randomPoint - playerTransform.position;

        // Get a random distance between 5 and 10
        float distance = Random.Range(minDistance, maxDistance);

        // Get the point that is some distance away from the player
        Vector3 pointAwayFromPlayer = playerTransform.position + direction.normalized * distance;

        return pointAwayFromPlayer;
    }

    private Vector3 GetClosestPointOnNavMesh(int radius, Vector3 point)
    {
        // Get the closest point on the NavMesh to the random point
        if (!NavMesh.SamplePosition(point, out NavMeshHit hit, radius, 1 << pedestrianAreaIndex))
        {
            // If the point is not on the NavMesh, then try again with a larger radius
            return GetClosestPointOnNavMesh(radius * 2, point);
        }

        return hit.position;
    }

    /**
     * Returns a random point on the surface of a unit sphere centered at the
     * provided origin.
     */
    public static Vector3 RandomPointUnitSphere(Vector3 origin)
    {
        Vector3 randDirection = Random.insideUnitSphere;

        randDirection.z = origin.z;

        return randDirection + origin;
    }
}
