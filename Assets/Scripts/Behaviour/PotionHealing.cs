using UnityEngine;
using TMPro;

public class PotionHealing : MonoBehaviour
{
    private RaycastController raycast;
    private LifeSystem lifeSystem;
    private Outline outline;
    private TextMeshProUGUI interactionText;
    private Inventory inventory;
    private GeneralManager generalManager;
    private AudioSource sfx;
    [SerializeField] private AudioClip takePotion;
    

    void Start()
    {
        lifeSystem = FindFirstObjectByType<LifeSystem>();
        raycast = FindFirstObjectByType<RaycastController>();
        interactionText = lifeSystem.interactionText;
        inventory=FindFirstObjectByType<Inventory>();
        generalManager = FindFirstObjectByType<GeneralManager>();
        sfx = generalManager.sfxSource;
        if (lifeSystem == null || raycast == null)
        {
            Debug.LogError("LifeSystem or RaycastController not found in the scene.");
            return;
        }
    }

    void Update()
    {
        if (raycast.GetHitGameObject() == gameObject)
        {
            outline = GetComponent<Outline>();
            outline.OutlineColor = Color.white;
            interactionText.text = "Click para guardar, numpad '1' para consumir";

            if (Input.GetMouseButtonDown(0))
            {
                inventory.pociones += 1;
                generalManager.ChangePotionText(inventory.pociones.ToString());
                sfx.PlayOneShot(takePotion);
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
