using UnityEngine;

public class TeleportOnFall : MonoBehaviour
{
    public GameObject teleportTarget;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = teleportTarget.transform.position; // Teleport the player to the specified coordinates
            other.transform.rotation = teleportTarget.transform.rotation; // Optional: Reset the player's rotation
        }
    }
}
