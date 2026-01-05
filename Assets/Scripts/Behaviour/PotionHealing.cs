using UnityEngine;
using TMPro;

public class PotionHealing : MonoBehaviour
{
    [SerializeField] private RaycastController Raycast;
    [SerializeField] private LifeSystem lifeSystem;

    void Update()
    {
        if (Raycast.GetHitObjectName() == "Healing Potion")
        {
            if (Input.GetMouseButtonDown(0))
            {
                lifeSystem.HealPlayer(50); // Heals the player by 50 health points
                Destroy(gameObject);
            }
            
        }
    }
}
