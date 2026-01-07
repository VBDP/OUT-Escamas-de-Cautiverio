using UnityEngine;

public class Timer : MonoBehaviour
{
    public float TimePlayed { get; private set; }
    private bool running;

    public void StartTimer()
    {
        TimePlayed = 0f;
        running = true;
    }

    public void Tick(float deltaTime)
    {
        if (!running) return;
        TimePlayed += deltaTime;
    }

    public string GetFormattedTime()
    {
        int minutes = Mathf.FloorToInt(TimePlayed / 60f);
        int seconds = Mathf.FloorToInt(TimePlayed % 60f);
        return $"{minutes:0}:{seconds:00}";
    }

    public void SaveTime()
    {
        PlayerPrefs.SetFloat("TimePlayed", TimePlayed);
    }

    public float LoadTime()
    {
        return PlayerPrefs.GetFloat("TimePlayed", 0f);
    }
}
