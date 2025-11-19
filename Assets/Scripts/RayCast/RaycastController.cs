using TMPro;
using UnityEngine;

public class RaycastController : MonoBehaviour
{
    public bool hittingDoor = false;
    public Inventory inventory;
    public Outline outline;
    public Outline outlineKey;
    public Outline LeverOutline;
    public TextMeshProUGUI interactionText;
    public void Update()
    {
        bool keyFirstDoor = inventory.returnKeyFirstDoor(this);
        interactionText.text = "";
            RaycastHit hit;
        outline.OutlineColor = new Color(0f, 0f, 0f, 0f); //No se ve el outline en la puerta
        outlineKey.OutlineColor = new Color(1f, 1f, 1f, 0f);
        LeverOutline.OutlineColor = new Color(1f, 1f, 1f, 0f);
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 5))
        {
            
            if (hit.collider.gameObject.tag == "Door") {

                if (keyFirstDoor)
                {
                    outline.OutlineColor = new Color(0f, 1f, 0f); //La puerta se vuelve verde cuando tienes la llave
                }

                if (Input.GetMouseButton(0) && keyFirstDoor)
                {
                        Debug.Log(keyFirstDoor);
                        hit.transform.Rotate(Vector3.down);
                    interactionText.text = "";
                }
                else if (Input.GetMouseButton(0)) 
                {
                    Debug.Log(keyFirstDoor);
                                    }
                else if (!keyFirstDoor)
                {
                    outline.OutlineColor = new Color(1f, 0f, 0f); //La puerta se vuelve roja cuando no tienes la llave
                    interactionText.text = "Necesitas una llave para abrir esa puerta";
                }
            }

            if(hit.collider.name == "Key01")
            {
                interactionText.text = "Click to grab";
                outlineKey.OutlineColor = new Color(1f, 1f, 1f,1f); //La llave se vuelve blanca al hacer hover
                if (Input.GetMouseButton(0))
                {
                    inventory.GetKeyFirstDoor();
                    Destroy(hit.collider.gameObject);
                    
                }
            }

            if (hit.collider.gameObject.tag == "Lever")
            {
                LeverOutline.OutlineColor = new Color(1f, 1f, 1f,1f); //El palanca se vuelve blanca al hacer hover
                interactionText.text = "Click to pull the lever";
                if (Input.GetMouseButton(0))
                {

                    
                }
                

        }
        
    }
}
}

