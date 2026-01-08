using UnityEngine;
using TMPro;

public class PotionHealing : MonoBehaviour
{
    private RaycastController raycast;
    private LifeSystem lifeSystem;

    void Start()
    {
        lifeSystem = FindObjectOfType<LifeSystem>();
        raycast = FindObjectOfType<RaycastController>();

        if (lifeSystem == null || raycast == null)
        {
            Debug.LogError("LifeSystem or RaycastController not found in the scene.");
            return;
        }
    }

    void Update()
    {
        if (raycast.GetHitObjectName() == "Healing Potion")
        {
            if (Input.GetMouseButtonDown(0))
            {
                lifeSystem.HealPlayer(50); // Heals the player by 50 health points
                Destroy(gameObject);
            }

        }
    }
}
