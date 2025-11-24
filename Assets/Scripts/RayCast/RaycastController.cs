using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RaycastController : MonoBehaviour
{
    public bool hittingDoor = false;
    public Inventory inventory;
    public Outline outline;
    public Outline outlineDoubleDoor;
    public Outline NPCOutline;
    public TextMeshProUGUI interactionText;
    public Image inventory1Image;

    
    public void Update()
    {
        interactionText.text = "";
        bool keyFirstDoor = inventory.returnKeyFirstDoor(this);
                    RaycastHit hit;
        outline.OutlineColor = new Color(0f, 0f, 0f, 0f); //No se ve el outline en la puerta
        outlineDoubleDoor.OutlineColor = new Color(1f, 1f, 1f, 0f);
        NPCOutline.OutlineColor = new Color(0f,0f,0f,0f);
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

            if(hit.collider.tag == "Collectable")
            {
                interactionText.text = "Click to grab";
                if (Input.GetMouseButton(0))
                {
                    inventory.GetKeyFirstDoor();
                    Destroy(hit.collider.gameObject);
                    inventory1Image.enabled = true;
                    
                }
            }
            
            if (hit.collider.tag == "Trigger")
            {
                interactionText.text = "Click to interact";
                outlineDoubleDoor.OutlineColor = new Color(1f, 1f, 1f, 1f);
                if (Input.GetMouseButtonDown(0))
                {
                    if(hit.collider.name == "Lever"){
                        Animator animator = hit.collider.GetComponent<Animator>();
                        animator.SetBool("IsActive", true);
                                            }

                }
            }

            if(hit.collider.tag == "NPC" && Input.GetMouseButton(0))
            {
                interactionText.text = "Assassin: �Puedes ver la palanca al lado de la puerta doble de madera?, Tira de ella y hayar�s lo que buscas";
                
            }
            else if (hit.collider.tag == "NPC" && !Input.GetMouseButton(0)){
                interactionText.text = "Click to Talk";
                NPCOutline.OutlineColor = new Color(1f, 0.84f, 0f, 1f);
                
            }



        }
        
    }
}

