using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This script is used to automatically create carrots are random spots
/// in the scene. The carrots are placed off-camera but within a specified
/// radius of the player.
/// </summary>
public class CarrotGenerator : PrefabGenerator
{
    [SerializeField]
    private GameObject carrotPrefab;

    private NavMeshAreas roadNavmesh = NavMeshAreas.Road;

    /// <summary>
    /// Generates some number of carrots at random locations.
    /// 
    /// The carrots are only placed on the road navmesh.
    /// </summary>
    protected override void Generate(int numberOfCarrots, float minDistance, float maxDistance)
    {
        for (int i = 0; i < numberOfCarrots; i++)
        {
            // Generate a random point in a circle around the player
            Vector2 randomPoint = Random.insideUnitCircle.normalized * Random.Range(minDistance, maxDistance);
            Vector2 position = playerTransform.position + new Vector3(randomPoint.x, randomPoint.y, 0f);

            // Find the closest point on the navmesh to that position
            if (NavMesh.SamplePosition(position, out NavMeshHit hit, 30f, (AreaMask)roadNavmesh))
            {
                // Create a carrot at that point
                Instantiate(carrotPrefab, hit.position, Quaternion.identity);
            }
        }
    }
}
