using UnityEngine;
using TMPro;

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

    private string currentUsername;
    private int currentScore;

    void Start()
    {
        currentUsername = PlayerPrefs.GetString("username", "Jugador");
        currentScore = PlayerPrefs.GetInt("score", 0);

        currentUserNameText.text = currentUsername;
        currentUserScoreText.text = currentScore.ToString();
        currentUserRankingText.text = "-";
        
        topScoresText.text = "Clasificación Desactivada";
    }
}