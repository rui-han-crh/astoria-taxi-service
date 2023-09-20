using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Action = System.Action;

public class NavmeshAgentMovement : MonoBehaviour
{
    private Transform target;
    private NavMeshAgent agent;

    [SerializeField]
    [Tooltip("The threshold of the agent from the waypoint position before it is considered reached.")]
    private float approachThreshold = 0.1f;

    public event Action OnDestinationReached;

    private Transform ownedTarget;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        ownedTarget = new GameObject($"navmesh-target-{name}").transform;
        ownedTarget.position = transform.position;

        target = ownedTarget;
    }

    private void Update()
    {
        SetAgentPosition();

        if (HasReachedDestination())
        {
            OnDestinationReached?.Invoke();
        }
    }

    private bool HasReachedDestination()
    {
        return Vector2.Distance(transform.position, target.position) < approachThreshold;
    }

    public void SetDestination(Transform target)
    {
        this.target = target;
    }

    public void SetDestination(Vector3 position)
    {
        ownedTarget.position = position;
        SetDestination(ownedTarget);
    }

    private void SetAgentPosition()
    {
        agent.SetDestination(new Vector3(target.position.x, target.position.y, transform.position.z));
    }
}
