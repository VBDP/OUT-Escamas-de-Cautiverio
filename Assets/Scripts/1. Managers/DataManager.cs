using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public List<ZoneData> zones;

    public string date;
    public string time;
    public string username;
    public string email;

    public float totalTime;
    public float firstKeyTime;
    public float jeraTime;
    public float othillaTime;

    public int takedPotions;
    public int usedPotions;
    public int deaths;
    public int score;
}

[System.Serializable]
public class ZoneData
{
    public string zone;
    public float time;
}

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    private GeneralManager generalManager;
    private LifeSystem lifeSystem;

    // -------------------
    // ZONES
    // -------------------
    private Dictionary<string, float> enterTime = new Dictionary<string, float>();
    private Dictionary<string, int> zoneCounter = new Dictionary<string, int>();
    private Dictionary<string, float> zoneDurations = new Dictionary<string, float>();

    // -------------------
    // TIMERS IMPORTANTES
    // -------------------
    public float totalTime = -1f;
    public float firstKeyTime = -1f;
    public float jeraTime = -1f;
    public float othillaTime = -1f;

    // -------------------
    // STATS
    // -------------------
    public int takedPotions;
    public int usedPotions;
    public int deaths;
    public int score;

    void Awake()
    {
        lifeSystem = FindFirstObjectByType<LifeSystem>();

        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        generalManager = FindFirstObjectByType<GeneralManager>();
    }

    // -------------------
    // ZONE ENTER
    // -------------------
    public void EnterZone(string zone)
    {
        if (!zoneCounter.ContainsKey(zone))
            zoneCounter[zone] = 0;

        zoneCounter[zone]++;

        if (zoneCounter[zone] == 1)
        {
            float time = generalManager.GetTime();
            enterTime[zone] = time;

            Debug.Log("ENTER zona: " + zone);

            if (zone == "Jera" && jeraTime < 0f)
                jeraTime = time;

            if (zone == "Othilla" && othillaTime < 0f)
                othillaTime = time;

            if (zone == "FirstKey" && firstKeyTime < 0f)
                firstKeyTime = time;
        }
    }

    // -------------------
    // ZONE EXIT
    // -------------------
    public void ExitZone(string zone)
    {
        if (!zoneCounter.ContainsKey(zone))
            return;

        zoneCounter[zone]--;

        if (zoneCounter[zone] <= 0)
        {
            float exitTime = generalManager.GetTime();

            if (enterTime.ContainsKey(zone))
            {
                float duration = exitTime - enterTime[zone];

                if (!zoneDurations.ContainsKey(zone))
                    zoneDurations[zone] = 0f;

                zoneDurations[zone] += duration;

                Debug.Log($"EXIT zona: {zone} | Duración: {duration} | Total: {zoneDurations[zone]}");

                enterTime.Remove(zone);
            }

            zoneCounter[zone] = 0;
        }
    }

    // -------------------
    // GETTERS / SETTERS
    // -------------------
    public float GetZoneTime(string zone)
    {
        if (zoneDurations.ContainsKey(zone))
            return zoneDurations[zone];
        return 0f;
    }

    public float GetTotalTime() => totalTime;
    public float GetFirstKeyTime() => firstKeyTime;
    public float GetJeraTime() => jeraTime;
    public float GetOthillaTime() => othillaTime;

    public void AddPotionTaken() => takedPotions++;
    public void AddPotionUsed() => usedPotions++;

    public int GetTakedPotions() => takedPotions;
    public int GetUsedPotions() => usedPotions;

    public void SetTotalTime(float time) => totalTime = time;

    public void SetFirstKeyTime(float time)
    {
        if (firstKeyTime < 0f)
            firstKeyTime = time;
    }

    public void SetJeraTime(float time)
    {
        if (jeraTime < 0f)
            jeraTime = time;
    }

    public void SetOthillaTime(float time)
    {
        if (othillaTime < 0f)
            othillaTime = time;
    }

    // -------------------
    // SAVE JSON
    // -------------------
    public void SaveToJSON()
    {
        try
        {
            deaths = lifeSystem.GetDeaths();
            score = generalManager.GetScore();

            string folderPath = Path.Combine(Application.dataPath, "Logs");

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string fileName = "GameData_" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".json";
            string path = Path.Combine(folderPath, fileName);

            GameData data = new GameData();

            data.date = System.DateTime.Now.ToString("dd/MM/yyyy");
            data.time = System.DateTime.Now.ToString("HH:mm");
            data.username = PlayerPrefs.GetString("username");
            data.email = PlayerPrefs.GetString("email");

            data.totalTime = totalTime;
            data.firstKeyTime = firstKeyTime;
            data.jeraTime = jeraTime;
            data.othillaTime = othillaTime;

            data.takedPotions = takedPotions;
            data.usedPotions = usedPotions;
            data.deaths = deaths;
            data.score = score;

            // Orden fijo
            data.zones = new List<ZoneData>();

            List<string> order = new List<string> { "Spawn", "Zone1", "Zone2", "Zone3" };

            foreach (string zoneName in order)
            {
                if (zoneDurations.ContainsKey(zoneName))
                {
                    data.zones.Add(new ZoneData
                    {
                        zone = zoneName,
                        time = zoneDurations[zoneName]
                    });
                }
            }

            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(path, json);

            Debug.Log("✅ JSON guardado en: " + path);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("❌ Error al guardar JSON: " + ex.Message);
        }
    }
}