using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwitch : MonoBehaviour
{
    [SerializeField] private string selectedScene;
    [SerializeField] private GameObject ratingPanel;

   [SerializeField] private UserDataSaver userDataSaver;

    // Actualiza el texto del botón según el estado de hasRated
    public void selectAction()
    {
        if (!userDataSaver.HasRated())
        {
            Debug.Log("No hay rating, abriendo panel");
            EnableRatingPanel();
        }
        else
        {
            Debug.Log("Hay rating, Abriendo menú principal");
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