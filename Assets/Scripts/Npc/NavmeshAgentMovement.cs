using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Action = System.Action;

/// <summary>
/// Moves the game object to a target position using the NavMeshAgent.
/// 
/// The OnDestinationReached event can be subscribed to to get notified when the 
/// target position is reached.
/// Once the target position is reached, the OnDestinationReached event is invoked.
/// </summary>
public class NavmeshAgentMovement : MonoBehaviour
{
    private static readonly float APPROACH_THRESHOLD = 0.1f;

    private NavMeshAgent agent;

    public delegate void OnDestinationReachedDelegate(NavMeshAgent agent);

    /**
     * Calls the delegate when the target position is reached.
     * 
     * This event is invoked once at the start of the game, as the target
     * is initialized to the game object's position.
     */
    public event OnDestinationReachedDelegate OnDestinationReached;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        // Disable rotation and up axis updates because the navmesh agent is used for 2D movement.
        // The area has been rotated by 90 degrees to make the navmesh agent work in 2D so the
        // up axis is not the y axis.
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        agent.SetDestination(transform.position);
    }

    private void Update()
    {
        if (HasReachedDestination())
        {
            print("Reached destination");
            OnDestinationReached?.Invoke(agent);
        }
    }

    private bool HasReachedDestination()
    {
        return Vector2.Distance(transform.position, agent.destination) < APPROACH_THRESHOLD;
    }

    public void SetDestination(Vector3 target)
    {
        agent.SetDestination(target);
    }

    public void SetAreaCost(int area, int cost)
    {
        agent.SetAreaCost(area, cost);
    }

    public void StopMoving()
    {
        agent.SetDestination(transform.position);
    }
}
