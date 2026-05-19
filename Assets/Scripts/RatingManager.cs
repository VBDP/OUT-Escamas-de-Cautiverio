using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class RatingManager : MonoBehaviour
{
    [Header("UI de Rating")]
    public Image jugabilidadFilledImage;
    public Image dificultadFilledImage;
    public Image graficosFilledImage;
    public Image concordanciaFilledImage;
    public Image generalFilledImage;

    public RectTransform jugabilidadRect;
    public RectTransform dificultadRect;
    public RectTransform graficosRect;
    public RectTransform concordanciaRect;
    public RectTransform generalRect;

    public TextMeshProUGUI errorText;

    void Awake()
    {
        // Inicializar barras al mínimo 1 estrella
        jugabilidadFilledImage.fillAmount = 0.2f;
        dificultadFilledImage.fillAmount = 0.2f;
        graficosFilledImage.fillAmount = 0.2f;
        concordanciaFilledImage.fillAmount = 0.2f;
        generalFilledImage.fillAmount = 0.2f;
    }

    // ------------------- Click en cada barra -------------------
    void SetRating(Image img, RectTransform rect, PointerEventData eventData)
    {
        if (eventData == null) return;

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, eventData.position, eventData.pressEventCamera, out localPoint);
        float width = rect.rect.width;
        float normalized = (localPoint.x + width * 0.5f) / width;
        normalized = Mathf.Clamp01(normalized);

        float step = 0.2f; // incrementos de 1 estrella
        normalized = Mathf.Round(normalized / step) * step;
        img.fillAmount = Mathf.Max(normalized, 0.2f); // mínimo 1 estrella
    }

    public void ClickJugabilidad(BaseEventData data) { SetRating(jugabilidadFilledImage, jugabilidadRect, data as PointerEventData); }
    public void ClickDificultad(BaseEventData data) { SetRating(dificultadFilledImage, dificultadRect, data as PointerEventData); }
    public void ClickGraficos(BaseEventData data) { SetRating(graficosFilledImage, graficosRect, data as PointerEventData); }
    public void ClickConcordancia(BaseEventData data) { SetRating(concordanciaFilledImage, concordanciaRect, data as PointerEventData); }
    public void ClickGeneral(BaseEventData data) { SetRating(generalFilledImage, generalRect, data as PointerEventData); }

    // ------------------- Enviar -------------------
    public void Submit()
    {
        // Validar que todas las categorías tengan al menos 1 estrella
        if (jugabilidadFilledImage.fillAmount <= 0f ||
            dificultadFilledImage.fillAmount <= 0f ||
            graficosFilledImage.fillAmount <= 0f ||
            concordanciaFilledImage.fillAmount <= 0f ||
            generalFilledImage.fillAmount <= 0f)
        {
            errorText.text = "Por favor, califica todas las categorías antes de enviar.";
            return;
        }

        errorText.text = "¡Rating guardado localmente!";
        
        // Delay antes de volver al menú principal
        StartCoroutine(DelayAndLoadMenu());
    }

    private IEnumerator DelayAndLoadMenu()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadSceneAsync("MainMenu");
    }
}