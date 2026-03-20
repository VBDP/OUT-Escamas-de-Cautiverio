using System.Collections.Generic;
using System.IO;
using UnityEngine;

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
    // Entrar en la zona 
    // -------------------

    public void EnterZone(string zone)
    {
        if (!zoneCounter.ContainsKey(zone))
            zoneCounter[zone] = 0;

        zoneCounter[zone]++;

        // Solo si es la primera vez dentro de la zona actualmente
        if (zoneCounter[zone] == 1)
        {
            float time = generalManager.GetTime();
            enterTime[zone] = time; // Guardamos la hora de entrada

            Debug.Log("ENTER zona: " + zone);

            // Eventos únicos de runas y primeras zonas
            if (zone == "Jera" && jeraTime < 0f)
                jeraTime = time;

            if (zone == "Othilla" && othillaTime < 0f)
                othillaTime = time;

            if (zone == "FirstKey" && firstKeyTime < 0f)
                firstKeyTime = time;
        }
    }

    // -------------------
    // Salir de la zona
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

                // ✅ Acumulamos el tiempo de cada visita
                zoneDurations[zone] += duration;

                Debug.Log($"EXIT zona: {zone} | Tiempo de esta visita: {duration} | Total acumulado: {zoneDurations[zone]}");

                // Limpiamos el tiempo de entrada para la próxima visita
                enterTime.Remove(zone);
            }

            zoneCounter[zone] = 0;
        }
    }

    public float TotalLapTime(string startZone, string endZone)
    {
        if (enterTime.ContainsKey(startZone) && enterTime.ContainsKey(endZone))
            return enterTime[endZone] - enterTime[startZone];

        return 0f;
    }

    // -------------------
    // ZONE TIMES (individuales)
    // -------------------
    public void SetZoneTime(string zone, float time)
    {
        zoneDurations[zone] = time;
    }

    public float GetZoneTime(string zone)
    {
        if (zoneDurations.ContainsKey(zone))
            return zoneDurations[zone];
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
    public void SetFirstKeyTime(float time)
    {
        if (firstKeyTime < 0f)
            firstKeyTime = time;
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
        if (jeraTime < 0f)
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
        if (othillaTime < 0f)
            othillaTime = time;
    }

    public float GetOthillaTime()
    {
        return othillaTime;
    }

    // -------------------
    // POTIONS
    // -------------------
    public void AddPotionTaken()
    {
        takedPotions++;
    }

    public int GetTakedPotions()
    {
        return takedPotions;
    }

    public void AddPotionUsed()
    {
        usedPotions++;
    }

    public int GetUsedPotions()
    {
        return usedPotions;
    }

    // -------------------
    // GUARDAR EN TXT
    // -------------------
public void SaveToFile()
{
    try
    {
        deaths = lifeSystem.GetDeaths();
        // Carpeta dentro de Assets
        string folderPath = Path.Combine(Application.dataPath, "Logs");

        // Crear carpeta si no existe
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
            Debug.Log("Carpeta creada: " + folderPath);
        }

        // Nombre del archivo con fecha y hora
        string fileName = "GameData_" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".txt";
        string path = Path.Combine(folderPath, fileName);

        using (StreamWriter writer = new StreamWriter(path))
        {
            writer.WriteLine("=== GAME DATA ===");

            writer.WriteLine("\n--- TIMES ---");
            writer.WriteLine("Total Time: " + totalTime);
            writer.WriteLine("First Key: " + firstKeyTime);
            writer.WriteLine("Jera: " + jeraTime);
            writer.WriteLine("Othilla: " + othillaTime);

            writer.WriteLine("\n--- ZONES ---");
            foreach (var zone in zoneDurations)
            {
                writer.WriteLine(zone.Key + ": " + zone.Value);
            }

            writer.WriteLine("\n--- STATS ---");
            writer.WriteLine("Potions Taken: " + takedPotions);
            writer.WriteLine("Potions Used: " + usedPotions);
            writer.WriteLine("Deaths: " + deaths);
        }

        Debug.Log("✅ Datos guardados correctamente en: " + path);
    }
    catch (System.Exception ex)
    {
        Debug.LogError("❌ Error al guardar el log: " + ex.Message);
    }
}
}