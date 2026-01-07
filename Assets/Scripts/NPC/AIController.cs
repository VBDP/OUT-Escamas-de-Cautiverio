using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform[] points;
    private int currentPoint = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        GoNextPoint();
    }


    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            GoNextPoint();
        }
    }

    void GoNextPoint()
    {
        if (points.Length == 0) return;

        agent.SetDestination(points[currentPoint].position);
        currentPoint = (currentPoint + 1) % points.Length;
    }
}
