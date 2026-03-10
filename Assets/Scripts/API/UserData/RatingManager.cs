using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Networking;

[System.Serializable]
public class VerifyResponse
{
    public bool rated;
    public Criterion[] criterion;
}

[System.Serializable]
public class Criterion
{
    public string name;
    public int min_score;
    public int max_score;
}

public class RatingManager : MonoBehaviour
{
    [Header("Images")]
    public Image jugabilidadFilledImage;
    public Image dificultadFilledImage;
    public Image graficosFilledImage;
    public Image generalFilledImage;
    public Image concordanciaFilledImage;

    [Header("Rects")]
    public RectTransform jugabilidadRect;
    public RectTransform dificultadRect;
    public RectTransform graficosRect;
    public RectTransform generalRect;
    public RectTransform concordanciaRect;

    [Header("UI")]
    public TextMeshProUGUI errorText;

    private string username;
    private string email;
    private string api_token = "nL3ggwGvsiYZ5vzCqhAL58WnDcZgB9ad7FtDv82oaAAYa36UoJPS35sIbR9F";

    // Guardar rating seleccionado
    private float jugSelected = 0.2f;
    private float difSelected = 0.2f;
    private float graSelected = 0.2f;
    private float genSelected = 0.2f;
    private float conSelected = 0.2f;

    void Start()
    {
        // Inicializar
        jugabilidadFilledImage.fillAmount = 0.2f;
        dificultadFilledImage.fillAmount = 0.2f;
        graficosFilledImage.fillAmount = 0.2f;
        generalFilledImage.fillAmount = 0.2f;
        concordanciaFilledImage.fillAmount = 0.2f;

        username = PlayerPrefs.GetString("username", "Player");
        email = PlayerPrefs.GetString("email", "noemail@game.com");
    }

    // ---------------------- Hover / Click ----------------------
    private void HoverRating(Image img, RectTransform rect, PointerEventData eventData)
    {
        if (eventData == null) return;
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, eventData.position, eventData.pressEventCamera, out localPoint);
        float width = rect.rect.width;
        float normalized = (localPoint.x + width * 0.5f) / width;
        normalized = Mathf.Clamp01(normalized);
        float step = 0.2f;
        normalized = Mathf.Round(normalized / step) * step;
        img.fillAmount = Mathf.Max(normalized, 0.2f);
    }

    private void SaveRating(Image img, ref float saved, RectTransform rect, PointerEventData eventData)
    {
        if (eventData == null) return;
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, eventData.position, eventData.pressEventCamera, out localPoint);
        float width = rect.rect.width;
        float normalized = (localPoint.x + width * 0.5f) / width;
        normalized = Mathf.Clamp01(normalized);
        float step = 0.2f;
        normalized = Mathf.Round(normalized / step) * step;
        normalized = Mathf.Max(normalized, 0.2f);
        img.fillAmount = normalized;
        saved = normalized;
    }

    private void ExitRating(Image img, float saved)
    {
        img.fillAmount = saved;
    }

    // ---------------------- Jugabilidad ----------------------
    public void HoverJugabilidad(BaseEventData data) => HoverRating(jugabilidadFilledImage, jugabilidadRect, data as PointerEventData);
    public void ClickJugabilidad(BaseEventData data) => SaveRating(jugabilidadFilledImage, ref jugSelected, jugabilidadRect, data as PointerEventData);
    public void ExitJugabilidad() => ExitRating(jugabilidadFilledImage, jugSelected);

    // ---------------------- Dificultad ----------------------
    public void HoverDificultad(BaseEventData data) => HoverRating(dificultadFilledImage, dificultadRect, data as PointerEventData);
    public void ClickDificultad(BaseEventData data) => SaveRating(dificultadFilledImage, ref difSelected, dificultadRect, data as PointerEventData);
    public void ExitDificultad() => ExitRating(dificultadFilledImage, difSelected);

    // ---------------------- Gráficos ----------------------
    public void HoverGraficos(BaseEventData data) => HoverRating(graficosFilledImage, graficosRect, data as PointerEventData);
    public void ClickGraficos(BaseEventData data) => SaveRating(graficosFilledImage, ref graSelected, graficosRect, data as PointerEventData);
    public void ExitGraficos() => ExitRating(graficosFilledImage, graSelected);

    // ---------------------- General ----------------------
    public void HoverGeneral(BaseEventData data) => HoverRating(generalFilledImage, generalRect, data as PointerEventData);
    public void ClickGeneral(BaseEventData data) => SaveRating(generalFilledImage, ref genSelected, generalRect, data as PointerEventData);
    public void ExitGeneral() => ExitRating(generalFilledImage, genSelected);

    // ---------------------- Concordancia ----------------------
    public void HoverConcordancia(BaseEventData data) => HoverRating(concordanciaFilledImage, concordanciaRect, data as PointerEventData);
    public void ClickConcordancia(BaseEventData data) => SaveRating(concordanciaFilledImage, ref conSelected, concordanciaRect, data as PointerEventData);
    public void ExitConcordancia() => ExitRating(concordanciaFilledImage, conSelected);

    // ---------------------- Submit ----------------------
    public void CheckAndSubmit()
    {
        StartCoroutine(CheckIfRated());
    }

    private IEnumerator CheckIfRated()
    {
        string url = "https://phpstack-1076337-5399863.cloudwaysapps.com/api/verify";
        string json = JsonUtility.ToJson(new { api_token = api_token, name = username, email = email });
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            errorText.text = "❌ Error de conexión: " + request.error;
            yield break;
        }

        VerifyResponse resp = JsonUtility.FromJson<VerifyResponse>(request.downloadHandler.text);

        if (resp.rated)
        {
            errorText.text = "⚠️ Ya has enviado tu valoración anteriormente.";
        }
        else
        {
            Submit();
        }
    }

    private void Submit()
    {
        // Validación mínima
        if (jugSelected <= 0 || difSelected <= 0 || graSelected <= 0 || genSelected <= 0 || conSelected <= 0)
        {
            errorText.text = "Por favor, califica todas las categorías antes de enviar.";
            return;
        }

        int jug = Mathf.RoundToInt(jugSelected * 5);
        int dif = Mathf.RoundToInt(difSelected * 5);
        int gra = Mathf.RoundToInt(graSelected * 5);
        int gen = Mathf.RoundToInt(genSelected * 5);
        int concordancia = Mathf.RoundToInt(conSelected * 5);
        int score = PlayerPrefs.GetInt("score", 0);

        StartCoroutine(SendRatingAndWait(username, email, score, gen, jug, dif, gra, concordancia));
    }

    private IEnumerator SendRatingAndWait(string username, string email, int score, int gen, int jug, int dif, int gra, int concordancia)
    {
        RatingSender sender = new RatingSender();
        bool finished = false;

        sender.SendAll(this, username, email, score, gen, jug, dif, gra, concordancia, () => { finished = true; });

        while (!finished) yield return null;

        errorText.text = "✅ ¡Rating enviado correctamente!";
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadSceneAsync("MainMenu");
    }
}