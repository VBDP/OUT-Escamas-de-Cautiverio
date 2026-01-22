using UnityEngine;
using TMPro;

public class PotionHealing : MonoBehaviour
{
    private RaycastController raycast;
    private LifeSystem lifeSystem;
    private Outline outline;
    private TextMeshProUGUI interactionText;
    private Inventory inventory;
    

    void Start()
    {
        lifeSystem = FindFirstObjectByType<LifeSystem>();
        raycast = FindFirstObjectByType<RaycastController>();
        interactionText = lifeSystem.interactionText;
        inventory=FindFirstObjectByType<Inventory>();
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
            outline = raycast.GetHitObjectOutline();
            outline.OutlineColor = Color.white;
            interactionText.text = "Click para beber poción de curación";
            if (Input.GetMouseButtonDown(0))
            {
                inventory.pociones += 1;
                Destroy(gameObject);
            }

        }
        else
        {
            if (outline != null)
            {
                outline.OutlineColor = Color.clear;
            }
        }
    }
}
