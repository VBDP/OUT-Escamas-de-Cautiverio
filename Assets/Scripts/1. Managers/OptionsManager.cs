using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class OptionsManager : MonoBehaviour
{
    void Awake()
    {
        // Initialize sliders and texts with default values
        musicVolumeSlider.value = musicVolumeLevel;
        musicVolumeText.text = (musicVolumeLevel * 100).ToString("0") + "%";

        sfxVolumeSlider.value = sfxVolumeLevel;
        sfxVolumeText.text = (sfxVolumeLevel * 100).ToString("0") + "%";

        mouseSensitivitySlider.value = mouseSensitivityLevel;
        mouseSensitivityText.text = (mouseSensitivityLevel * 100).ToString("0") + "%";
    }
   
// Music Volume management variables
    private float musicVolumeLevel = 0.75f;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private TextMeshProUGUI musicVolumeText;

    public void OnMusicVolumeChanged()
    {
        musicVolumeLevel = musicVolumeSlider.value;
        musicVolumeText.text = (musicVolumeLevel * 100).ToString("0") + "%";
        // Here you would typically also update the actual music volume in your audio manager
    }

//SFX Volume management variables
    private float sfxVolumeLevel = 0.75f;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private TextMeshProUGUI sfxVolumeText;

    public void OnSFXVolumeChanged()
    {
        sfxVolumeLevel = sfxVolumeSlider.value;
        sfxVolumeText.text = (sfxVolumeLevel * 100).ToString("0") + "%";
        // Here you would typically also update the actual SFX volume in your audio manager
    }

 //Mouse Sensitivity management variables
    private float mouseSensitivityLevel = 0.5f;
    [SerializeField] private Slider mouseSensitivitySlider;
    [SerializeField] private TextMeshProUGUI mouseSensitivityText;

    public void OnMouseSensitivityChanged()
    {
        mouseSensitivityLevel = mouseSensitivitySlider.value;
        mouseSensitivityText.text = (mouseSensitivityLevel * 100).ToString("0") + "%";
        // Here you would typically also update the actual mouse sensitivity in your input manager
    }
}
