using TMPro;
using UnityEngine;

public class RaycastController : MonoBehaviour
{
    public bool hittingDoor = false;
    public Inventory inventory;
    public Outline outline;
    public TextMeshProUGUI interactionText;
    public void Update()
    {
        bool keyFirstDoor = inventory.returnKeyFirstDoor(this);
        interactionText.text = "";
        if (keyFirstDoor)
        {
            outline.OutlineColor = new Color(0f, 1f, 0f);
        }
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 5))
        {
            if (hit.collider.gameObject.tag == "Door") {
           
                if (Input.GetMouseButton(0) && keyFirstDoor)
                {
                        Debug.Log(keyFirstDoor);
                        hit.transform.Rotate(Vector3.down);
                    interactionText.text = "";
                }
                else if (Input.GetMouseButton(0)) 
                {
                    Debug.Log(keyFirstDoor);
                    interactionText.text = "Necesitas una llave para abrir esa puerta";
                }
            }

            if(hit.collider.name == "Key01")
            {
                interactionText.text = "Click to grab";
                if (Input.GetMouseButton(0))
                {
                    inventory.GetKeyFirstDoor();
                    Destroy(hit.collider.gameObject);
                    
                }
            }

        }
    }
}

