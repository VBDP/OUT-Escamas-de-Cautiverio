using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class GeneralManager : MonoBehaviour
{   
    [SerializeField] private List<GameObject> panelsToDeactivate;
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private PlayerMovement playerMovement;
    private bool pauseMenuActive = false;
    private Timer timer;
    [SerializeField] private TextMeshProUGUI timerText;

    void Awake()
    {
    timer = new Timer();
    timer.StartTimer();
    }
    void Update()
    { 
         timer.Tick(Time.deltaTime);
         timerText.text = timer.GetFormattedTime();


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

        score();
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

    void score()
    {
        int score = 1000;
        float timePlayed = timer.TimePlayed;
        score -= (int)timePlayed;
        if (score < 0) score = 0;
        PlayerPrefs.SetInt("Score", score); 
        Debug.Log("Score: " + score);
    }
}
