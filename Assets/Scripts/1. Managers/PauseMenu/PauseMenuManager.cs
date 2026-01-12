using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> panels;

    public void ReturnToGame()
    {
        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }
        panels[0].SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OpenSettingsMenu()
    {
        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }
        panels[3].SetActive(true);
    }

    public void OpenOptionsMenu()
    {
        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }
        panels[4].SetActive(true);
    }

    public void MainMenu()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
