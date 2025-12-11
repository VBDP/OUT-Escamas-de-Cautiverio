using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;



public class MainMenuManager : MonoBehaviour
{

  [SerializeField] private List<GameObject> Panels;
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
    foreach (GameObject panel in Panels)
    {
        panel.SetActive(false);
    }
      Panels[0].SetActive(true);
    }

    public void OpenCredits()
    {
    foreach (GameObject panel in Panels)
    {
        panel.SetActive(false);
    }
      Panels[1].SetActive(true);
    }

        public void OpenOptions()
    {
    foreach (GameObject panel in Panels)
    {
        panel.SetActive(false);
    }
      Panels[2].SetActive(true);
    }

        public void OpenLeaderboard()
    {
    foreach (GameObject panel in Panels)
    {
        panel.SetActive(false);
    }
      Panels[3].SetActive(true);
    }
    



  public void ExitGame()
{
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#endif
    Application.Quit();
}

}
