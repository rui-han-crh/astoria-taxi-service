using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class NavmeshAgentAnimationController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;

    [SerializeField]
    private string xParam = "x";

    [SerializeField]
    private string yParam = "y";

    private int xHash;
    private int yHash;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        xHash = Animator.StringToHash("x");
        yHash = Animator.StringToHash("y");
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat(xHash, agent.velocity.x);
        animator.SetFloat(yHash, agent.velocity.y);
    }
}
