using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Audio;
using Unity.VisualScripting;

public class OptionsManager : MonoBehaviour
{
    private GeneralManager generalManager;
    private AudioSource musicSource;
    private AudioSource sfxSource;


    void Awake()
    {
        generalManager = FindObjectOfType<GeneralManager>();
        musicSource = generalManager.musicSource;
        sfxSource = generalManager.sfxSource;
        // Initialize sliders and texts with default values
        musicVolumeSlider.value = musicVolumeLevel;
        musicVolumeText.text = (musicVolumeLevel * 100).ToString("0") + "%";

        sfxVolumeSlider.value = sfxVolumeLevel;
        sfxVolumeText.text = (sfxVolumeLevel * 100).ToString("0") + "%";

        mouseSensitivitySlider.value = mouseSensitivityLevel;
        mouseSensitivityText.text = (mouseSensitivityLevel * 100).ToString("0") + "%";
    }

    void start()
    {
        musicSource = generalManager.musicSource;
        sfxSource = generalManager.sfxSource;

        SetMusicVolume(musicVolumeLevel);
        SetSFXVolume(sfxVolumeLevel);
    }

    // Music Volume management variables
    private float musicVolumeLevel = 0.50f;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private TextMeshProUGUI musicVolumeText;

    public void OnMusicVolumeChanged()
    {
        musicVolumeLevel = musicVolumeSlider.value;
        musicVolumeText.text = (musicVolumeLevel * 100).ToString("0") + "%";
        PlayerPrefs.SetFloat("MusicVolume", musicVolumeLevel);
        SetMusicVolume(musicVolumeLevel);
    }

    //SFX Volume management variables
    private float sfxVolumeLevel = 0.50f;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private TextMeshProUGUI sfxVolumeText;

    public void OnSFXVolumeChanged()
    {
        sfxVolumeLevel = sfxVolumeSlider.value;
        sfxVolumeText.text = (sfxVolumeLevel * 100).ToString("0") + "%";
        PlayerPrefs.SetFloat("SFXVolume", sfxVolumeLevel);
        SetSFXVolume(sfxVolumeLevel);
    }

    //Mouse Sensitivity management variables
    private float mouseSensitivityLevel = 0.5f;
    [SerializeField] private Slider mouseSensitivitySlider;
    [SerializeField] private TextMeshProUGUI mouseSensitivityText;

    public void OnMouseSensitivityChanged()
    {
        mouseSensitivityLevel = mouseSensitivitySlider.value;
        mouseSensitivityText.text = (mouseSensitivityLevel * 100).ToString("0") + "%";
        PlayerPrefs.SetFloat("MouseSensitivity", mouseSensitivityLevel);
        // Here you would typically also update the actual mouse sensitivity in your input manager
    }

    public void SetMusicVolume(float value)
    {
        musicSource.volume = value;
    }

    public void SetSFXVolume(float value)
    {
        sfxSource.volume = value;
    }
}
