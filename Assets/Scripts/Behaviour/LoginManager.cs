using System;
using System.Net.Mail;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DisableLoginPanel : MonoBehaviour
{
    private bool ok;
    private PlayerMovement playerMovement;

    [SerializeField] private GameObject loginPanel;
    [SerializeField] private GameObject hudPanel;

    [SerializeField] private TMP_InputField userInput;
    [SerializeField] private TMP_InputField emailInput;

    private string username;
    private string email;

    private void Start()
    {
        playerMovement = FindFirstObjectByType<PlayerMovement>();

        // Asegurarse de que el primer input esté seleccionado al iniciar
        userInput.Select();

        // Añadir listeners para Enter
        userInput.onSubmit.AddListener(delegate { Submit(); });
        emailInput.onSubmit.AddListener(delegate { Submit(); });
    }

    private void Update()
    {
        if (!ok)
        {
            playerMovement.BlockCamera();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // Detectar Tab para cambiar de input
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (userInput.isFocused)
            {
                emailInput.Select();
            }
            else if (emailInput.isFocused)
            {
                userInput.Select();
            }
        }
    }

    public void Submit()
    {
        // Obtener texto de los inputs TMPRO
        username = userInput.text.Trim();
        email = emailInput.text.Trim();

        if (string.IsNullOrEmpty(username))
        {
            Debug.Log("Por favor, escribe un username");
            userInput.Select(); // volver a seleccionar
            return;
        }

        if (!IsValidEmail(email))
        {
            Debug.Log("Email inválido");
            emailInput.Select(); // volver a seleccionar
            return;
        }

        ok = true;

        loginPanel.SetActive(false);
        hudPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerMovement.UnblockCamera();

        Debug.Log("Username: " + username);
        Debug.Log("Email: " + email);
    }

    // Función para validar el email
    private bool IsValidEmail(string email)
    {
        try
        {
            MailAddress mail = new MailAddress(email);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
