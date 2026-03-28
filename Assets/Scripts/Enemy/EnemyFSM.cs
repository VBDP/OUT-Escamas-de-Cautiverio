using UnityEngine;
using UnityEngine.AI;

public class EnemyFSM : MonoBehaviour
{
    public enum State
    {
        Patrol,
        Chase,
        Attack,
        Return
    }

    public State currentState;

    [Header("References")]
    public Transform player;
    public Transform[] waypoints;
    private NavMeshAgent agent;
    public Animator animator;

    [Header("Parámetros del Animator (Bools)")]
    public string boolWalk = "isWalking";
    public string boolRun = "isRunning";
    public string boolAttack = "isAttacking";

    [Header("Settings")]
    public float detectionRange = 10f;
    [Tooltip("Ángulo de visión frontal (cono)")]
    public float visionAngle = 90f;
    [Tooltip("Capas que bloquean la visión del enemigo (ej. Paredes, Obstáculos)")]
    public LayerMask obstacleMask;
    [Tooltip("Desplazamiento en Y para lanzar el rayo desde los 'ojos'")]
    public float eyeHeight = 1.5f;
    public float attackRange = 2f;
    public float loseRange = 15f;

    [Header("Patrol Settings")]
    public float waitMin = 5f;
    public float waitMax = 15f;
    public float rotationSpeed = 5f;

    private int currentWaypoint = 0;
    private float waitTimer = 0f;
    private float currentWaitTime = 0f;
    private bool isWaiting = false;

    private Vector3 startPosition;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (animator == null) animator = GetComponentInChildren<Animator>();
        
        startPosition = transform.position;
        
        SetNextWaitTime();
        
        // Iniciamos en estado Patrol llamando al método
        ChangeState(State.Patrol);
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        switch (currentState)
        {
            case State.Patrol:
                Patrol();
                if (CanSeePlayer(distance))
                    ChangeState(State.Chase);
                break;

            case State.Chase:
                agent.SetDestination(player.position);
                // Si está en rango de ataque, ataca
                if (distance <= attackRange)
                    ChangeState(State.Attack);
                // Si se aleja mucho, vuelve al inicio
                else if (distance > loseRange)
                    ChangeState(State.Return);
                break;

            case State.Attack:
                agent.SetDestination(transform.position); // Quedarse quieto para atacar
                // Rotar hacia el jugador suavemente mientras ataca
                Vector3 dir = (player.position - transform.position).normalized;
                if (dir != Vector3.zero)
                {
                    Quaternion lookRotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
                }
                
                // Si el jugador se aleja, volver a perseguir
                if (distance > attackRange)
                    ChangeState(State.Chase);
                break;

            case State.Return:
                agent.SetDestination(startPosition);
                // Si llegó a su posición inicial, volver a patrullar
                if (Vector3.Distance(transform.position, startPosition) < 1.5f)
                    ChangeState(State.Patrol);
                break;
        }
    }

    /// <summary>
    /// Cambia el estado actual y ajusta los parámetros del Animator.
    /// </summary>
    public void ChangeState(State newState)
    {
        currentState = newState;

        // Primero apagamos todos los parámetros por seguridad
        animator.SetBool(boolWalk, false);
        animator.SetBool(boolRun, false);
        animator.SetBool(boolAttack, false);

        switch (currentState)
        {
            case State.Patrol:
                if (!isWaiting) 
                    animator.SetBool(boolWalk, true);
                break;
                
            case State.Chase:
                animator.SetBool(boolRun, true);
                break;
                
            case State.Attack:
                animator.SetBool(boolAttack, true);
                break;
                
            case State.Return:
                animator.SetBool(boolWalk, true);
                break;
        }
    }

    void Patrol()
    {
        if (waypoints.Length == 0) return;

        Transform targetPoint = waypoints[currentWaypoint];

        // Si está esperando en un waypoint
        if (isWaiting)
        {
            agent.SetDestination(transform.position);

            // Rotar hacia el siguiente punto
            Vector3 dir = (targetPoint.position - transform.position).normalized;
            if (dir != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
            }

            waitTimer += Time.deltaTime;

            if (waitTimer >= currentWaitTime)
            {
                isWaiting = false;
                waitTimer = 0f;
                currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
                
                // Se acabó la espera, empezamos a caminar
                animator.SetBool(boolWalk, true);
            }

            return;
        }

        // Moverse al waypoint
        agent.SetDestination(targetPoint.position);

        // Si llega al punto (ajustado a 1.5f para que la detección sea más precisa)
        if (Vector3.Distance(transform.position, targetPoint.position) < 1.5f)
        {
            isWaiting = true;
            SetNextWaitTime();
            
            // Hemos llegado, así que dejamos de caminar (pasamos a Idle por defecto)
            animator.SetBool(boolWalk, false);
        }
    }

    void SetNextWaitTime()
    {
        currentWaitTime = Random.Range(waitMin, waitMax);
    }

    bool CanSeePlayer(float distance)
    {
        if (distance <= detectionRange)
        {
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            // Ignoramos la diferencia de altura para el cálculo horizontal del ángulo (opcional, pero más estable)
            directionToPlayer.y = 0; 
            Vector3 forward = transform.forward;
            forward.y = 0;
            
            float angle = Vector3.Angle(forward, directionToPlayer.normalized);
            
            // Si el jugador está dentro de la mitad del ángulo total de visión hacia la izquierda o derecha
            if (angle <= visionAngle / 2f)
            {
                // Raycast para verificar línea de visión y obstáculos
                Vector3 origin = transform.position + Vector3.up * eyeHeight;
                Vector3 target = player.position + Vector3.up * eyeHeight;
                Vector3 dirToTarget = target - origin;
                
                // Si el rayo choca con algo en la capa de obstáculos antes de llegar al jugador, no lo ve
                if (!Physics.Raycast(origin, dirToTarget.normalized, dirToTarget.magnitude, obstacleMask))
                {
                    return true;
                }
            }
        }
        return false;
    }

    void OnDrawGizmosSelected()
    {
        Vector3 eyePosition = transform.position + Vector3.up * eyeHeight;

        // Dibujamos el rango general de detección en amarillo
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(eyePosition, detectionRange);

        // Dibujamos las líneas del cono de visión en rojo
        Gizmos.color = Color.red;
        Vector3 rightDir = Quaternion.Euler(0, visionAngle / 2f, 0) * transform.forward;
        Vector3 leftDir = Quaternion.Euler(0, -visionAngle / 2f, 0) * transform.forward;
        
        Gizmos.DrawRay(eyePosition, rightDir * detectionRange);
        Gizmos.DrawRay(eyePosition, leftDir * detectionRange);
    }
}