using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenuManager : MonoBehaviour
{


  public void OpenTutorial()
    {
        SceneManager.LoadSceneAsync("Tutorial");
    }

      public void OpenLevel1()
    {
        SceneManager.LoadSceneAsync("Level1");
    }

  public void ExitGame()
{
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#endif
    Application.Quit();
}

}
