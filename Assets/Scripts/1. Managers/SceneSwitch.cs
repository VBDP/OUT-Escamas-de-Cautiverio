using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneSwitch : MonoBehaviour
{
    [SerializeField] private string selectedScene;
    [SerializeField] private GameObject ratingPanel;
    [SerializeField] private bool hasRated;
    [SerializeField] private TextMeshProUGUI submitText;

    public void Start()
    {
            if (submitText != null)
            {
                if(hasRated)
                {
                    submitText.text = "Main Menu";
                }
                else
                {
                    submitText.text = "Rate the game";
                }
            }
    }

    public void selectAction()
    {
        if (!hasRated)
        {
            EnableRatingPanel();
        }
        else
        {
            SwitchScene();
        }
    }

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
