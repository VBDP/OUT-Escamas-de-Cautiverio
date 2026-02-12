using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class ScoreSender
{
    private string url = "https://phpstack-1076337-5399863.cloudwaysapps.com/api/classification";

    // ✅ Token como miembro de ScoreSender
    private string api_token = "nL3ggwGvsiYZ5vzCqhAL58WnDcZgB9ad7FtDv82oaAAYa36UoJPS35sIbR9F";

    [System.Serializable]
    public class ScoreData
    {
        public string api_token;
        public string name;
        public int puntuacion;
    }

    public void SendScore(MonoBehaviour coroutineRunner, string username, int score)
    {
        coroutineRunner.StartCoroutine(PostScore(this.api_token, username, score));
    }

    private IEnumerator PostScore(string api_token, string username, int score)
    {
        ScoreData data = new ScoreData
        {
            api_token = api_token,
            name = username,
            puntuacion = score
        };

        string json = JsonUtility.ToJson(data);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

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

