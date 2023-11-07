using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NpcDriverBehaviour : DriverBehavior
{
    private readonly NavMeshAreas roadNavMeshArea = NavMeshAreas.Road;
    private Vector2[] corners = new Vector2[0];

    [SerializeField]
    private float paddingAmount = 2f;

    private Vector3 destination;
    private int currentCornerIndex = 0;

    protected override void Start()
    {
        base.Start();
        
        // Select a point outside of the map, then find the closest position to the navmesh
        Vector2 randomPoint = UnityEngine.Random.onUnitSphere * 256f;

        if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 512f, (AreaMask)roadNavMeshArea))
        {
            // Set the path to the closest point to the navmesh
            destination = hit.position;
            SetPath(destination);
        } else
        {
            Debug.LogError("Could not find a point on the navmesh");
        }

        TriggerMovement((corners[currentCornerIndex] - (Vector2)transform.position).normalized);
    }

    private void SetPath(Vector3 destination)
    {
        currentCornerIndex = 0;
        NavMeshPath path = new NavMeshPath();
        // Calculate the path to the destination
        if (!NavMesh.CalculatePath(transform.position, destination, (AreaMask)roadNavMeshArea, path))
        {
            return;
        }

        // Pad the corners of the path
        corners = new Vector2[path.corners.Length];

        corners[0] = path.corners[0];
        corners[^1] = path.corners[^1];

        for (int i = 1; i < corners.Length - 1; i++)
        {
            // Find the unit vector difference between the current corner and the previous corner
            Vector3 cornerDifferenceBefore = (path.corners[i - 1] - path.corners[i]).normalized;

            // Find the unit vector difference between the current corner and the next corner
            Vector3 cornerDifferenceAfter = (path.corners[i + 1] - path.corners[i]).normalized;

            // Find the bisection vector of the angle between the two vectors
            Vector3 bisection = (cornerDifferenceBefore + cornerDifferenceAfter).normalized;

            corners[i] = path.corners[i] - bisection * paddingAmount;
        }
    }

    private void Update()
    {
        if (corners.Length == 0)
        {
            StopMoving();
            SetPath(destination);
            return;
        }

        if (IsBlocked(distance: 2f) && !isReversing)
        {
            isReversing = true;
            StartCoroutine(StopReversing());
            return;
        }

        float threshold = currentCornerIndex == corners.Length - 1 ? 0.1f : 3f;

        if (Vector2.Distance(transform.position, corners[currentCornerIndex]) <= threshold)
        {
            currentCornerIndex++;
            if (currentCornerIndex >= corners.Length)
            {
                Destroy(gameObject.transform.root.gameObject);
                return;
            }

            TriggerMovement((corners[currentCornerIndex] - (Vector2)transform.position).normalized);
        }
    }

    private IEnumerator StopReversing()
    {
        yield return new WaitForSeconds(1f);
        isReversing = false;
        SetPath(destination);
    }

    private bool IsBlocked(float distance)
    {
        // Check if the vehicle is blocked by any colliders
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, distance);

        return hit.collider != null && !hit.collider.isTrigger && hit.transform.root != transform.root;
    }
}
