using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class ScoreSender
{
    private string url = "https://phpstack-1076337-5399863.cloudwaysapps.com/api/classification";
    private string api_token = "nL3ggwGvsiYZ5vzCqhAL58WnDcZgB9ad7FtDv82oaAAYa36UoJPS35sIbR9F";

    public void SendScore(MonoBehaviour coroutineRunner, string username, string email, int scoreValue)
    {
        coroutineRunner.StartCoroutine(PostScore(username, email, scoreValue));
    }

    private IEnumerator PostScore(string username, string email, int scoreValue)
    {
        // Crear DTO con los datos que la API espera
        PostScoreDTO postData = new PostScoreDTO
        {
            api_token = api_token,
            name = username,
            email = email,
            puntuacion = scoreValue
        };

        // Convertir a JSON
        string json = JsonUtility.ToJson(postData);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        Debug.Log("JSON enviado: " + json);

        // Crear la request POST
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Accept", "application/json");

        // Esperar la respuesta
        yield return request.SendWebRequest();

        // Comprobar resultado
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("✅ Score enviado correctamente");
            Debug.Log(request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("❌ Error: " + request.error);
        }
    }
}