using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneSwitch : MonoBehaviour
{
    [SerializeField] private string selectedScene;
    [SerializeField] private GameObject ratingPanel;
    public void SwitchScene()
    {
        SceneManager.LoadSceneAsync(selectedScene);
    }

    public void EnableRatingPanel()
    {
        if (ratingPanel != null)
        {
            ratingPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("No se encontró el panel de calificación en la escena.");
        }
    }
}
