using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NpcCarriageGenerator : PrefabGenerator
{
    [SerializeField]
    private GameObject npcCarriagePrefab;

    private readonly NavMeshAreas roadArea = NavMeshAreas.Road;

    protected override void Generate(int numberOfInstances, float minDistance = 20, float maxDistance = 40)
    {
        float angleIntervals = 360 / numberOfInstances;

        // Find a random point between minDistance and maxDistance away from the player
        Vector3 randomDirection = Random.onUnitSphere;
        Vector3 randomPoint = playerTransform.position + randomDirection * Random.Range(minDistance, maxDistance);

        // Find the closest point on the NavMesh to that random point
        Vector3 closestPoint = GetClosestPointOnNavMesh(10, randomPoint);

        if (Vector3.Distance(closestPoint, playerTransform.position) >= minDistance)
        {
            // Instantiate the NPC
            Instantiate(npcCarriagePrefab, closestPoint, Quaternion.identity);
        }

        for (int i = 0; i < numberOfInstances - 1; i++)
        {
            Vector3 transformToLastPoint = closestPoint - playerTransform.position;

            Vector3 nextPointVector = (Quaternion.Euler(0, 0, angleIntervals) * transformToLastPoint).normalized;

            Vector3 nextPoint = playerTransform.position + nextPointVector * Random.Range(minDistance, maxDistance);

            closestPoint = GetClosestPointOnNavMesh(10, nextPoint);

            if (Vector3.Distance(closestPoint, playerTransform.position) >= minDistance)
            {
                // Instantiate the NPC
                Instantiate(npcCarriagePrefab, closestPoint, Quaternion.identity);
            }
        }
    }

    private Vector3 GetClosestPointOnNavMesh(int radius, Vector3 point)
    {
        // Get the closest point on the NavMesh to the random point
        if (!NavMesh.SamplePosition(point, out NavMeshHit hit, radius, (AreaMask)roadArea))
        {
            // If the point is not on the NavMesh, then try again with a larger radius
            return GetClosestPointOnNavMesh(radius * 2, point);
        }

        return hit.position;
    }
}
