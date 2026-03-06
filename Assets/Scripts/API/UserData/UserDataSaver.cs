using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
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

    private CanvasGroup loginCanvas;
    private PlayerMovement playerMovement;

    void Start()
    {
#if UNITY_EDITOR
        PlayerPrefs.DeleteAll();
#endif

        nameInputField.characterLimit = 10;

        loginCanvas = LoginPanel.GetComponent<CanvasGroup>();

        errorText.text = "";
        playerMovement = FindFirstObjectByType<PlayerMovement>();

        nameInputField.onValueChanged.AddListener(delegate { ValidateInputs(); });
        mailInputField.onValueChanged.AddListener(delegate { ValidateInputs(); });

        saveButton.interactable = false;

        if (PlayerPrefs.HasKey("name") && PlayerPrefs.HasKey("mail"))
        {
            nameInputField.text = PlayerPrefs.GetString("name");
            mailInputField.text = PlayerPrefs.GetString("mail");

            LoginPanel.SetActive(false);
            HudPanel.SetActive(true);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            playerMovement.UnblockCamera();
        }
        else
        {
            LoginPanel.SetActive(true);
            HudPanel.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && saveButton.interactable)
        {
            saveData();
        }
    }

    void ValidateInputs()
    {
        string name = nameInputField.text.Trim();
        string mail = mailInputField.text.Trim();

        bool nameValid =
            !string.IsNullOrEmpty(name) &&
            name.Length <= 10 &&
            Regex.IsMatch(name, @"^[a-zA-Z0-9]{1,10}$");

        bool mailValid =
            !string.IsNullOrEmpty(mail) &&
            Regex.IsMatch(mail, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");

        saveButton.interactable = nameValid && mailValid;
    }

    public void saveData()
    {
        string name = nameInputField.text.Trim();
        string mail = mailInputField.text.Trim();

        errorText.text = "";

        PlayerPrefs.SetString("name", name);
        PlayerPrefs.SetString("mail", mail);
        PlayerPrefs.Save();

        Debug.Log("Data saved: " + name + ", " + mail);

        StopAllCoroutines();
        StartCoroutine(LoginSuccess());
    }

IEnumerator LoginSuccess()
{
    errorText.text = "Data saved";

    yield return new WaitForSeconds(1.5f);

    yield return StartCoroutine(FadeOutLogin());

    // Forzar captura del cursor
    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;

    // Resetea input para evitar que Unity espere clicks
    Input.ResetInputAxes();

    // Limpiar selección de UI si hay
    UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);

    playerMovement.UnblockCamera();
}

    IEnumerator FadeOutLogin()
    {
        float duration = 1f;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            loginCanvas.alpha = 1 - (time / duration);
            yield return null;
        }

        loginCanvas.alpha = 0;
        LoginPanel.SetActive(false);
        HudPanel.SetActive(true);

        // Aquí también puedes asegurar que el cursor esté bloqueado
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }
}