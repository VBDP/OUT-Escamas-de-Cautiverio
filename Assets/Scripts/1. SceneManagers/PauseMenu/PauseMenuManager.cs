using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> Panels;

    public void ReturnToGame()
    {
        foreach (GameObject panel in Panels)
    {
        panel.SetActive(false);
    }
      Panels[0].SetActive(true);
      Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void MainMenu()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
