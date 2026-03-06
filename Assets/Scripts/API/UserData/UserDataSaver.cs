using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Text.RegularExpressions;
using System.Collections;
public class UserDataSaver : MonoBehaviour
{

    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private TMP_InputField mailInputField;
    [SerializeField] private GameObject LoginPanel;
    [SerializeField] private GameObject HudPanel;
    [SerializeField] private TMP_Text errorText;
     private PlayerMovement playerMovement;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        errorText.text = "";
        playerMovement = FindFirstObjectByType<PlayerMovement>();
        LoginPanel.SetActive(true);
    }

public void saveData()
{
    string name = nameInputField.text.Trim();
    string mail = mailInputField.text.Trim();

    // Reset error
    errorText.text = "";

    // ---- VALIDAR NOMBRE ----
    if (string.IsNullOrEmpty(name))
    {
        errorText.text = "Name cannot be empty.";
        return;
    }

    if (name.Length > 10)
    {
        errorText.text = "Name cannot be longer than 10 characters.";
        return;
    }

    // ---- VALIDAR EMAIL ----
    if (string.IsNullOrEmpty(mail))
    {
        errorText.text = "Email cannot be empty.";
        return;
    }

    // regex simple para email
    if (!Regex.IsMatch(mail, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
    {
        errorText.text = "Invalid email format.";
        return;
    }

    // ---- GUARDAR DATOS ----
    PlayerPrefs.SetString("name", name);
    PlayerPrefs.SetString("mail", mail);

    Debug.Log("Data saved: " + name + ", " + mail);
    StartCoroutine(ShowTemporaryMessage("Data saved", 3f));
}

IEnumerator ShowTemporaryMessage(string message, float time)
{
    errorText.text = message;
    yield return new WaitForSeconds(time);
    errorText.text = "";
    LoginPanel.SetActive(false);
    HudPanel.SetActive(true);

    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;

    playerMovement.UnblockCamera();
}
}
