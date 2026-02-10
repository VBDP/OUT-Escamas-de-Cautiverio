using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintSpeed = 10f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float groundCheckDistance = 0.1f;

    [Header("Camera Settings")]
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float maxVerticalAngle = 60f;

    private Rigidbody rb;
    private float verticalRotation = 0f;
    private bool cameraUnlocked = true;
    private bool isGrounded = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Evita rotación física
        if (!playerCamera && transform.childCount > 0)
            playerCamera = transform.GetChild(0);
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleCameraRotation();
        CheckGrounded();
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

        // Calculamos velocidad horizontal
        Vector3 targetVelocity = moveDir * currentSpeed;
        // Mantener velocidad vertical para saltos/gravedad
        targetVelocity.y = rb.linearVelocity.y;

        rb.linearVelocity = targetVelocity;

        // Saltar
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z); // reset vertical
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void CheckGrounded()
    {
        // Raycast hacia abajo para detectar si está en el suelo
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance + 0.1f);
    }
    #endregion

    #region Camera
    private void HandleCameraRotation()
    {
        if (!cameraUnlocked) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -maxVerticalAngle, maxVerticalAngle);

        if (playerCamera != null)
            playerCamera.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);
    }

    public void BlockCamera() => cameraUnlocked = false;
    public void UnblockCamera() => cameraUnlocked = true;
    #endregion
}
