using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;
using TMPro;
using System;


public class GeneralManager : MonoBehaviour
{
    String actualScene;

    /*
    ----------------------------------------------------------------------------------------------------------------------------
    Player and Timer References
    ----------------------------------------------------------------------------------------------------------------------------
    */
    [SerializeField] private PlayerMovement playerMovement;
    private Timer timer;
    /*
    ----------------------------------------------------------------------------------------------------------------------------
    * Pause menu management variables
    ----------------------------------------------------------------------------------------------------------------------------
    */
    [SerializeField] private List<GameObject> panelsToDeactivate;
    [SerializeField] private GameObject pauseMenuPanel;
    private bool pauseMenuActive = false;
    /*
    ----------------------------------------------------------------------------------------------------------------------------
    * UI Elements
    ----------------------------------------------------------------------------------------------------------------------------
    */
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI interactionText;
    /*
    ----------------------------------------------------------------------------------------------------------------------------
    * Score management variables
    ----------------------------------------------------------------------------------------------------------------------------
    */
    private int score = 1000;
    private float scoreIntervalTimer = 0f;
    private float scoreInterval = 60f;
    private int scorePenalty = 100;

    /*
    * Music and SFX Audio Mixers
    */

    /*
    ----------------------------------------------------------------------------------------------------------------------------
    · Void Awake() and Update() Methods
    ----------------------------------------------------------------------------------------------------------------------------
    */
    void Awake()
    {
        timer = new Timer();
        timer.StartTimer();
        actualScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
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

        if (!pauseMenuActive)
        {
            scoreIntervalTimer += Time.deltaTime;

            if (scoreIntervalTimer >= scoreInterval)
            {
                DecreaseScore(scorePenalty);
                scoreIntervalTimer = 0f;
            }
        }
        Score();
    }

    /*
    ----------------------------------------------------------------------------------------------------------------------------
    · Pause Menu Management
    ----------------------------------------------------------------------------------------------------------------------------
    */
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
    /*
    ----------------------------------------------------------------------------------------------------------------------------
    · Score Management
    ----------------------------------------------------------------------------------------------------------------------------
    */
    void Score()
    {
        if (actualScene != "Tutorial")
        {
            if (score < 0) score = 0;
            PlayerPrefs.SetInt("Score", score);
            scoreText.text = "Score: " + score.ToString();
        }
        else
        {
            scoreText.text = "Score: ∞";
        }

    }

    public void DecreaseScore(int amount)
    {
        score -= amount;
        if (score < 0) score = 0;
    }

    public void IncreaseScore(int amount)
    {
        score += amount;
    }
    /*
    ----------------------------------------------------------------------------------------------------------------------------
    · Interaction Text Management
    ----------------------------------------------------------------------------------------------------------------------------
    */
    public void SetInteractionText(string text)
    {
        interactionText.text = text;
    }

    public void ClearInteractionText()
    {
        interactionText.text = "";
    }

    /*
    ----------------------------------------------------------------------------------------------------------------------------
    · On Application Quit
    ----------------------------------------------------------------------------------------------------------------------------
    */
    void OnApplicationQuit()
    {
        timer.SaveTime();
    }

    /*
    ----------------------------------------------------------------------------------------------------------------------------
    · Music Volume Management
    ----------------------------------------------------------------------------------------------------------------------------
    */
}