using TMPro;
using UnityEngine;

public class TutorialChest : MonoBehaviour
{
    private Outline outline;
    private Animator animator;
    [SerializeField] private RaycastController raycast;
    [SerializeField] private TextMeshProUGUI interactionText;
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
    }
    void OpenChest() { if (raycast.GetHitObjectName() == "HellChest") { outline.OutlineColor = Color.white; interactionText.text = "Abre el cofre para finalizar el tutorial"; if(Input.GetMouseButton(0)){ animator.SetBool("IsOpen",true);  } } }
}
