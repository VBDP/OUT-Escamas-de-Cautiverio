using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ClickRotationPhysics : MonoBehaviour
{
    [SerializeField] private float torqueStrength = 15f;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        
        // Congelar posición para que no se mueva, y rotación en X y Z para que solo gire sobre Y
        rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            float mouseX = Input.GetAxis("Mouse X");

            // Aplicamos torque mientras arrastra
            rb.AddTorque(Vector3.up * -mouseX * torqueStrength, ForceMode.Acceleration);
        }
    }
}
