using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TutorialFases : MonoBehaviour
{
    public TutorialSystem tutorialSystem;
    public TextMeshProUGUI textHolderTutorial;
    public Image imageTutorial;
    public string textTutorial;
    private int finalizado = 0;
    public Sprite spriteTutorial;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Wait(float t, System.Action a) { yield return new WaitForSeconds(t); a(); } //Co-rutina para la espera.
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && finalizado == 0)
        {
            StartCoroutine(Wait(0.5f, () => MostrarPanel())); //Espera 1 segundo
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            tutorialSystem.PanelDeactivate();
            textHolderTutorial.text = "";
        }
    }

    public void MostrarPanel()
    {
        imageTutorial.sprite = spriteTutorial;
        textHolderTutorial.text = textTutorial + "  Pulsa la tecla E para continuar";
        finalizado = 1;
        tutorialSystem.PanelActivate();
    }

}
