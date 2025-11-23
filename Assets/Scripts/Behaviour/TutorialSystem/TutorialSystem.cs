using UnityEngine;

public class TutorialSystem : MonoBehaviour
{
    public GameObject tutorialPanel;
    public Rigidbody rb;
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
        rb.constraints = RigidbodyConstraints.FreezePosition;
    }

    public void PanelDeactivate()
    {
        tutorialPanel.SetActive(false);
               
    }
}
