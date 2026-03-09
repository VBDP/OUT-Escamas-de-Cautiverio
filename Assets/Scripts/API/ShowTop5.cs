using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Linq; // Para ordenar la lista

public class Top5ScoresWithUserAuto : MonoBehaviour
{
    [Header("TextMeshPro para top 5")]
    public TextMeshProUGUI topScoresText;

    [Header("TextMeshPro para usuario actual")]
    public TextMeshProUGUI currentUserNameText;
    public TextMeshProUGUI currentUserScoreText;
    public TextMeshProUGUI currentUserRankingText;

    [Header("Actualización automática")]
    public float refreshInterval = 10f;

    private string api_token = "nL3ggwGvsiYZ5vzCqhAL58WnDcZgB9ad7FtDv82oaAAYa36UoJPS35sIbR9F";
    private string scoreUrl = "https://phpstack-1076337-5399863.cloudwaysapps.com/api/classification/";

    private string currentUsername;
    private int currentScore;

    void Start()
    {
        currentUsername = PlayerPrefs.GetString("username", "Jugador");
        currentScore = PlayerPrefs.GetInt("score", 0);

        currentUserNameText.text = currentUsername;
        currentUserScoreText.text = currentScore.ToString();
        currentUserRankingText.text = "-";

        StartCoroutine(AutoRefresh());
    }

    private IEnumerator AutoRefresh()
    {
        while (true)
        {
            yield return GetTopScoresAndRanking();
            yield return new WaitForSeconds(refreshInterval);
        }
    }

    private IEnumerator GetTopScoresAndRanking()
    {
        string url = scoreUrl + api_token; // obtener todos los jugadores
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("❌ Error al obtener clasificación: " + request.error);
            topScoresText.text = "-";
            currentUserRankingText.text = "-";
            yield break;
        }

        string json = request.downloadHandler.text;
        TopScoreResponse response = JsonUtility.FromJson<TopScoreResponse>(json);
        List<ScoreEntry> data = response.data;

        // Ordenar por puntuación descendente
        data = data.OrderByDescending(e => e.puntuacion).ToList();

        // Construir top 5 únicos
        string top5Text = "";
        HashSet<string> addedNames = new HashSet<string>();
        int count = 0;
        foreach (var entry in data)
        {
            if (!addedNames.Contains(entry.name))
            {
                addedNames.Add(entry.name);
                count++;
                if (count <= 5)
                    top5Text += $"{entry.name} - {entry.puntuacion}\n";
            }
        }
        topScoresText.text = string.IsNullOrEmpty(top5Text) ? "-" : top5Text.TrimEnd();

        // Calcular ranking real considerando empates
        int ranking = 1;
        int prevScore = -1;
        int skipped = 0; // para manejar empates
        bool found = false;

        foreach (var entry in data)
        {
            if (prevScore != -1 && entry.puntuacion < prevScore)
            {
                ranking += skipped + 1;
                skipped = 0;
            }
            else if (prevScore != -1 && entry.puntuacion == prevScore)
            {
                skipped++;
            }

            if (entry.name == currentUsername)
            {
                found = true;
                break;
            }

            prevScore = entry.puntuacion;
        }

        currentUserRankingText.text = found ? ranking.ToString() : "-";
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