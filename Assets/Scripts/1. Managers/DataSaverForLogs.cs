using System.Collections.Generic;

public class DataSaverForLogs
{
    private static DataSaverForLogs instance;

    public static DataSaverForLogs Instance
    {
        get
        {
            if (instance == null)
                instance = new DataSaverForLogs();

            return instance;
        }
    }

    private DataSaverForLogs() { }

    // Timers
    public float totalTime = -1f;
    public float firstKeyTime = -1f;
    public float jeraTime = -1f;
    public float othillaTime = -1f;

    // Objects
    public int takedPotions;
    public int usedPotions;

    public Dictionary<string, float> zoneTimes = new Dictionary<string, float>();

    // -------------------
    // ZONE TIMES
    // -------------------

    public void SetZoneTime(string zone, float time)
    {
        zoneTimes[zone] = time;
    }

    public float GetZoneTime(string zone)
    {
        if (zoneTimes.ContainsKey(zone))
            return zoneTimes[zone];

        return 0f;
    }

    // -------------------
    // TOTAL TIME
    // -------------------

    public void SetTotalTime(float time)
    {
        totalTime = time;
    }

    public float GetTotalTime()
    {
        return totalTime;
    }

    // -------------------
    // FIRST KEY
    // -------------------

    public void SetFirstKeyTime(float firstKeyTime)
    {
        if (this.firstKeyTime < 0f)
            this.firstKeyTime = firstKeyTime;
    }

    public float GetFirstKeyTime()
    {
        return firstKeyTime;
    }

    // -------------------
    // JERA
    // -------------------

    public void SetJeraTime(float time)
    {
        if (jeraTime <= 0f)
            jeraTime = time;
    }

    public float GetJeraTime()
    {
        return jeraTime;
    }

    // -------------------
    // OTHILLA
    // -------------------

    public void SetOthillaTime(float time)
    {
        if (othillaTime <= 0f)
            othillaTime = time;
    }

    public float GetOthillaTime()
    {
        return othillaTime;
    }

    // -------------------
    // POTIONS
    // -------------------

    public void SetTakedPotions()
    {
        takedPotions++;
    }

    public int GetTakedPotions()
    {
        return takedPotions;
    }

    public void SetUsedPotions()
    {
        usedPotions++;
    }

    public int GetUsedPotions()
    {
        return usedPotions;
    }
}