using UnityEngine;

public class RaycastController : MonoBehaviour
{
    public bool hittingDoor = false;
    public void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 5))
        {
            if (hit.collider.gameObject.tag == "Door")
            {
                if (Input.GetMouseButton(0))
                {                
                        hit.transform.Rotate(Vector3.down);                   
                }
            }
        }
    }
}

