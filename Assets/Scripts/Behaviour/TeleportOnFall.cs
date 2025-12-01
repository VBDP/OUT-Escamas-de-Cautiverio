using UnityEngine;

public class TeleportOnFall : MonoBehaviour
{
    public GameObject teleportTarget;
    public GameObject player;   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.transform.position = teleportTarget.transform.position; // Teleport the player to the specified coordinates
            player.transform.rotation = teleportTarget.transform.rotation; // Optional: Reset the player's rotation
        }
    }
}
