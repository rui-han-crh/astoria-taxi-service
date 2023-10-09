using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// If this script is enabled, then the game object will find the closest point in
/// a unit circle and on the navmesh area it is allowed on and move to that point.
///
/// When the game object reaches the point, it will find a new point and move to that point.
/// </summary>
public class NpcRoamBehaviour : MonoBehaviour
{
    [SerializeField]
    private NavMeshAreas navMeshAreas;

    [SerializeField]
    private int[] areaCosts = new int[32];

    private NavmeshAgentMovement navmeshAgentMovement;

    private void Awake()
    {
        navmeshAgentMovement = GetComponent<NavmeshAgentMovement>();
    }

    private void Start()
    {
        navmeshAgentMovement.OnDestinationReached += FindNewDestination;

        IEnumerable<NavMeshAreas> walkableAreas = System.Enum.GetValues(typeof(NavMeshAreas))
            .Cast<NavMeshAreas>()
            .Where(area => area != NavMeshAreas.None && area != NavMeshAreas.All);

        // Set the cost of moving through each area
        foreach (NavMeshAreas area in walkableAreas)
        {
            if ((navMeshAreas & area) == 0)
            {
                navmeshAgentMovement.SetAreaCost((int)area, 1000000000);
            }
        }
    }

    private void FindNewDestination(NavMeshAgent agent)
    {
        // From the current position, find a random point in a unit circle
        Vector2 randomPoint = Random.insideUnitCircle * 30f;

        Vector2 position = transform.position + new Vector3(randomPoint.x, randomPoint.y, 0f);

        // Then find the closest point on the navmesh to that point
        Vector3 randomPointOnNavmesh = GetRandomPointOnNavmesh(position);

        agent.SetDestination(randomPointOnNavmesh);
    }

    private Vector3 GetRandomPointOnNavmesh(Vector2 randomPoint)
    {
        NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 30f, (AreaMask)navMeshAreas);

        return hit.position;
    }

    private void OnDestroy()
    {
        navmeshAgentMovement.OnDestinationReached -= FindNewDestination;
    }

    private void OnEnable()
    {
        navmeshAgentMovement.SetDestination(transform.position);
    }
}
