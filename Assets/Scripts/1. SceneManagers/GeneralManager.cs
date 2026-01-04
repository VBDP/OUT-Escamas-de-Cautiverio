using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class GeneralManager : MonoBehaviour
{   
    [SerializeField] private List<GameObject> panelsToDeactivate;
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private TextMeshProUGUI timerText;
    private float timer = 0f;
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

        UpdateTimerDisplay(Time.timeSinceLevelLoad);
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

    void UpdateTimerDisplay(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time - minutes * 60);
        timerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
    }
}
