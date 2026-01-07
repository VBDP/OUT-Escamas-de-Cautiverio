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
    private int score = 1000;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI interactionText;


    private float scoreIntervalTimer = 0f;
    private float scoreInterval = 60f;
    private int scorePenalty = 100;
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

        Score();

        if (!pauseMenuActive)
        {
            scoreIntervalTimer += Time.deltaTime;

            if (scoreIntervalTimer >= scoreInterval)
            {
                decreaseScore(scorePenalty);
                scoreIntervalTimer = 0f;
            }
        }
    }

    //Pause Menu Management
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

    //Score Management
    void Score()
    {
        if (score < 0) score = 0;
        PlayerPrefs.SetInt("Score", score);
        scoreText.text = "Score: " + score.ToString();
    }

    public void decreaseScore(int amount)
    {
        score -= amount;
        if (score < 0) score = 0;
    }

    public void increaseScore(int amount)
    {
        score += amount;
    }

    //Timer Save on Quit
    void OnApplicationQuit()
    {
        timer.SaveTime();
    }
}
