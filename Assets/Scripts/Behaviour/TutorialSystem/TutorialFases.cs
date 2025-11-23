using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TutorialFases : MonoBehaviour
{
      public TutorialSystem TutorialSystem;
    public TextMeshProUGUI textTutorial;
    public Image imageTutorial;
    public string TextoTutorial;
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
            TutorialSystem.PanelDeactivate();
            textTutorial.text = "";
        }
    }

    public void MostrarPanel()
    {
        imageTutorial.sprite = spriteTutorial;
        textTutorial.text = TextoTutorial + "  Pulsa la tecla E para continuar";
        finalizado = 1;
        TutorialSystem.PanelActivate();
        
    }

}
