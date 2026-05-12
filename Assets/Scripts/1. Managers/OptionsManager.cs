using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsManager : MonoBehaviour
{
    private GeneralManager generalManager;

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    private PlayerMovement player;

    // -------------------------
    // Music
    // -------------------------

    private float musicVolumeLevel;

    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private TextMeshProUGUI musicVolumeText;

    // -------------------------
    // SFX
    // -------------------------

    private float sfxVolumeLevel;

    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private TextMeshProUGUI sfxVolumeText;

    // -------------------------
    // Mouse Sensitivity
    // -------------------------

    private float mouseSensitivityLevel;

    [SerializeField] private Slider mouseSensitivitySlider;
    [SerializeField] private TextMeshProUGUI mouseSensitivityText;

    // -------------------------
    // API Setting
    // -------------------------
    private bool apiEnabled;
    [SerializeField] private Toggle apiToggle;

    void Awake()
    {
        // Cargar valores guardados
        musicVolumeLevel = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        sfxVolumeLevel = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        mouseSensitivityLevel = PlayerPrefs.GetFloat("MouseSensitivity", 0.4f);
        apiEnabled = PlayerPrefs.GetInt("ApiEnabled", 1) == 1;

        musicVolumeSlider.value = musicVolumeLevel;
        sfxVolumeSlider.value = sfxVolumeLevel;
        mouseSensitivitySlider.value = mouseSensitivityLevel;
        if (apiToggle != null) apiToggle.isOn = apiEnabled;

        // Buscar managers
        generalManager = FindFirstObjectByType<GeneralManager>();

        if (generalManager != null)
        {
            musicSource = generalManager.musicSource;
            sfxSource = generalManager.sfxSource;
        }

        player = FindFirstObjectByType<PlayerMovement>();

        // Aplicar valores
        SetMusicVolume(musicVolumeLevel);
        SetSFXVolume(sfxVolumeLevel);
        ApplyMouseSensitivity();

        // Inicializar UI
        InitializeUI();
    }

    void InitializeUI()
    {
        musicVolumeSlider.value = musicVolumeLevel;
        musicVolumeText.text = (musicVolumeLevel * 100).ToString("0") + "%";

        sfxVolumeSlider.value = sfxVolumeLevel;
        sfxVolumeText.text = (sfxVolumeLevel * 100).ToString("0") + "%";

        mouseSensitivitySlider.value = mouseSensitivityLevel;
        mouseSensitivityText.text = (mouseSensitivityLevel * 100).ToString("0") + "%";

        if (apiToggle != null) apiToggle.isOn = apiEnabled;
    }

    // -------------------------
    // MUSIC
    // -------------------------

    public void OnMusicVolumeChanged()
    {
        musicVolumeLevel = musicVolumeSlider.value;

        musicVolumeText.text = (musicVolumeLevel * 100).ToString("0") + "%";

        PlayerPrefs.SetFloat("MusicVolume", musicVolumeLevel);
        PlayerPrefs.Save();

        SetMusicVolume(musicVolumeLevel);
    }

    void SetMusicVolume(float value)
    {
        if (musicSource != null)
        {
            musicSource.volume = value;
        }
    }

    // -------------------------
    // SFX
    // -------------------------

    public void OnSFXVolumeChanged()
    {
        sfxVolumeLevel = sfxVolumeSlider.value;

        sfxVolumeText.text = (sfxVolumeLevel * 100).ToString("0") + "%";

        PlayerPrefs.SetFloat("SFXVolume", sfxVolumeLevel);
        PlayerPrefs.Save();

        SetSFXVolume(sfxVolumeLevel);
    }

    void SetSFXVolume(float value)
    {
        if (sfxSource != null)
        {
            sfxSource.volume = value;
        }
    }

    // -------------------------
    // MOUSE SENSITIVITY
    // -------------------------

    public void OnMouseSensitivityChanged()
    {
        mouseSensitivityLevel = mouseSensitivitySlider.value;

        mouseSensitivityText.text = (mouseSensitivityLevel * 100).ToString("0") + "%";

        PlayerPrefs.SetFloat("MouseSensitivity", mouseSensitivityLevel);
        PlayerPrefs.Save();

        ApplyMouseSensitivity();
    }

    void ApplyMouseSensitivity()
    {
        if (player != null)
        {
            player.SetMouseSensitivity(Mathf.Lerp(0.1f, 2.0f, mouseSensitivityLevel));
        }
    }

    // -------------------------
    // API
    // -------------------------
    
    public void OnApiToggleChanged()
    {
        if (apiToggle != null)
        {
            apiEnabled = apiToggle.isOn;
            PlayerPrefs.SetInt("ApiEnabled", apiEnabled ? 1 : 0);
            PlayerPrefs.Save();
            Debug.Log("API Enabled: " + apiEnabled);
        }
    }
}