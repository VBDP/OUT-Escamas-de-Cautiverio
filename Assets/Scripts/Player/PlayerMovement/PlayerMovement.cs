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
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float maxVerticalAngle = 60f;

    private Rigidbody rb;
    private CapsuleCollider capsule;
    private float verticalRotation = 0f;
    private bool cameraUnlocked = true;
    private bool isGrounded = true;

    [HideInInspector] public bool disableCursorLockOnStart = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();

        rb.freezeRotation = true;

        if (!playerCamera && transform.childCount > 0)
            playerCamera = transform.GetChild(0);

        // Cargar sensibilidad desde PlayerPrefs (el valor guardado es de 0 a 1)
        float savedSens = PlayerPrefs.GetFloat("MouseSensitivity", 0.4f);
        mouseSensitivity = savedSens * 5f; // Sincronizado con OptionsManager
    }

    private void Start()
    {
        // Solo bloqueamos el cursor si el sistema lo permite al empezar
        // (El GeneralManager puede deshabilitar esto si el login está activo)
        if (!disableCursorLockOnStart)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
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

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -maxVerticalAngle, maxVerticalAngle);

        if (playerCamera != null)
            playerCamera.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);
    }

    public void SetMouseSensitivity(float value)
    {
        mouseSensitivity = value;
    }

    public void BlockCamera()
    {
        cameraUnlocked = false;
        Debug.Log("<color=red>Camera BLOCKED</color> by call from: " + new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().Name);
    }

    public void UnblockCamera()
    {
        cameraUnlocked = true;
        Debug.Log("<color=green>Camera UNBLOCKED</color> by call from: " + new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().Name);
    }
    #endregion
}