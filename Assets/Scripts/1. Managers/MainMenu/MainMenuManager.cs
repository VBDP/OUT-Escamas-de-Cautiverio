using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> panels;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;

    void Start()
    {
        LoadAudioSettings();
    }

    void LoadAudioSettings()
    {
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);

        if (musicSource != null)
            musicSource.volume = musicVolume;
    }

    // -----------------------
    // SCENES
    // -----------------------

    public void OpenTutorial()
    {
        SceneManager.LoadSceneAsync("Tutorial");
    }

    public void OpenLevel1()
    {
        SceneManager.LoadSceneAsync("Level1");
    }

    // -----------------------
    // PANELS
    // -----------------------

    void DisableAllPanels()
    {
        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }
    }

    public void OpenMenu()
    {
        DisableAllPanels();
        panels[0].SetActive(true);
    }

    public void OpenCredits()
    {
        DisableAllPanels();
        panels[1].SetActive(true);
    }

    public void OpenOptions()
    {
        DisableAllPanels();
        panels[2].SetActive(true);
    }

    public void OpenLeaderboard()
    {
        DisableAllPanels();
        panels[3].SetActive(true);
    }

    // -----------------------
    // EXIT
    // -----------------------

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
