using TMPro;
using UnityEngine;

public class FinalDoorLevel1 : MonoBehaviour
{
    private RaycastController raycast;
    private Inventory inventario;
    private int score;
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] private GameObject finalDoorLevel1;
    [SerializeField] private Rigidbody rb;

    [SerializeField] private GameObject winPanel;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GeneralManager generalManager;
    [SerializeField] private Outline outline;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        raycast = FindFirstObjectByType<RaycastController>();
        inventario = FindFirstObjectByType<Inventory>();
        text.text = "";
        score = generalManager.score;
    }

    // Update is called once per frame
    void Update()
    {
        if (inventario.jeraPlaced && inventario.othillaPlaced)
        {
            generalManager.SetInteractionText("Haz click para abrir la puerta");
            outline.OutlineColor = Color.white;
        }
        else
        {
            generalManager.SetInteractionText("Necesitas colocar las 2 runas para abrir esta puerta");
        }
        
        if (raycast.GetHitObjectName() == "FinalDoorLevel1")
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (inventario.othillaPlaced == true && inventario.jeraPlaced == true)
                {
                    winPanel.SetActive(true);
                    text.text = "Has ganado y has obtenido"+ score + " puntos.";
                    playerMovement.BlockCamera();
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    rb.constraints = RigidbodyConstraints.FreezeAll;
                    
                }
            }
        }
    }
}