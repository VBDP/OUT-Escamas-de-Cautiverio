using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;



public class MainMenuManager : MonoBehaviour
{

  [SerializeField] private List<GameObject> panels;
  public void OpenTutorial()
    {
        SceneManager.LoadSceneAsync("Tutorial");
    }

      public void OpenLevel1()
    {
        SceneManager.LoadSceneAsync("Level1");
    }

        public void OpenMenu()
    {
    foreach (GameObject panel in panels)
    {
        panel.SetActive(false);
    }
      panels[0].SetActive(true);
    }

    public void OpenCredits()
    {
    foreach (GameObject panel in panels)
    {
        panel.SetActive(false);
    }
      panels[1].SetActive(true);
    }

        public void OpenOptions()
    {
    foreach (GameObject panel in panels)
    {
        panel.SetActive(false);
    }
      panels[2].SetActive(true);
    }

        public void OpenLeaderboard()
    {
    foreach (GameObject panel in panels)
    {
        panel.SetActive(false);
    }
      panels[3].SetActive(true);
    }
    



  public void ExitGame()
{
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#endif
    Application.Quit();
}

}
