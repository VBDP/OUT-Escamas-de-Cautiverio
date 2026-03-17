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

    private CanvasGroup loginCanvas;
    private PlayerMovement playerMovement;
    private bool hasRated;
    private string apiToken = "nL3ggwGvsiYZ5vzCqhAL58WnDcZgB9ad7FtDv82oaAAYa36UoJPS35sIbR9F";
    private string apiBaseUrl = "https://phpstack-1076337-5399863.cloudwaysapps.com/api";

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
        // Guardar los datos del usuario en PlayerPrefs
        string name = nameInputField.text.Trim();
        string mail = mailInputField.text.Trim();

        errorText.text = "";

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