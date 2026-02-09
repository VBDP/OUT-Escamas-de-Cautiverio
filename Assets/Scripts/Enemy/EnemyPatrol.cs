using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyPatrol : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;

    [Header("Patrol")] [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float waitTimeAtPoint = 3f;
    [SerializeField] private float rotationSpeed = 5f;
    private int currentPointIndex = 0;

    [Header("Vision")] [SerializeField] private Transform player;
    [SerializeField] private float viewDistance = 10f;
    [SerializeField] private float viewAngle = 60f;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private float losePlayerTime = 3f;


    [Header("Attack")] [SerializeField] private float attackDistance = 1.5f;
    [SerializeField] private float attackCooldown = 1.2f;
    private float attackTimer = 0f;
    private bool isAttacking = false;

    private bool chasingPlayer = false;
    private float losePlayerTimer = 0f;
    private Coroutine patrolCoroutine;

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;

        patrolCoroutine = StartCoroutine(PatrolRoutine());
    }

    void Update()
    {
        if (chasingPlayer)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // ATAQUE
            if (distanceToPlayer <= attackDistance && attackTimer <= 0f)
            {
                StartCoroutine(AttackRoutine());
                return;
            }

            // PERSECUCIÓN
            if (!isAttacking)
            {
                agent.SetDestination(player.position);
                animator.SetFloat("Speed", agent.velocity.magnitude / agent.speed);
                RotateTowards(player.position);
            }

            // Pérdida del jugador por tiempo
            if (!CanSeePlayer())
            {
                losePlayerTimer -= Time.deltaTime;

                if (losePlayerTimer <= 0f)
                {
                    chasingPlayer = false;
                    patrolCoroutine = StartCoroutine(PatrolRoutine());
                }
            }
            else
            {
                losePlayerTimer = losePlayerTime;
            }

            attackTimer -= Time.deltaTime;
            return;
        }

        // INICIAR PERSECUCIÓN
        if (CanSeePlayer())
        {
            chasingPlayer = true;
            losePlayerTimer = losePlayerTime;

            if (patrolCoroutine != null)
                StopCoroutine(patrolCoroutine);
        }

        HandleRotation();
    }


    private void HandleRotation()
    {
        Vector3 velocity = agent.velocity;
        velocity.y = 0;

        if (velocity.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(velocity);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }

    private void RotateTowards(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0;

        if (direction.sqrMagnitude < 0.01f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    private IEnumerator PatrolRoutine()
    {
        while (true)
        {
            agent.SetDestination(patrolPoints[currentPointIndex].position);

            while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
            {
                float speed = agent.velocity.magnitude / agent.speed;
                animator.SetFloat("Speed", speed);
                yield return null;
            }

            animator.SetFloat("Speed", 0f);

            float timer = waitTimeAtPoint;
            while (timer > 0f)
            {
                if (chasingPlayer)
                    yield break;

                timer -= Time.deltaTime;
                yield return null;
            }

            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
        }
    }

    private bool CanSeePlayer()
    {
        Vector3 origin = transform.position + Vector3.up * 1.6f;
        Vector3 dirToPlayer = player.position - origin;

        if (dirToPlayer.magnitude > viewDistance)
            return false;

        float angle = Vector3.Angle(transform.forward, dirToPlayer);
        if (angle > viewAngle * 0.5f)
            return false;

        if (Physics.Raycast(origin, dirToPlayer.normalized, out RaycastHit hit,
                viewDistance, obstacleMask | playerMask))
        {
            return ((1 << hit.collider.gameObject.layer) & playerMask) != 0;
        }

        return false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewDistance);

        Vector3 left = Quaternion.Euler(0, -viewAngle / 2, 0) * transform.forward;
        Vector3 right = Quaternion.Euler(0, viewAngle / 2, 0) * transform.forward;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + left * viewDistance);
        Gizmos.DrawLine(transform.position, transform.position + right * viewDistance);
    }

    private IEnumerator AttackRoutine()
    {
        isAttacking = true;
        attackTimer = attackCooldown;

        agent.isStopped = true;
        animator.SetFloat("Speed", 0f);
        animator.SetTrigger("Attack"); // Trigger en el Animator

        // Mirar al jugador mientras ataca
        float attackDuration = 0.6f; // ajusta a tu animación
        float timer = 0f;

        while (timer < attackDuration)
        {
            RotateTowards(player.position);
            timer += Time.deltaTime;
            yield return null;
        }

        agent.isStopped = false;
        isAttacking = false;
    }
}