using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class RatingManager : MonoBehaviour
{
    public Image jugabilidadFilledImage;
    public Image dificultadFilledImage;
    public Image graficosFilledImage;

    public RectTransform jugabilidadRect;
    public RectTransform dificultadRect;
    public RectTransform graficosRect;

    public TextMeshProUGUI errorText;

    void Start()
    {
        jugabilidadFilledImage.fillAmount = 0.2f;
        dificultadFilledImage.fillAmount = 0.2f;
        graficosFilledImage.fillAmount = 0.2f;
    }

    void SetRating(Image img, RectTransform rect, PointerEventData eventData)
    {
        if (eventData == null) return;

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, eventData.position, eventData.pressEventCamera, out localPoint);
        float width = rect.rect.width;
        float normalized = (localPoint.x + width * 0.5f) / width;
        normalized = Mathf.Clamp01(normalized);

        float step = 0.2f;
        normalized = Mathf.Round(normalized / step) * step;
        img.fillAmount = Mathf.Max(normalized, 0.2f); // mínimo 1 estrella
    }

    public void ClickJugabilidad(BaseEventData data) { SetRating(jugabilidadFilledImage, jugabilidadRect, data as PointerEventData); }
    public void ClickDificultad(BaseEventData data) { SetRating(dificultadFilledImage, dificultadRect, data as PointerEventData); }
    public void ClickGraficos(BaseEventData data) { SetRating(graficosFilledImage, graficosRect, data as PointerEventData); }

    public void Submit()
    {
        if(jugabilidadFilledImage.fillAmount <= 0f ||
           dificultadFilledImage.fillAmount <= 0f ||
           graficosFilledImage.fillAmount <= 0f)
        {
            errorText.text = "Por favor, califica todas las categorías antes de enviar.";
            return;
        }

        string username = PlayerPrefs.GetString("username", "Player");
        string email = PlayerPrefs.GetString("email", "noemail@game.com");

        int jug = Mathf.RoundToInt(jugabilidadFilledImage.fillAmount * 5);
        int dif = Mathf.RoundToInt(dificultadFilledImage.fillAmount * 5);
        int gra = Mathf.RoundToInt(graficosFilledImage.fillAmount * 5);
        int gen = (jug + dif + gra) / 3;
        int concordancia = 5;

        StartCoroutine(SendRatingAndWait(username, email, gen, jug, dif, gra, concordancia));
    }

    private IEnumerator SendRatingAndWait(string username, string email, int gen, int jug, int dif, int gra, int concordancia)
    {
        RatingSender sender = new RatingSender();
        bool finished = false;
        // Usamos SendAll, que hace: verificar usuario → enviar score → enviar rating
        int score = PlayerPrefs.GetInt("score", 0); // si tienes un score guardado
        sender.SendAll(this, username, email, score, gen, jug, dif, gra, concordancia, () => { finished = true; });

        // Espera a que se termine de enviar
        while (!finished)
        {
            yield return null;
        }

        errorText.text = "✅ ¡Rating enviado correctamente!";

        // Delay para que el usuario vea el mensaje
        yield return new WaitForSeconds(1.5f);

        // Cambiar escena después de enviar
        SceneManager.LoadSceneAsync("MainMenu");
    }
}