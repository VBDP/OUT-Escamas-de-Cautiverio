using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float verticalRotation = 0f;
    [SerializeField] private float jumpForce = 5f;
    private bool isGrounded;
    private bool cameraUnlocked = true;
    Transform playerCamera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        isGrounded = true;
        playerCamera = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        CameraRotation();
    }

    void FixedUpdate()
    {
        float inputH = Input.GetAxis("Horizontal");
        float inputV = Input.GetAxis("Vertical");

        Vector3 moveDirection = (transform.forward * inputV + transform.right * inputH).normalized
         * moveSpeed * Time.deltaTime;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.MovePosition(rb.position + moveDirection);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            moveSpeed = 10f;
        }
        else
        {
            moveSpeed = 5f;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") && !isGrounded)
        {
            if (collision.transform.position.y - transform.position.y < 1f)
            {
                isGrounded = true;
            }
        }
    }

    public void CameraRotation()
    {
        if (cameraUnlocked)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            verticalRotation -= mouseY;
            verticalRotation = Mathf.Clamp(verticalRotation, -60f, 60f);

            playerCamera.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
        }
    }

    public void BlockCamera()
    {
        cameraUnlocked = false;
    }

    public void UnblockCamera()
    {
        cameraUnlocked = true;
    }
}
