using TMPro;
using UnityEngine;

public class FinalDoorLevel1 : MonoBehaviour
{
    private RaycastController raycast;
    private Inventory inventario;

    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject winPanel;

    [SerializeField] private TextMeshProUGUI interactionText;

    private Outline outline;
    private GeneralManager generalManager;

    private bool ratingOpened = false;

    void Start()
    {
        generalManager = FindFirstObjectByType<GeneralManager>();
        raycast = FindFirstObjectByType<RaycastController>();
        inventario = FindFirstObjectByType<Inventory>();
        outline = GetComponent<Outline>();

        interactionText.text = "";
    }

    void Update()
    {
        string hitObject = raycast.GetHitObjectName();
        string interactionMessage = "";

        if (hitObject == "FinalDoorLevel1")
        {
            if (!inventario.jeraPlaced || !inventario.othillaPlaced)
            {
                interactionMessage = "Debes colocar las dos runas en las paredes de los lados para avanzar";
            }
            else
            {
                interactionMessage = "Haz click para abrir la puerta";
                outline.OutlineColor = Color.white;

                if (Input.GetMouseButtonDown(0) && !ratingOpened)
                {
                    int score = generalManager.score;
                    PlayerPrefs.SetInt("score", score);

                    // -------------------
                    // Guardamos el totalTime en DataManager
                    // -------------------
                    DataManager.Instance.SetTotalTime(generalManager.GetTime());

                    // -------------------
                    // Guardamos TODO el DataManager en archivo
                    // -------------------
                    DataManager.Instance.SaveToFile();

                    Debug.Log("✅ Datos guardados y panel de victoria abierto.");
                    Debug.Log("Has tardado " + DataManager.Instance.GetTotalTime() + " segundos en total.");

                    // Abrimos panel de victoria
                    winPanel.SetActive(true);

                    playerMovement.BlockCamera();

                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;

                    rb.constraints = RigidbodyConstraints.FreezeAll;

                    ratingOpened = true;
                }
            }
        }
        else if (!string.IsNullOrEmpty(hitObject) && hitObject.Contains("Potion"))
        {
            interactionMessage = "Click para guardar, numpad 1 para curar";
        }

        if (!ratingOpened)
        {
            interactionText.text = interactionMessage;
        }
    }
}