using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;

public class RatingSender
{
    private string api_token = "nL3ggwGvsiYZ5vzCqhAL58WnDcZgB9ad7FtDv82oaAAYa36UoJPS35sIbR9F";
    private string verifyUrl = "https://phpstack-1076337-5399863.cloudwaysapps.com/api/verify";
    private string rateUrl = "https://phpstack-1076337-5399863.cloudwaysapps.com/api/rateGame";
    private string scoreUrl = "https://phpstack-1076337-5399863.cloudwaysapps.com/api/classification";

    // Flag para activar/desactivar la API
    public bool useAPI = true;

    // Clase interna para parsear la respuesta de verify
    [System.Serializable]
    private class VerifyResponse
    {
        public bool rated;
    }

    // Llamada principal para enviar todo
    public void SendAll(MonoBehaviour runner, string username, string email, int score,
                        int general, int jug, int dif, int gra, int concordancia,
                        System.Action onComplete = null)
    {
        if (useAPI)
        {
            runner.StartCoroutine(VerifyScoreAndRate(username, email, score, general, jug, dif, gra, concordancia, onComplete));
        }
        else
        {
            // Guardar internamente sin usar API
            PlayerPrefs.SetInt("rating_general", general);
            PlayerPrefs.SetInt("rating_jugabilidad", jug);
            PlayerPrefs.SetInt("rating_dificultad", dif);
            PlayerPrefs.SetInt("rating_graficos", gra);
            PlayerPrefs.SetInt("rating_concordancia", concordancia);
            PlayerPrefs.SetInt("score", score);
            PlayerPrefs.Save();

            Debug.Log("⚡ API desactivada: rating guardado localmente.");
            onComplete?.Invoke();
        }
    }

    // Corrutina para enviar verify, score y rating
    private IEnumerator VerifyScoreAndRate(string username, string email, int score,
                                           int general, int jug, int dif, int gra, int concordancia,
                                           System.Action onComplete)
    {
        // ------------------- 1️⃣ Verificar usuario -------------------
        UserDTO userData = new UserDTO
        {
            api_token = api_token,
            name = username,
            email = email
        };
        string jsonVerify = JsonUtility.ToJson(userData);

        UnityWebRequest verifyReq = new UnityWebRequest(verifyUrl, "POST");
        verifyReq.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonVerify));
        verifyReq.downloadHandler = new DownloadHandlerBuffer();
        verifyReq.SetRequestHeader("Content-Type", "application/json");
        yield return verifyReq.SendWebRequest();

        if (verifyReq.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("❌ Error verify: " + verifyReq.error + " | " + verifyReq.downloadHandler.text);
            yield break;
        }

        // Parsear respuesta
        VerifyResponse verifyResp = JsonUtility.FromJson<VerifyResponse>(verifyReq.downloadHandler.text);

        // ------------------- Debug: rating previo -------------------
        if (verifyResp.rated)
        {
            Debug.Log("⚠️ El jugador YA había enviado rating anteriormente.");
        }
        else
        {
            Debug.Log("✅ El jugador NO había enviado rating. Se procederá a enviar.");
        }

        // ------------------- 2️⃣ Enviar puntuación -------------------
        PostScoreDTO scoreData = new PostScoreDTO
        {
            api_token = api_token,
            name = username,
            email = email,
            puntuacion = score
        };
        string jsonScore = JsonUtility.ToJson(scoreData);

        UnityWebRequest scoreReq = new UnityWebRequest(scoreUrl, "POST");
        scoreReq.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonScore));
        scoreReq.downloadHandler = new DownloadHandlerBuffer();
        scoreReq.SetRequestHeader("Content-Type", "application/json");
        yield return scoreReq.SendWebRequest();

        if (scoreReq.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("❌ Error score: " + scoreReq.error + " | " + scoreReq.downloadHandler.text);
            yield break;
        }
        Debug.Log("✅ Score enviado correctamente: " + scoreReq.downloadHandler.text);

        // ------------------- 3️⃣ Enviar rating -------------------
        RatingDTO rateData = new RatingDTO
        {
            api_token = api_token,
            name = username,
            email = email,
            general = general,
            jugabilitat = jug,
            dificultat = dif,
            grafics = gra,
            concordancia = concordancia
        };
        string jsonRate = JsonUtility.ToJson(rateData);

        UnityWebRequest rateReq = new UnityWebRequest(rateUrl, "POST");
        rateReq.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonRate));
        rateReq.downloadHandler = new DownloadHandlerBuffer();
        rateReq.SetRequestHeader("Content-Type", "application/json");
        yield return rateReq.SendWebRequest();

        if (rateReq.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("❌ Error rating: " + rateReq.error + " | " + rateReq.downloadHandler.text);
            yield break;
        }
        Debug.Log("✅ Rating enviado correctamente: " + rateReq.downloadHandler.text);

        // ------------------- Finalizar -------------------
        onComplete?.Invoke();
    }
}