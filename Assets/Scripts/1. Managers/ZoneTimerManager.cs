using System.Collections.Generic;
using UnityEngine;

public class ZoneTimerManager : MonoBehaviour
{
    private GeneralManager generalManager;

    private Dictionary<string, float> enterTime = new Dictionary<string, float>();
    private Dictionary<string, int> zoneCounter = new Dictionary<string, int>();

    void Start()
    {
        generalManager = FindFirstObjectByType<GeneralManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        string zone = other.tag;

        // Inicializar contador si no existe
        if (!zoneCounter.ContainsKey(zone))
            zoneCounter[zone] = 0;

        zoneCounter[zone]++;

        if (zoneCounter[zone] == 1)
        {
            enterTime[zone] = generalManager.GetTime();

            Debug.Log("ENTER zona: " + zone);

            float time = enterTime[zone];

            // Usando Singleton (recomendado)
            if (zone == "Jera")
                DataSaverForLogs.Instance.SetJeraTime(time);
            else if (zone == "Othilla")
                DataSaverForLogs.Instance.SetOthillaTime(time);
            else if (zone == "FirstKey")
                DataSaverForLogs.Instance.SetFirstKeyTime(time);
        }
    }

    void OnTriggerExit(Collider other)
    {
        string zone = other.tag;

        if (!zoneCounter.ContainsKey(zone))
            return;

        zoneCounter[zone]--;

        if (zoneCounter[zone] <= 0)
        {
            float exitTime = generalManager.GetTime();

            if (enterTime.ContainsKey(zone))
            {
                float duration = exitTime - enterTime[zone];

                Debug.Log("EXIT zona: " + zone + " | Tiempo: " + duration);
            }

            zoneCounter[zone] = 0;
        }
    }

    public float TotalLapTime(string startZone, string endZone)
    {
        if (enterTime.ContainsKey(startZone) && enterTime.ContainsKey(endZone))
        {
            return enterTime[endZone] - enterTime[startZone];
        }

        return 0f;
    }
}