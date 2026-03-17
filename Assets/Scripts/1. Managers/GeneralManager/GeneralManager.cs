using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;
using TMPro;
using System;
using UnityEngine.UI;


public class GeneralManager : MonoBehaviour
{
    String actualScene;
    /*
    ----------------------------------------------------------------------------------------------------------------------------
    Player and Timer References
    ----------------------------------------------------------------------------------------------------------------------------
    */
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Rigidbody playerRb;
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
    [SerializeField] private GameObject decreaseScoreText;
    [SerializeField] private TextMeshProUGUI potionText;
    [SerializeField] private GameObject keyImage;
    [SerializeField] private GameObject deathPanel;
    [SerializeField] private TextMeshProUGUI textDeathPanel;
    public TextMeshProUGUI interactionText;
    /*
    ----------------------------------------------------------------------------------------------------------------------------
    * Score management variables
    ----------------------------------------------------------------------------------------------------------------------------
    */
    public int score = 5000;
    private float scoreIntervalTimer = 0f;
    private float scoreInterval = 10f;
    private int scorePenalty = 25;

    /*
    * Music and SFX Audio Sources
    */

    public AudioSource musicSource;
    public AudioSource sfxSource;

    public AudioClip decreaseScoreClip;
    /*
    * Health
    */
    public Image healthBar;
    [SerializeField] private GameObject LoginPanel;
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
        if (actualScene != "Tutorial")
        {
            LoginPanel.SetActive(true);
        }
    }

    void Update()
    {
        GetTime();
        if (LoginPanel.activeSelf && actualScene != "Tutorial")
        {
            playerMovement.BlockCamera();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

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
                EnableDecreaseText(scorePenalty);
                DisableDecreaseTextDelayed(2f);
            }
        }
        UpdateScore();
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
    void UpdateScore()
    {
        if (actualScene != "Tutorial")
        {
            if (score < 0) score = 0;
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

    public void SaveScore()
    {
        PlayerPrefs.SetInt("Score", score);
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
    · Music Volume Management
    ----------------------------------------------------------------------------------------------------------------------------
    */

    public void EnableDecreaseText(int amount)
    {
        if (actualScene != "Tutorial")
        {
            decreaseScoreText.SetActive(true);
            decreaseScoreText.GetComponent<TextMeshProUGUI>().text = "-" + amount.ToString();
            sfxSource.PlayOneShot(decreaseScoreClip);
        }
    }

    public void DisableDecreaseText()
    {
        decreaseScoreText.SetActive(false);
    }

    public void DisableDecreaseTextDelayed(float delay)
    {
        Invoke(nameof(DisableDecreaseText), delay);
    }

    public void ChangePotionText(string text)
    {
        potionText.text = text;
    }

    public void EnableKeyImage()
    {
        keyImage.SetActive(true);
    }

    public void EnableDeathPanel()
    {
        deathPanel.SetActive(true);
        playerMovement.BlockCamera();
        playerRb.constraints = RigidbodyConstraints.FreezeAll;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (actualScene != "Tutorial")
        {
            textDeathPanel.text = "Has muerto, ¡¡Dame 200 puntos y vuelve a empezar!!";
        }
        else
        {
            textDeathPanel.text = "¡Has muerto, reapareces al inicio del nivel!";
        }

    }

    public void DisableDeathPanel()
    {
        deathPanel.SetActive(false);
        playerMovement.UnblockCamera();
        playerRb.constraints = RigidbodyConstraints.FreezeRotation;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public float GetTime()
    {
        string timeStr = timerText.text.Trim(); // Quita espacios al inicio/final
        float totalSeconds = 0f;

        // Separar minutos y segundos
        string[] parts = timeStr.Split(':');

        if (parts.Length == 2)
        {
            // Parsear minutos
            if (!float.TryParse(parts[0], out float minutes))
            {
                Debug.LogWarning("No se pudo convertir los minutos: " + parts[0]);
                minutes = 0f;
            }

            // Parsear segundos (puede tener decimales)
            if (!float.TryParse(parts[1], out float seconds))
            {
                Debug.LogWarning("No se pudo convertir los segundos: " + parts[1]);
                seconds = 0f;
            }

            totalSeconds = minutes * 60f + seconds;
        }
        else
        {
            Debug.LogWarning("Formato de tiempo incorrecto: " + timeStr);
        }

        // Debug opcional
        //Debug.Log("Tiempo total en segundos: " + totalSeconds);

        return totalSeconds;
    }
}