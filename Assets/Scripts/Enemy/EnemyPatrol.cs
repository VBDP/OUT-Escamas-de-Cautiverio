using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyPatrol : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;

    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float waitTimeAtPoint = 3f;
    [SerializeField] private float rotationSpeed = 5f; // suaviza la rotación

    private int currentPointIndex = 0;

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false; // rotación manual

        if (patrolPoints.Length > 0)
        {
            StartCoroutine(PatrolRoutine());
        }
    }

    void Update()
    {
        // Rotación suave hacia dirección de movimiento
        Vector3 velocity = agent.velocity;
        velocity.y = 0;

        if (velocity.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(velocity);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private IEnumerator PatrolRoutine()
    {
        while (true)
        {
            // Ir al punto actual
            agent.SetDestination(patrolPoints[currentPointIndex].position);

            // Mientras se mueve, actualiza Speed para Blend Tree
            while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
            {
                float speed = agent.velocity.magnitude / agent.speed; // normalizado 0-1
                animator.SetFloat("Speed", speed);
                yield return null;
            }

            // Llegó al punto → Idle
            animator.SetFloat("Speed", 0f);

            // Esperar en el punto
            float timer = waitTimeAtPoint;
            while (timer > 0f)
            {
                animator.SetFloat("Speed", 0f);
                timer -= Time.deltaTime;
                yield return null;
            }

            // Siguiente punto
            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
        }
    }
}
