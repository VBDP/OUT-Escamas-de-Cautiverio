using System.Collections.Generic;
using UnityEngine;

public class ZoneTimerManager : MonoBehaviour
{
    private GeneralManager generalManager;

    // Tiempo de entrada por zona
    private Dictionary<string, float> enterTime = new Dictionary<string, float>();

    // Contador de colliders dentro de la zona
    private Dictionary<string, int> zoneCounter = new Dictionary<string, int>();

    void Start()
    {
        generalManager = FindFirstObjectByType<GeneralManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        string zone = other.tag;

        if (!zoneCounter.ContainsKey(zone))
            zoneCounter[zone] = 0;

        zoneCounter[zone]++;

        // Primera vez que entras a la zona
        if (zoneCounter[zone] == 1)
        {
            enterTime[zone] = generalManager.GetTime();
            Debug.Log("ENTER zona: " + zone);
        }
    }

    void OnTriggerExit(Collider other)
    {
        string zone = other.tag;

        if (!zoneCounter.ContainsKey(zone))
            return;

        zoneCounter[zone]--;

        // Sales completamente de la zona
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

    // Tiempo total de una vuelta
    public float TotalLapTime(string startZone, string endZone)
    {
        if (enterTime.ContainsKey(startZone) && enterTime.ContainsKey(endZone))
        {
            return enterTime[endZone] - enterTime[startZone];
        }

        return 0f;
    }
}