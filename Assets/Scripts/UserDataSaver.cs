using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class UserDataSaver : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private TMP_InputField mailInputField;
    [SerializeField] private GameObject LoginPanel;
    [SerializeField] private GameObject HudPanel;
    [SerializeField] private TMP_Text errorText;
    [SerializeField] private Button saveButton;
    [SerializeField] private TextMeshProUGUI winText;
    [SerializeField] private TextMeshProUGUI submitText;

    private CanvasGroup loginCanvas;
    private PlayerMovement playerMovement;
    private GeneralManager generalManager;

    void Start()
    {
        // Bypass login completely
        LoginPanel.SetActive(false);
        HudPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerMovement = FindFirstObjectByType<PlayerMovement>();
        generalManager = FindFirstObjectByType<GeneralManager>();

        if (playerMovement != null)
        {
            playerMovement.UnblockCamera();
        }

        if (generalManager != null)
        {
            generalManager.EndLoginUI();
        }
    }

    void Update()
    {
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }
}