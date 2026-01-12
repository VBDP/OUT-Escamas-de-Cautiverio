using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneSwitch : MonoBehaviour
{
    [SerializeField] private string selectedScene;
    public void SwitchScene()
    {
        SceneManager.LoadSceneAsync(selectedScene);
    }
}
