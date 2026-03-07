using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Text;

public class RatingSender
{
    private string api_token = "nL3ggwGvsiYZ5vzCqhAL58WnDcZgB9ad7FtDv82oaAAYa36UoJPS35sIbR9F";
    private string verifyUrl = "https://phpstack-1076337-5399863.cloudwaysapps.com/api/verify";
    private string rateUrl   = "https://phpstack-1076337-5399863.cloudwaysapps.com/api/rateGame";
    private string scoreUrl = "https://phpstack-1076337-5399863.cloudwaysapps.com/api/classification";

    public void SendRating(MonoBehaviour runner, string username, string email, int general, int jug, int dif, int gra, int concordancia)
    {
        runner.StartCoroutine(VerifyAndRate(username, email, general, jug, dif, gra, concordancia, runner));
    }

    private IEnumerator VerifyAndRate(string username, string email, int general, int jug, int dif, int gra, int concordancia, MonoBehaviour runner)
    {
        // ✅ 1. Verificar o crear usuario
        PostScoreDTO verifyData = new PostScoreDTO { api_token = api_token, name = username, email = email };
        string jsonVerify = JsonUtility.ToJson(verifyData);
        byte[] bodyVerify = Encoding.UTF8.GetBytes(jsonVerify);

        UnityWebRequest requestVerify = new UnityWebRequest(verifyUrl, "POST");
        requestVerify.uploadHandler = new UploadHandlerRaw(bodyVerify);
        requestVerify.downloadHandler = new DownloadHandlerBuffer();
        requestVerify.SetRequestHeader("Content-Type", "application/json");

        yield return requestVerify.SendWebRequest();

        if (requestVerify.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("❌ Error verify: " + requestVerify.error);
            yield break;
        }
        Debug.Log("✅ Usuario verificado: " + requestVerify.downloadHandler.text);

        // ✅ 2. Enviar rating
        RatingDTO rateData = new RatingDTO
        {
            api_token = api_token,
            email = email,
            name = username,
            general = general,
            jugabilitat = jug,
            dificultat = dif,
            grafics = gra,
            concordancia = concordancia
        };
        string jsonRate = JsonUtility.ToJson(rateData);
        byte[] bodyRate = Encoding.UTF8.GetBytes(jsonRate);

        UnityWebRequest requestRate = new UnityWebRequest(rateUrl, "POST");
        requestRate.uploadHandler = new UploadHandlerRaw(bodyRate);
        requestRate.downloadHandler = new DownloadHandlerBuffer();
        requestRate.SetRequestHeader("Content-Type", "application/json");

        yield return requestRate.SendWebRequest();

        if (requestRate.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("❌ Error rating: " + requestRate.error);
            yield break;
        }
        Debug.Log("✅ Rating enviado correctamente: " + requestRate.downloadHandler.text);
    }

    public void SendRating(MonoBehaviour coroutineRunner, string username, string email,
    int general, int jugabilitat, int dificultat, int grafics, int concordancia, System.Action onComplete)
{
    coroutineRunner.StartCoroutine(PostRating(username, email, general, jugabilitat, dificultat, grafics, concordancia, onComplete));
}

private IEnumerator PostRating(string username, string email,
    int general, int jugabilitat, int dificultat, int grafics, int concordancia, System.Action onComplete)
{
    string url = "https://phpstack-1076337-5399863.cloudwaysapps.com/api/rateGame";
    string api_token = "TU_TOKEN";

    RatingDTO postData = new RatingDTO
    {
        api_token = api_token,
        name = username,
        email = email,
        general = general,
        jugabilitat = jugabilitat,
        dificultat = dificultat,
        grafics = grafics,
        concordancia = concordancia
    };

    string json = JsonUtility.ToJson(postData);
    byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

    UnityWebRequest request = new UnityWebRequest(url, "POST");
    request.uploadHandler = new UploadHandlerRaw(bodyRaw);
    request.downloadHandler = new DownloadHandlerBuffer();
    request.SetRequestHeader("Content-Type", "application/json");

    yield return request.SendWebRequest();

    if (request.result == UnityWebRequest.Result.Success)
    {
        Debug.Log("✅ Rating enviado correctamente");
        Debug.Log(request.downloadHandler.text);
    }
    else
    {
        Debug.LogError("❌ Error al enviar rating: " + request.error);
    }

    // Llamar callback
    onComplete?.Invoke();
}

public void SendAll(MonoBehaviour runner, string username, string email, int score,
                    int general, int jug, int dif, int gra, int concordancia,
                    System.Action onComplete = null)
{
    runner.StartCoroutine(VerifyScoreAndRate(username, email, score, general, jug, dif, gra, concordancia, onComplete));
}

private IEnumerator VerifyScoreAndRate(string username, string email, int score,
                                       int general, int jug, int dif, int gra, int concordancia,
                                       System.Action onComplete)
{
    // 1️⃣ Verificar usuario
    PostScoreDTO verifyData = new PostScoreDTO { api_token = api_token, name = username, email = email, puntuacion = score };
    string jsonVerify = JsonUtility.ToJson(verifyData);
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
    Debug.Log("✅ Usuario verificado: " + verifyReq.downloadHandler.text);

    // 2️⃣ Enviar puntuación
    PostScoreDTO scoreData = new PostScoreDTO { api_token = api_token, name = username, email = email, puntuacion = score };
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

    // 3️⃣ Enviar rating
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

    onComplete?.Invoke();
}

}