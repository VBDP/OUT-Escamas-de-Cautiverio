using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UserData
{
    public string username;
    public string fecha;
    public string hora;

    // ⏱️ Tiempos (en segundos)
    public float tiempoTotal;
    public float tiempoPrimeraLlave;
    public float tiempoZona1;
    public float tiempoZona2;

    // 🪄 Runas (tiempo en coger cada una)
    public float[] tiemposRunas;

    // 🧪 Stats
    public int pocionesAgarradas;
    public int pocionesUsadas;
    public int muertes;

    // ⚠️ Trampas que mataron al jugador con conteo
    public List<TrampaMuerte> trampasMuertes;
}

[Serializable]
public class TrampaMuerte
{
    public string nombreTrampa;
    public int cantidad;

    public TrampaMuerte(string nombre, int cant)
    {
        nombreTrampa = nombre;
        cantidad = cant;
    }
}

public class SaveToJson 
{
    public string username = "Player";

    // Variables de ejemplo
    public float tiempoTotal;
    public float tiempoPrimeraLlave;
    public float tiempoZona1;
    public float tiempoZona2;

    public float[] tiemposRunas;

    public int pocionesAgarradas;
    public int pocionesUsadas;
    public int muertes;

    // Diccionario interno para contar muertes por trampa
    private Dictionary<string, int> muertesPorTrampa = new Dictionary<string, int>();

    // Registrar muerte por trampa
    public void RegistrarMuertePorTrampa(string nombreTrampa)
    {
        if (muertesPorTrampa.ContainsKey(nombreTrampa))
        {
            muertesPorTrampa[nombreTrampa]++;
        }
        else
        {
            muertesPorTrampa[nombreTrampa] = 1;
        }

        muertes++;
        Debug.Log($"Jugador muerto por {nombreTrampa}. Total: {muertesPorTrampa[nombreTrampa]}");
    }

    public void GuardarDatos()
    {
        UserData data = new UserData();

        // Datos básicos
        data.username = username;
        data.fecha = DateTime.Now.ToString("yyyy-MM-dd");
        data.hora = DateTime.Now.ToString("HH:mm:ss");

        // Tiempos
        data.tiempoTotal = tiempoTotal;
        data.tiempoPrimeraLlave = tiempoPrimeraLlave;
        data.tiempoZona1 = tiempoZona1;
        data.tiempoZona2 = tiempoZona2;

        // Runas
        data.tiemposRunas = tiemposRunas;

        // Stats
        data.pocionesAgarradas = pocionesAgarradas;
        data.pocionesUsadas = pocionesUsadas;
        data.muertes = muertes;

        // Convertir diccionario a lista serializable
        data.trampasMuertes = new List<TrampaMuerte>();
        foreach (var kvp in muertesPorTrampa)
        {
            data.trampasMuertes.Add(new TrampaMuerte(kvp.Key, kvp.Value));
        }

        // Convertir a JSON
        string json = JsonUtility.ToJson(data, true);

        string path = Path.Combine(Application.persistentDataPath, "userdata.json");

        File.WriteAllText(path, json);

        Debug.Log("Datos guardados en: " + path);
        Debug.Log(json);
    }
}