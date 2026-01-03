using UnityEngine;

public class ClickRotation : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;
    [SerializeField] private float rotationSpeed = 100f;
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            float mouseX = Input.GetAxis("Mouse X");
            targetObject.transform.Rotate(Vector3.up, -mouseX * rotationSpeed * Time.deltaTime);
        }
    }
}
