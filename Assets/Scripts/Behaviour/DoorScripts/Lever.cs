using UnityEngine;

public class Lever : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private RaycastController Raycast;
    private Outline outline;
    void Start() { animator = GetComponent<Animator>(); outline = GetComponent<Outline>(); }
    private void Update()
    {
        outline.OutlineColor = new Color(0,0,0,0);
        openDoor();
    }
    public void openDoor() { if (Raycast.GetHitObjectName() == "Lever") { outline.OutlineColor = Color.white; if (Input.GetMouseButton(0)) { animator.SetBool("IsActive", true); } } }    
}
