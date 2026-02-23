using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float sprintSpeed = 5f;
    [SerializeField] private float jumpForce = 3f;

    [Header("Camera Settings")]
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float mouseSensitivityX = 2f;
    [SerializeField] private float mouseSensitivityY = 2f;
    [SerializeField] private float maxVerticalAngle = 60f;

    [Header("Camera Smooth Settings")]
    [SerializeField] private float smoothTime = 0.05f;

    [Header("Camera Collision Settings")]
    [SerializeField] private float cameraDistance = 0.5f; // distancia deseada de la cámara desde el player
    [SerializeField] private float cameraCollisionRadius = 0.2f; // radio del “capsule” de la cámara
    [SerializeField] private LayerMask collisionMask; // capas con las que la cámara colisiona

    private Rigidbody rb;
    private CapsuleCollider capsule;
    private float verticalRotation = 0f;
    private bool cameraUnlocked = true;
    private bool isGrounded = true;

    private float currentMouseX;
    private float currentMouseY;
    private float mouseXVelocity;
    private float mouseYVelocity;

    private Vector3 cameraOriginalLocalPos;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();

        rb.freezeRotation = true; // Evita rotación física
        rb.interpolation = RigidbodyInterpolation.Interpolate; // Suaviza movimiento físico

        if (!playerCamera && transform.childCount > 0)
            playerCamera = transform.GetChild(0);

        // Guardamos posición local inicial de la cámara
        if (playerCamera)
            cameraOriginalLocalPos = playerCamera.localPosition;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleCameraRotation();
        HandleJump();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    #region Movement
    private void HandleMovement()
    {
        float inputH = Input.GetAxisRaw("Horizontal");
        float inputV = Input.GetAxisRaw("Vertical");

        Vector3 moveDir = (transform.forward * inputV + transform.right * inputH).normalized;

        float currentSpeed = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            ? sprintSpeed
            : walkSpeed;

        Vector3 targetVelocity = moveDir * currentSpeed;
        targetVelocity.y = rb.linearVelocity.y; // mantener velocidad vertical
        rb.linearVelocity = targetVelocity;
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z); // reset vertical
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            if (Vector3.Dot(contact.normal, Vector3.up) > 0.5f)
            {
                isGrounded = true;
                break;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }
    #endregion

    #region Camera
    private void HandleCameraRotation()
    {
        if (!cameraUnlocked) return;

        float targetMouseX = Input.GetAxis("Mouse X") * mouseSensitivityX;
        float targetMouseY = Input.GetAxis("Mouse Y") * mouseSensitivityY;

        // Suavizado tipo SmoothDamp
        currentMouseX = Mathf.SmoothDamp(currentMouseX, targetMouseX, ref mouseXVelocity, smoothTime);
        currentMouseY = Mathf.SmoothDamp(currentMouseY, targetMouseY, ref mouseYVelocity, smoothTime);

        verticalRotation -= currentMouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -maxVerticalAngle, maxVerticalAngle);

        if (playerCamera != null)
            playerCamera.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);

        transform.Rotate(Vector3.up * currentMouseX);

        // Maneja colisión de cámara
        HandleCameraCollision();
    }

    private void HandleCameraCollision()
    {
        if (!playerCamera) return;

        Vector3 origin = transform.position + Vector3.up * (capsule.height * 0.5f); // centro del jugador
        Vector3 desiredCameraDir = playerCamera.forward;
        float maxDistance = cameraDistance;

        RaycastHit hit;
        if (Physics.SphereCast(origin, cameraCollisionRadius, -desiredCameraDir, out hit, maxDistance, collisionMask))
        {
            float distance = Mathf.Max(hit.distance - 0.05f, 0.1f); // evitar tocar el objeto
            Vector3 targetLocalPos = cameraOriginalLocalPos - desiredCameraDir * (cameraDistance - distance);
            playerCamera.localPosition = Vector3.Lerp(playerCamera.localPosition, targetLocalPos, 0.2f);
        }
        else
        {
            playerCamera.localPosition = Vector3.Lerp(playerCamera.localPosition, cameraOriginalLocalPos, 0.2f);
        }
    }

    public void BlockCamera() => cameraUnlocked = false;
    public void UnblockCamera() => cameraUnlocked = true;
    #endregion
}