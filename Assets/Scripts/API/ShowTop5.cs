using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class Top5ScoresWithUser : MonoBehaviour
{
    [Header("TextMeshPro para top 5")]
    public TextMeshProUGUI topScoresText;

    [Header("TextMeshPro para usuario actual")]
    public TextMeshProUGUI currentUserNameText;
    public TextMeshProUGUI currentUserScoreText;
    public TextMeshProUGUI currentUserRankingText;

    private string api_token = "nL3ggwGvsiYZ5vzCqhAL58WnDcZgB9ad7FtDv82oaAAYa36UoJPS35sIbR9F";
    private string scoreUrl = "https://phpstack-1076337-5399863.cloudwaysapps.com/api/classification/";

    void Start()
    {
        // Obtener datos del usuario desde PlayerPrefs
        string username = PlayerPrefs.GetString("username", "Jugador");
        int score = PlayerPrefs.GetInt("score", 0);

        // Mostrar solo el valor sin títulos
        currentUserNameText.text = username;
        currentUserScoreText.text = score.ToString();
        currentUserRankingText.text = "-"; // Se actualizará después de obtener la clasificación

        StartCoroutine(GetTopScores(5, username));
    }

    private IEnumerator GetTopScores(int top, string currentUsername)
    {
        string url = scoreUrl + api_token + "/" + top;

        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("❌ Error al obtener clasificación: " + request.error);
            topScoresText.text = "Error cargando scores";
            currentUserRankingText.text = "-";
            yield break;
        }

        string json = request.downloadHandler.text;

        // Parsear JSON
        TopScoreResponse response = JsonUtility.FromJson<TopScoreResponse>(json);

        // Construir top 5 sin nombres repetidos
        string displayText = "";
        HashSet<string> addedNames = new HashSet<string>();
        List<ScoreEntry> data = response.data;

        int count = 0;
        foreach (var entry in data)
        {
            if (!addedNames.Contains(entry.name))
            {
                addedNames.Add(entry.name);
                count++;
                displayText += $"{entry.name} - {entry.puntuacion}\n";
            }

            if (count >= 5) break; // Solo top 5 únicos
        }

        topScoresText.text = string.IsNullOrEmpty(displayText) ? "-" : displayText.TrimEnd();

        // Buscar ranking del usuario actual en la lista completa
        int ranking = -1;
        for (int i = 0; i < data.Count; i++)
        {
            if (data[i].name == currentUsername)
            {
                ranking = i + 1; // Ranking empieza en 1
                break;
            }
        }

        currentUserRankingText.text = ranking > 0 ? ranking.ToString() : "-";
    }

    [System.Serializable]
    public class ScoreEntry
    {
        public string name;
        public int puntuacion;
    }

    [System.Serializable]
    public class TopScoreResponse
    {
        public List<ScoreEntry> data;
    }
}