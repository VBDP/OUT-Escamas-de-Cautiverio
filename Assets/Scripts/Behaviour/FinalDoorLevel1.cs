using TMPro;
using UnityEngine;

public class FinalDoorLevel1 : MonoBehaviour
{
    private RaycastController raycast;
    private Inventory inventario;

    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject winPanel; // Panel de rating
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Outline outline;
    private GeneralManager generalManager;

    private bool ratingOpened = false;

    void Start()
    {
        generalManager = FindFirstObjectByType<GeneralManager>();
        raycast = FindFirstObjectByType<RaycastController>();
        inventario = FindFirstObjectByType<Inventory>();
        text.text = "";
    }

    void Update()
    {
        string hitObject = raycast.GetHitObjectName();
        string interactionText = "";

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

                if (Input.GetMouseButtonDown(0) && !ratingOpened)
                {
                    int score = generalManager.score; // Obtener el score actual del juego
                    PlayerPrefs.SetInt("score", score); // Guardar el score actual para usarlo en el rating
                    // Abrir panel de rating
                    winPanel.SetActive(true);
                    text.text = "¡Has ganado! Califica el juego para continuar.";
                    playerMovement.BlockCamera();
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    rb.constraints = RigidbodyConstraints.FreezeAll;

                    ratingOpened = true;
                }
            }
        }

        else if (hitObject.Contains("Potion"))
        {
            interactionText = "Click para guardar, numpad 1 para curar";
        }
        else
        {
            interactionText = "";
        }

        // Actualizar UI
        text.text = interactionText;
    }
}