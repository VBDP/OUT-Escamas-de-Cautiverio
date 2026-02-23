using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    private GeneralManager generalManager;
    private AudioSource musicSource;
    private AudioSource sfxSource;

    [Header("Music Settings")]
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private TextMeshProUGUI musicVolumeText;
    private float musicVolumeLevel = 0.5f;

    [Header("SFX Settings")]
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private TextMeshProUGUI sfxVolumeText;
    private float sfxVolumeLevel = 0.5f;

    [Header("Mouse Settings")]
    [SerializeField] private Slider mouseSensitivitySlider;
    [SerializeField] private TextMeshProUGUI mouseSensitivityText;
    private float mouseSensitivityLevel = 0.5f;

    void Awake()
    {
        generalManager = FindObjectOfType<GeneralManager>();
        musicSource = generalManager.musicSource;
        sfxSource = generalManager.sfxSource;

        // Cargar valores guardados
        musicVolumeLevel = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        sfxVolumeLevel = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        mouseSensitivityLevel = PlayerPrefs.GetFloat("MouseSensitivity", 0.5f);
    }

    void Start()
    {
        // Inicializar sliders y textos con valores cargados
        UpdateMusicUI();
        UpdateSFXUI();
        UpdateMouseUI();

        // Aplicar volúmenes
        SetMusicVolume(musicVolumeLevel);
        SetSFXVolume(sfxVolumeLevel);

        // Aplicar sensibilidad al jugador
        PlayerMovement player = FindObjectOfType<PlayerMovement>();
        if (player != null)
            player.SetMouseSensitivity(mouseSensitivityLevel * 10f); // Ajusta escala si quieres
    }

    #region Music
    public void OnMusicVolumeChanged()
    {
        musicVolumeLevel = musicVolumeSlider.value;
        PlayerPrefs.SetFloat("MusicVolume", musicVolumeLevel);
        UpdateMusicUI();
        SetMusicVolume(musicVolumeLevel);
    }

    private void UpdateMusicUI()
    {
        if (musicVolumeSlider) musicVolumeSlider.value = musicVolumeLevel;
        if (musicVolumeText) musicVolumeText.text = (musicVolumeLevel * 100).ToString("0") + "%";
    }

    public void SetMusicVolume(float value)
    {
        if (musicSource) musicSource.volume = value;
    }
    #endregion

    #region SFX
    public void OnSFXVolumeChanged()
    {
        sfxVolumeLevel = sfxVolumeSlider.value;
        PlayerPrefs.SetFloat("SFXVolume", sfxVolumeLevel);
        UpdateSFXUI();
        SetSFXVolume(sfxVolumeLevel);
    }

    private void UpdateSFXUI()
    {
        if (sfxVolumeSlider) sfxVolumeSlider.value = sfxVolumeLevel;
        if (sfxVolumeText) sfxVolumeText.text = (sfxVolumeLevel * 100).ToString("0") + "%";
    }

    public void SetSFXVolume(float value)
    {
        if (sfxSource) sfxSource.volume = value;
    }
    #endregion

    #region Mouse
    public void OnMouseSensitivityChanged()
    {
        mouseSensitivityLevel = mouseSensitivitySlider.value;
        PlayerPrefs.SetFloat("MouseSensitivity", mouseSensitivityLevel);
        UpdateMouseUI();

        PlayerMovement player = FindObjectOfType<PlayerMovement>();
        if (player != null)
            player.SetMouseSensitivity(mouseSensitivityLevel * 10f); // Escala como prefieras
    }

    private void UpdateMouseUI()
    {
        if (mouseSensitivitySlider) mouseSensitivitySlider.value = mouseSensitivityLevel;
        if (mouseSensitivityText) mouseSensitivityText.text = (mouseSensitivityLevel * 100).ToString("0") + "%";
    }
    #endregion
}