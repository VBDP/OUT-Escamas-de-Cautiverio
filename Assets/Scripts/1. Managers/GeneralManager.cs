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
}
/*
PlayerPrefs:
- MusicVolume (float)
- SFXVolume (float)
- Item_<itemID> (int: 0 o 1)
*/
public class DataManager : MonoBehaviour
{
    [Header("Audio")]
    public AudioMixer audioMixer;     // Mixer con Music y SFX
    public string musicParam = "MusicVolume";
    public string sfxParam = "SFXVolume";

    [Header("Inventario (IDs)")]
    public string[] inventoryItems;

    public static DataManager Instance;

    void Awake()
    {
        // Singleton (persiste entre escenas)
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadSettings();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // =========================
    // AUDIO
    // =========================

    public void SetMusicVolume(float value)
    {
        audioMixer.SetFloat(musicParam, Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    public void SetSFXVolume(float value)
    {
        audioMixer.SetFloat(sfxParam, Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    // =========================
    // INVENTARIO
    // =========================

    public void SetItem(string itemID, bool obtained)
    {
        PlayerPrefs.SetInt("Item_" + itemID, obtained ? 1 : 0);
    }

    public bool HasItem(string itemID)
    {
        return PlayerPrefs.GetInt("Item_" + itemID, 0) == 1;
    }

    // =========================
    // CARGA
    // =========================

    void LoadSettings()
    {
        // Audio
        float music = PlayerPrefs.GetFloat("MusicVolume", 0.8f);
        float sfx = PlayerPrefs.GetFloat("SFXVolume", 0.8f);

        audioMixer.SetFloat(musicParam, Mathf.Log10(music) * 20);
        audioMixer.SetFloat(sfxParam, Mathf.Log10(sfx) * 20);
    }

    public void SaveAll()
    {
        PlayerPrefs.Save();
    }
}