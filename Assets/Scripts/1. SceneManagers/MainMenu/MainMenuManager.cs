using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenuManager : MonoBehaviour
{

  public void OpenTutorial()
    {
        SceneManager.LoadSceneAsync("Tutorial");

    }

  public void ExitGame()
{
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#endif
    Application.Quit();
}

}
