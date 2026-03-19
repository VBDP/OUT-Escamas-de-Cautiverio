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
        inventory = FindFirstObjectByType<Inventory>();
        generalManager = FindFirstObjectByType<GeneralManager>();

        interactionText = lifeSystem.interactionText;
        sfx = generalManager.sfxSource;

        outline = GetComponent<Outline>();

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
            if (outline != null)
                outline.OutlineColor = Color.white;

            interactionText.text = "Click para guardar, numpad '1' para consumir";

            if (Input.GetMouseButtonDown(0))
            {
                inventory.pociones += 1;

                // ✅ USO CORRECTO DEL DATASAVER
                DataSaverForLogs.Instance.SetTakedPotions();

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