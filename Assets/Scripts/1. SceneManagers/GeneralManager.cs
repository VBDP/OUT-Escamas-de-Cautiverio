using UnityEngine;
using System.Collections.Generic;

public class GeneralManager : MonoBehaviour
{   
    [SerializeField] private List<GameObject> panelsToDeactivate;
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private PlayerMovement playerMovement;
    private bool pauseMenuActive = false;

    void Update()
    { 
        if (Input.GetKeyDown(KeyCode.Escape) && pauseMenuActive == false)
        {
            playerMovement.BlockCamera();
            OpenPauseMenu();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && pauseMenuActive == true)
        {
            playerMovement.UnblockCamera();
            ClosePauseMenu();
        }
    }

    void OpenPauseMenu()
    {
        foreach (GameObject panel in panelsToDeactivate)
        {
            panel.SetActive(false);
        }
        pauseMenuPanel.SetActive(true);
        pauseMenuActive = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
    }

    void ClosePauseMenu()
    {
        pauseMenuPanel.SetActive(false);
        panelsToDeactivate[0].gameObject.SetActive(true);
        pauseMenuActive = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
    }
}
