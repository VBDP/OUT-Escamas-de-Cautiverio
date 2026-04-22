using UnityEngine;
using UnityEngine.UI;

public class BeastModeController : MonoBehaviour
{
    public GameObject beastModeEffect; // Reference to the beast mode effect GameObject
    public Image beastModeHud; // Reference to the UI icon for beast mode
    private LifeSystem lifeSystem;
    private GeneralManager generalManager;
    private float beastModeTimer = 0f;
    private float beastModeInterval = 10f;
    
    [Header("HUD Animation Settings")]
    [SerializeField] private float pulseSpeed = 2f;
    [SerializeField] private Color colorGold = new Color(0.55f, 0.42f, 0.08f, 0.2f); // Dorado base
    [SerializeField] private Color colorRed = new Color(0.5f, 0f, 0f, 0.2f);      // Rojo fuego
    [SerializeField] private float flickerSpeed = 15f;

    void Start()
    {
        lifeSystem = FindFirstObjectByType<LifeSystem>(); // Get the LifeSystem component attached to the player
        generalManager = FindFirstObjectByType<GeneralManager>(); // Referencia al GeneralManager
        beastModeEffect.SetActive(false); // Ensure the beast mode effect is initially inactive
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B)) // Check if the 'B' key is pressed to toggle beast mode
        {
            // Si ya está activo, no permitimos desactivarlo
            if (beastModeEffect.activeSelf) return;

            // Solo permite activarlo si el GeneralManager y LoginPanel existen, y el panel está desactivado.
            if (generalManager != null && generalManager.LoginPanel != null && generalManager.LoginPanel.activeSelf)
            {
                return; // Evita la activación si el login o menú está activo
            }

            ToggleBeastMode();
        }

        if (beastModeEffect.activeSelf)
        {
            beastModeTimer += Time.deltaTime;
            if (beastModeTimer >= beastModeInterval)
            {
                if (generalManager != null)
                {
                    generalManager.DecreaseScore(100);
                    generalManager.EnableDecreaseText(100);
                    generalManager.DisableDecreaseTextDelayed(0.8f);
                }
                beastModeTimer = 0f;
            }

            // Lógica de animación del HUD (Fuego/Energía)
            float pulse = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f;
            float flicker = Mathf.Lerp(0.7f, 1.3f, Mathf.PingPong(Time.time * flickerSpeed, 1f));
            
            Color currentEffectColor = Color.Lerp(colorGold, colorRed, pulse);
            currentEffectColor.a *= flicker; // Aplicar parpadeo a la transparencia
            beastModeHud.color = currentEffectColor;
        }
        else
        {
            // Asegurarse de que el HUD esté invisible si el modo está apagado
            if (beastModeHud.color.a > 0)
            {
                beastModeHud.color = new Color(0, 0, 0, 0);
            }
        }
    }

    void ToggleBeastMode()
    {
        bool isActive = beastModeEffect.activeSelf; // Check the current state of the beast mode effect
        beastModeEffect.SetActive(!isActive); // Toggle the active state of the beast mode effect
        if (!isActive)
        {
            beastModeTimer = 0f; // Reiniciar el temporizador al activarlo
        }

        // El color se gestionará dinámicamente en el Update una vez activo, 
        // pero establecemos un estado inicial aquí si se desea.
        if (!isActive) 
        {
            beastModeHud.color = colorGold;
        }

        // Si isActive es false, significa que nos estamos transformando (activando el modo bestia), y la vida debe desactivarse.
        // Si isActive es true, significa que volvemos a la normalidad (desactivando el modo bestia), y la vida debe activarse.
        if (lifeSystem != null)
        {
            lifeSystem.SetLifeSystemActive(isActive); 
        }
    }
}
