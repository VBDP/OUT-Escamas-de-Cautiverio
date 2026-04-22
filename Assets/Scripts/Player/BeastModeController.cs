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

        beastModeHud.color = new Color(0.55f, 0.42f, 0.08f, isActive ? 0 : 0.3f); // Change the HUD opacity based on the beast mode state

        // Si isActive es false, significa que nos estamos transformando (activando el modo bestia), y la vida debe desactivarse.
        // Si isActive es true, significa que volvemos a la normalidad (desactivando el modo bestia), y la vida debe activarse.
        if (lifeSystem != null)
        {
            lifeSystem.SetLifeSystemActive(isActive); 
        }
    }
}
