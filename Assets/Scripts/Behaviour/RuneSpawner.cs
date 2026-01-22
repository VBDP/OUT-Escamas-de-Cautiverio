using System.Collections.Generic;
using UnityEngine;

public class RuneSpawner : MonoBehaviour
{
    public List<Transform>  runeSpawners;
    private int randomIndex;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        randomIndex = Random.Range(0, runeSpawners.Count);
        transform.position = runeSpawners[randomIndex].position;
        transform.rotation = runeSpawners[randomIndex].rotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
