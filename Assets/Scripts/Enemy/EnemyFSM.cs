using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

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

    [Header("Alert Settings")]
    [Range(0, 100)] public float currentAlert = 0f;
    public float maxAlert = 100f;
    [Tooltip("Sprite (Image UI) en el HUD que se llenará con el nivel de alerta")]
    public Image alertUIFill;
    [Tooltip("AudioSource para el sonido de tensión. ¡Recuerda marcar 'Loop' en Unity!")]
    public AudioSource alertAudioSource;
    [Tooltip("Tono (Pitch) más grave/lento cuando te empieza a ver")]
    public float minPitch = 1.0f;
    [Tooltip("Tono (Pitch) más agudo/rápido justo antes de atacarte")]
    public float maxPitch = 2.0f;
    [Tooltip("Aumento por segundo si estás en su rango pero no frente a él")]
    public float alertIncreaseSlow = 15f;
    [Tooltip("Aumento por segundo si te ve directamente en el cono visual")]
    public float alertIncreaseFast = 50f;
    [Tooltip("Disminución por segundo si deja de verte / te escondes")]
    public float alertDecrease = 25f;

    [Header("Combat Settings")]
    public float attackCooldown = 2.5f;
    [Tooltip("Tiempo ínfimo (ej: 0.1) que el Boolean de ataque está encendido. Funciona como un Trigger para que el zarpazo no se repita 3 veces seguidas.")]
    public float attackAnimationDuration = 0.1f;
    private float attackTimer = 0f;

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

        // Nivel de alerta sube o baja constantemente basado en posición del jugador
        UpdateAlertLevel(distance);

        switch (currentState)
        {
            case State.Patrol:
                Patrol();
                if (currentAlert >= maxAlert)
                    ChangeState(State.Chase);
                break;

            case State.Chase:
                agent.SetDestination(player.position);
                // Si está en rango de ataque, ataca
                if (distance <= attackRange)
                    ChangeState(State.Attack);
                // Si la alerta llegó a 0 (estuvo escondido un rato) o se alejó mucho 
                else if (currentAlert <= 0 || distance > loseRange)
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

                // --------- LÓGICA DE COOLDOWN ---------
                attackTimer += Time.deltaTime;
                
                if (attackTimer >= attackAnimationDuration)
                {
                    // Apagamos la orden de ataque para que el Animator regrese a IDLE
                    animator.SetBool(boolAttack, false);
                }
                
                if (attackTimer >= attackCooldown)
                {
                    // Ha pasado el tiempo de recarga, damos el siguiente golpe
                    animator.SetBool(boolAttack, true);
                    attackTimer = 0f; 
                }
                // --------------------------------------
                
                // Si el jugador se aleja, volver a perseguir
                if (distance > attackRange)
                {
                    if (currentAlert > 0)
                        ChangeState(State.Chase);
                    else
                        ChangeState(State.Return);
                }
                break;

            case State.Return:
                agent.SetDestination(startPosition);
                // Si mientras vuelve a su puesto la alerta se llena, persigue de nuevo
                if (currentAlert >= maxAlert)
                    ChangeState(State.Chase);
                // Si llegó a su posición inicial, volver a patrullar
                else if (Vector3.Distance(transform.position, startPosition) < 1.5f)
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
                attackTimer = 0f; // Empezar el ataque inmediatamente
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

    void UpdateAlertLevel(float distance)
    {
        bool inRange = distance <= detectionRange;
        bool hasLineOfSight = false;
        bool inVisionCone = false;

        if (inRange)
        {
            Vector3 origin = transform.position + Vector3.up * eyeHeight;
            Vector3 target = player.position + Vector3.up * eyeHeight;
            Vector3 dirToTarget = target - origin;

            // Revisamos si no hay paredes (aplica tanto para visión frontal como periférica/cercana)
            if (!Physics.Raycast(origin, dirToTarget.normalized, dirToTarget.magnitude, obstacleMask))
            {
                hasLineOfSight = true;

                Vector3 directionToPlayer = (player.position - transform.position).normalized;
                directionToPlayer.y = 0; 
                Vector3 forward = transform.forward;
                forward.y = 0;
                
                float angle = Vector3.Angle(forward, directionToPlayer.normalized);
                
                if (angle <= visionAngle / 2f)
                {
                    inVisionCone = true;
                }
            }
        }

        // Lógica de llenado/vaciado de alerta
        if (inVisionCone)
        {
            // Te está viendo directamente
            currentAlert += alertIncreaseFast * Time.deltaTime;
        }
        else if (inRange && hasLineOfSight)
        {
            // Estás cerca de él y no hay paredes, pero a su espalda o lados
            currentAlert += alertIncreaseSlow * Time.deltaTime;
        }
        else
        {
            // O estás muy lejos o hay un muro de por medio
            currentAlert -= alertDecrease * Time.deltaTime;
        }

        // Limitamos la alerta para que no pase de 100 ni baje de 0
        currentAlert = Mathf.Clamp(currentAlert, 0f, maxAlert);

        // HUD: Actualizar el relleno de la imagen si se le ha asignado una
        if (alertUIFill != null)
        {
            alertUIFill.fillAmount = currentAlert / maxAlert;
            
            // Opcional: Esto hace que el Sprite entero desaparezca de la pantalla si no te ha detectado nada (0 alerta).
            alertUIFill.gameObject.SetActive(currentAlert > 0);
        }

        // Sonido de Detección Dinámico
        if (alertAudioSource != null)
        {
            // Solo suena si la alerta es mayor a 0 y menor al máximo (cuando te persigue, el efecto de tensión cesa)
            if (currentAlert > 0 && currentAlert < maxAlert)
            {
                // Solo le damos a Play si no estaba sonando ya
                if (!alertAudioSource.isPlaying)
                    alertAudioSource.Play();
                
                // Calcular qué tan alertado está (0 a 1)
                float alertPercentage = currentAlert / maxAlert;
                
                // Lerp entre minPitch y maxPitch. De este modo, por muy alto que suba, nunca pasará del valor seguro que asgines a maxPitch
                alertAudioSource.pitch = Mathf.Lerp(minPitch, maxPitch, alertPercentage);
            }
            else
            {
                // Si la alerta bajó por completo o llegó al 100%, apagar el sonido de tensión
                if (alertAudioSource.isPlaying)
                    alertAudioSource.Stop();
            }
        }
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