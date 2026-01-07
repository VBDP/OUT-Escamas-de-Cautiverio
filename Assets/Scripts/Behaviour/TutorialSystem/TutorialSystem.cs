using UnityEngine;

public class TutorialSystem : MonoBehaviour
{
    public GameObject tutorialPanel;
    public Rigidbody rb;
    [SerializeField] private PlayerMovement playerMovement;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            rb.constraints = RigidbodyConstraints.None;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            PanelDeactivate();
        }
    }

    public void PanelActivate()
    {
        tutorialPanel.SetActive(true);
        rb.constraints = RigidbodyConstraints.FreezeAll;
        playerMovement.BlockCamera();
    }

    public void PanelDeactivate()
    {
        tutorialPanel.SetActive(false);
        playerMovement.UnblockCamera();
    }
}
