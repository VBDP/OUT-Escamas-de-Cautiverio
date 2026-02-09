using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Components")] private NavMeshAgent agent;
    private Animator animator;

    [Header("Patrol")] [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float waitTimeAtPoint = 3f;
    [SerializeField] private float rotationSpeed = 5f;
    private int currentPointIndex = 0;
    private Coroutine patrolCoroutine;

    [Header("Vision")] [SerializeField] private Transform player;
    [SerializeField] private float viewDistance = 10f;
    [SerializeField] private float viewAngle = 60f;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private float losePlayerTime = 3f;
    private bool chasingPlayer = false;
    private float losePlayerTimer = 0f;

    [Header("Attack")] [SerializeField] private float attackCooldown = 1.2f;
    private float attackTimer = 0f;
    private bool isAttacking = false;

    [Header("NavMesh Settings")] [SerializeField]
    private float stoppingDistanceToPlayer = 1f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        agent.updateRotation = false; // controlamos rotación manual
        agent.stoppingDistance = stoppingDistanceToPlayer;

        patrolCoroutine = StartCoroutine(PatrolRoutine());
    }

    void Update()
    {
        if (isAttacking) return;

        if (chasingPlayer)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= agent.stoppingDistance && attackTimer <= 0f)
            {
                StartCoroutine(AttackRoutine());
                return;
            }

            if (distanceToPlayer > agent.stoppingDistance)
            {
                agent.SetDestination(player.position);
                animator.SetFloat("Speed", agent.velocity.magnitude / agent.speed);
            }
            else
            {
                agent.ResetPath();
                animator.SetFloat("Speed", 0f);
            }

            HandleRotation();

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

        if (CanSeePlayer())
        {
            chasingPlayer = true;
            losePlayerTimer = losePlayerTime;
            if (patrolCoroutine != null) StopCoroutine(patrolCoroutine);
        }

        HandleRotation();
    }

    #region Rotation

    private void HandleRotation()
    {
        Vector3 velocity = agent.velocity;
        velocity.y = 0;

        if (velocity.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(velocity);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void RotateTowards(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0;

        if (direction.sqrMagnitude < 0.01f) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    #endregion

    #region Patrol

    private IEnumerator PatrolRoutine()
    {
        while (true)
        {
            // Destino actual
            Transform currentTarget = patrolPoints[currentPointIndex];
            agent.SetDestination(currentTarget.position);

            // Moverse hasta llegar
            while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
            {
                float speed = agent.velocity.magnitude / agent.speed;
                animator.SetFloat("Speed", speed);
                yield return null;
            }

            // Llegó al punto
            animator.SetFloat("Speed", 0f);
            agent.ResetPath(); // detener al agente

            // Girar inmediatamente hacia el siguiente punto
            int nextIndex = (currentPointIndex + 1) % patrolPoints.Length;
            Transform nextTarget = patrolPoints[nextIndex];

            Quaternion startRotation = transform.rotation;
            Quaternion targetRotation = Quaternion.LookRotation((nextTarget.position - transform.position).normalized);

            float rotateTime = 1f; // tiempo que tarda en girar
            float t = 0f;

            while (t < 1f)
            {
                t += Time.deltaTime / rotateTime;
                transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
                yield return null;
            }

            // Esperar X segundos antes de moverse al siguiente punto
            float waitTime = 10f; // tiempo que se queda quieto antes de ir al siguiente punto
            float timer = 0f;
            while (timer < waitTime)
            {
                timer += Time.deltaTime;

                // Si detecta al jugador mientras espera, interrumpe la espera
                if (chasingPlayer)
                    yield break;

                yield return null;
            }

            // Pasar al siguiente punto
            currentPointIndex = nextIndex;
        }
    }

    #endregion

    #region Vision

    private bool CanSeePlayer()
    {
        Vector3 origin = transform.position + Vector3.up * 1.6f;
        Vector3 dirToPlayer = player.position - origin;

        if (dirToPlayer.magnitude > viewDistance) return false;

        float angle = Vector3.Angle(transform.forward, dirToPlayer);
        if (angle > viewAngle * 0.5f) return false;

        if (Physics.Raycast(origin, dirToPlayer.normalized, out RaycastHit hit, viewDistance,
                obstacleMask | playerMask))
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

    #endregion

    #region Attack

    private IEnumerator AttackRoutine()
    {
        isAttacking = true;
        attackTimer = attackCooldown;

        agent.isStopped = true;
        animator.SetFloat("Speed", 0f);
        animator.SetTrigger("Attack");

        float attackDuration = 0.6f;
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

    #endregion
}