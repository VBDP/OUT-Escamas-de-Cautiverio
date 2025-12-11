using TMPro;
using UnityEngine;

public class TutorialChest : MonoBehaviour
{
    private Outline outline;
    private Animator animator;
    [SerializeField] private RaycastController raycast;
    [SerializeField] private TextMeshProUGUI interactionText;
    [SerializeField] private GameObject WinPanel;
    [SerializeField] private GameObject HUDPanel;

    [SerializeField] private GameObject particles;

    [SerializeField] private Transform player;
    [SerializeField] private Transform teleportPoint;
    
    [SerializeField] private Rigidbody rb;

    [SerializeField] private bool isOpen = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        outline = GetComponent<Outline>();
       
    }
    private void Update()
    {
        outline.OutlineColor = new Color(0,0,0,0);
        OpenChest();

        if (isOpen) { particles.SetActive(true); player.position = teleportPoint.position; player.rotation = teleportPoint.rotation;
            rb.constraints = RigidbodyConstraints.FreezeAll;WinPanel.SetActive(true); HUDPanel.SetActive(false); }
    }
    void OpenChest() { 
        if (raycast.GetHitObjectName() == "HellChest") { 
            outline.OutlineColor = Color.white; interactionText.text = "Abre el cofre para finalizar el tutorial"; 
            if(Input.GetMouseButton(0))
            { 
                animator.SetBool("IsOpen",true); isOpen = true;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            } 
         
        }
    } 
}
