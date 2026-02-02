using TMPro;
using UnityEngine;

public class FinalDoorLevel1 : MonoBehaviour
{
    private RaycastController raycast;
    private Inventory inventario;
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] private GameObject finalDoorLevel1;
    [SerializeField] private Rigidbody rb;

    [SerializeField] private GameObject winPanel;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GeneralManager generalManager;
    [SerializeField] private Outline outline;

    private string hitObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        raycast = FindFirstObjectByType<RaycastController>();
        inventario = FindFirstObjectByType<Inventory>();
        text.text = "";
        hitObject = raycast.GetHitObjectName();
    }

    // Update is called once per frame
    void Update()
    {
        string hitObject = raycast.GetHitObjectName();

        // Reset del texto por defecto
        string interactionText = "";

        // PUERTA FINAL
        if (hitObject == "FinalDoorLevel1")
        {
            if (!inventario.jeraPlaced || !inventario.othillaPlaced)
            {
                interactionText = "Debes colocar las dos runas en las paredes de los lados para avanzar";
            }
            else
            {
                interactionText = "Haz click para abrir la puerta";
                outline.OutlineColor = Color.white;

                if (Input.GetMouseButtonDown(0))
                {
                    winPanel.SetActive(true);
                    text.text = "Has ganado y has obtenido " + generalManager.score + " puntos.";
                    playerMovement.BlockCamera();
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    rb.constraints = RigidbodyConstraints.FreezeAll;
                }
            }
        }

        // POCIÓN
        else if (hitObject.Contains("Potion")) // o el nombre exacto de la poción
        {
            interactionText = "Click para guardar, numpad 1 para curar";
        }

        // OTROS OBJETOS → no interacción
        else
        {
            interactionText = "";
        }

        // Finalmente, actualiza el texto de la UI
        generalManager.SetInteractionText(interactionText);
    }

}