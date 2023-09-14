using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Action = System.Action;

namespace Assets.Scripts
{
    internal class NavmeshAgentMovement : MonoBehaviour
    {
        private Vector3 destination;
        private NavMeshAgent agent;

        [SerializeField]
        [Tooltip("The threshold of the agent from the waypoint position before it is considered reached.")]
        private float threshold = 0.1f;

        public event Action OnDestinationReached;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            agent.updateRotation = false;
            agent.updateUpAxis = false;
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
            return Vector2.Distance(transform.position, destination) < threshold;
        }

        public void SetDestination(Vector3 destination)
        {
            this.destination = destination;
        }

        private void SetAgentPosition()
        {
            agent.SetDestination(new Vector3(destination.x, destination.y, transform.position.z));
        }
    }
}
