using UnityEngine;

public class AntiSaltos : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            rb.constraints = RigidbodyConstraints.FreezePositionY;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            rb.constraints = RigidbodyConstraints.None;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }
}
