using UnityEngine;
using System.Collections.Generic;

public class KeySpawner : MonoBehaviour
{
    public GameObject keyPrefab;
    public List<Transform> spawnPoints;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instantiate(keyPrefab, spawnPoints[Random.Range(0, spawnPoints.Count)].position, Quaternion.identity);
    }
}
