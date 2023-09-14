using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(NavmeshAgentMovement))]
    internal class NpcRoamBehaviour: MonoBehaviour
    {
        [SerializeField]
        private Transform[] waypoints;

        private NavmeshAgentMovement agentMovement;

        public void Awake()
        {
            agentMovement = GetComponent<NavmeshAgentMovement>();
            agentMovement.SetDestination(SelectRandomWayPoint().position);

            agentMovement.OnDestinationReached += OnDestinationReached;
        }

        private void OnDestinationReached()
        {
            print("Destination reached");
            agentMovement.SetDestination(SelectRandomWayPoint().position);
        }

        private Transform SelectRandomWayPoint()
        {
            int randomIndex = Random.Range(0, waypoints.Length);
            return waypoints[randomIndex];
        }
    }
}
