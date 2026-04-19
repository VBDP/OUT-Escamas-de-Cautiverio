using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class NPCBasicScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI npcTextBox; // La caja de texto del diálogo (HUD inferior)
    [SerializeField] private RaycastController raycast;
    [SerializeField] private Outline outline;
    [SerializeField] private List<string> frases;
    private int count = 0;

    private GeneralManager generalManager;
    private float textTimer = 0f;
    private bool isShowingText = false;

    void Start()
    {
        // Asignación automática de dependencias para Raycast y Outline
        generalManager = FindFirstObjectByType<GeneralManager>();
        
        if (raycast == null) 
            raycast = FindFirstObjectByType<RaycastController>();
            
        if (outline == null) 
            outline = GetComponent<Outline>();
            
        if (npcTextBox != null)
        {
            npcTextBox.text = "Click para hablar"; // Asegurarse de que empieza vacío
        }
    }

    void Update()
    {
        if (outline != null)
        {
            outline.OutlineColor = new Color(0, 0, 0, 0); // Apaga el outline por defecto
        }

        // Si el raycast detecta a este NPC (por nombre)
        if (raycast != null && raycast.GetHitObjectName() == "NPC")
        {
            if (outline != null) outline.OutlineColor = Color.white;

            // Forzamos mostrar el texto siempre que le miremos (sin importar isShowingText)
            if (generalManager != null)
            {
                generalManager.SetInteractionText("Click para hablar con NPC");
            }

            // Al hacer click, avanzamos el diálogo en el text box grande (npcTextBox)
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log($"Click detectado. frases.Count={frases.Count}, npcTextBox asignado={(npcTextBox != null)}");

                if (frases.Count > 0 && npcTextBox != null)
                {
                    Debug.Log("Poniendo la frase: " + frases[count]);
                    npcTextBox.text = frases[count];
                    isShowingText = true;
                    textTimer = 4f; // Tiempo que la frase durará en pantalla

                    // Pasar a la siguiente línea
                    if (count < frases.Count - 1)
                    {
                        count++;
                    }
                    else
                    {
                        count = 0; // O reiniciar si llega al final
                    }
                }
            }
        }

        // Sistema de auto-limpieza del díalogo (npcTextBox)
        if (isShowingText)
        {
            textTimer -= Time.deltaTime;
            if (textTimer <= 0)
            {
                isShowingText = false;
                if (npcTextBox != null)
                {
                    npcTextBox.text = ""; // Limpia la frase
                }
            }
        }
    }
}
