using UnityEngine;

public class ColissionDetection : MonoBehaviour
{
    public float pushBackForce = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            Vector3 pushDir = (transform.position - other.ClosestPoint(transform.position)).normalized;
            transform.position += pushDir * pushBackForce * Time.deltaTime;
        }
    }

   private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            Vector3 pushDir = (transform.position - other.ClosestPoint(transform.position)).normalized;
            transform.position += pushDir * 0f * Time.deltaTime;
        }
    }
}
