using UnityEngine;

[CreateAssetMenu(fileName = "NetworkingData", menuName = "Scriptable Objects/NetworkingData")]
public class NetworkingData : ScriptableObject
{
    public string playerName;
    public int score;
}