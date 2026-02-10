using UnityEngine;

public class Timer : MonoBehaviour
{
    public float timePlayed { get; private set; }
    private bool running;

    public void StartTimer()
    {
        timePlayed = 0f;
        running = true;
    }

    public void Tick(float deltaTime)
    {
        if (!running) return;
        timePlayed += deltaTime;
    }

    public string GetFormattedTime()
    {
        int minutes = Mathf.FloorToInt(timePlayed / 60f);
        int seconds = Mathf.FloorToInt(timePlayed % 60f);
        return $"{minutes:0}:{seconds:00}";
    }

    public void SaveTime()
    {
        PlayerPrefs.SetFloat("TimePlayed", timePlayed);
    }

    public float LoadTime()
    {
        return PlayerPrefs.GetFloat("TimePlayed", 0f);
    }
}
