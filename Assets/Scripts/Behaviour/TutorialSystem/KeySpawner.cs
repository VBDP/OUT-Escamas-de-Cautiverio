using UnityEngine;
using System.Collections.Generic;

public class KeySpawner : MonoBehaviour
{
    public GameObject keyPrefab;
    public List<Transform> spawnPoints;

    void Start()
    {
        // Instanciamos la llave en un punto aleatorio
        GameObject key = Instantiate(
            keyPrefab,
            spawnPoints[Random.Range(0, spawnPoints.Count)].position,
            Quaternion.identity
        );

        // Buscamos cualquier Outline en el objeto o sus hijos
        Outline outline = key.GetComponentInChildren<Outline>();
        if (outline != null)
        {
            // Cargamos normales suavizadas de forma segura
            // Esto evita NullReferenceException si MeshFilter está en hijo
            outline.LoadSmoothNormals();
        }
    }
}