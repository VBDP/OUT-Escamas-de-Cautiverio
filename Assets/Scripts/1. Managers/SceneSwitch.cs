using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    [SerializeField] private string selectedScene;

    public void selectAction()
    {
        SwitchScene();
    }

    public void SwitchScene()
    {
        SceneManager.LoadSceneAsync(selectedScene);
    }
}