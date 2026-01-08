using UnityEngine;

public class Lever : MonoBehaviour
{
    private Animator animator;
    private RaycastController raycast;
    private Outline outline;
    void Start()
    {
        animator = GetComponent<Animator>();
        outline = GetComponent<Outline>();
        raycast = FindObjectOfType<RaycastController>();
    }
    private void Update()
    {
        outline.OutlineColor = new Color(0, 0, 0, 0);
        OpenDoor();
    }
    public void OpenDoor()
    {
        if (raycast.GetHitObjectName() == "Lever")
        {
            outline.OutlineColor = Color.white;
            if (Input.GetMouseButton(0))
            {
                animator.SetBool("IsActive", true);
            }
        }
    }
}
