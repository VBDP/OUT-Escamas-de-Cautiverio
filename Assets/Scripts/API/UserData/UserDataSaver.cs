using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

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
    private bool hasRated;
    private string apiToken = "nL3ggwGvsiYZ5vzCqhAL58WnDcZgB9ad7FtDv82oaAAYa36UoJPS35sIbR9F";
    private string apiBaseUrl = "https://phpstack-1076337-5399863.cloudwaysapps.com/api";

    void Start()
    {
        PlayerPrefs.DeleteAll();
        nameInputField.characterLimit = 10;
        loginCanvas = LoginPanel.GetComponent<CanvasGroup>();
        errorText.text = "";
        errorText.color = Color.red;
        playerMovement = FindFirstObjectByType<PlayerMovement>();

        nameInputField.onValueChanged.AddListener(delegate { ValidateInputs(); });
        mailInputField.onValueChanged.AddListener(delegate { ValidateInputs(); });

        saveButton.interactable = false;

        if (PlayerPrefs.HasKey("username") && PlayerPrefs.HasKey("email"))
        {
            nameInputField.text = PlayerPrefs.GetString("username");
            mailInputField.text = PlayerPrefs.GetString("email");

            LoginPanel.SetActive(false);
            HudPanel.SetActive(true);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            playerMovement.UnblockCamera();

            // Verificar si ya ha puntuado
            StartCoroutine(VerifyUser(nameInputField.text.Trim(), mailInputField.text.Trim()));
        }
        else
        {
            LoginPanel.SetActive(true);
            HudPanel.SetActive(false);
        }
    }

    void Update()
    {
        // Navegación con Tabulador entre inputs
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (nameInputField.isFocused)
            {
                mailInputField.ActivateInputField();
            }
            else if (mailInputField.isFocused)
            {
                nameInputField.ActivateInputField();
            }
            else
            {
                // Si ninguno está enfocado, enfoca el primero
                nameInputField.ActivateInputField();
            }
        }

        if (Input.GetKeyDown(KeyCode.Return) && saveButton.interactable)
        {
            saveData();
        }
    }

    void ValidateInputs()
    {
        string name = nameInputField.text.Trim();
        string mail = mailInputField.text.Trim();

        errorText.color = Color.red;

        // Validar Nombre
        if (string.IsNullOrEmpty(name))
        {
            errorText.text = "Introduce un nombre";
            saveButton.interactable = false;
            return;
        }

        if (name.Length < 3)
        {
            errorText.text = "Nombre muy corto (mín. 3 caracteres)";
            saveButton.interactable = false;
            return;
        }

        if (!Regex.IsMatch(name, @"^[a-zA-Z0-9_]{3,10}$"))
        {
            errorText.text = "Nombre no válido (solo letras, números y _)";
            saveButton.interactable = false;
            return;
        }

        // Validar Email
        if (string.IsNullOrEmpty(mail))
        {
            errorText.text = "Introduce un email";
            saveButton.interactable = false;
            return;
        }

        if (!Regex.IsMatch(mail, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            errorText.text = "Formato de email no válido";
            saveButton.interactable = false;
            return;
        }

        // Todo correcto
        errorText.text = "Datos correctos";
        errorText.color = Color.green;
        saveButton.interactable = true;
    }

    public void saveData()
    {
        // Guardar los datos del usuario en PlayerPrefs
        string name = nameInputField.text.Trim();
        string mail = mailInputField.text.Trim();

        errorText.color = Color.green;
        PlayerPrefs.SetString("username", name);
        PlayerPrefs.SetString("email", mail);
        PlayerPrefs.Save();

        Debug.Log("Data saved: " + name + ", " + mail);

        StopAllCoroutines();
        StartCoroutine(LoginSuccess());

        // Tomar los datos directamente de PlayerPrefs para la verificación
        string savedName = PlayerPrefs.GetString("username");
        string savedEmail = PlayerPrefs.GetString("email");

        // Verificar usuario en el servidor usando los datos guardados
        StartCoroutine(VerifyUser(savedName, savedEmail));
    }

    IEnumerator LoginSuccess()
    {
        errorText.text = "Data saved";
        yield return new WaitForSeconds(1.5f);
        yield return StartCoroutine(FadeOutLogin());

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Input.ResetInputAxes();
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

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    IEnumerator VerifyUser(string username, string email)
    {
        string url = apiBaseUrl + "/verify";

        // Crear el JSON a enviar
        string jsonData = JsonUtility.ToJson(new VerifyRequest
        {
            api_token = apiToken,
            name = username,
            email = email
        });

        using (UnityWebRequest www = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error verifying user: " + www.error);
                hasRated = false;
            }
            else
            {
                string jsonResponse = www.downloadHandler.text;
                VerifyResponse response = JsonUtility.FromJson<VerifyResponse>(jsonResponse);
                hasRated = response.rated;
                Debug.Log("User hasRated: " + hasRated);

                winText.text = hasRated ? "You already rated the game, go to Main Menu" : "You win, Rate the game now";
                submitText.text = hasRated? "Return to Main Menu" : "Rate the game now";
            }
        }
    }

    [System.Serializable]
    public class VerifyRequest
    {
        public string api_token;
        public string name;
        public string email;
    }

    [System.Serializable]
    public class Criterion
    {
        public string name;
        public int min_score;
        public int max_score;
    }

    [System.Serializable]
    public class VerifyResponse
    {
        public bool rated;
        public Criterion[] criterion;
    }

    public bool HasRated()
{
    return hasRated;
}

    void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }
}