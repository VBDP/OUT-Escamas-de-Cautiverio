using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
public class UserDataSaver : MonoBehaviour
{

    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private TMP_InputField mailInputField;
    [SerializeField] private GameObject LoginPanel;
    [SerializeField] private GameObject HudPanel;
     private PlayerMovement playerMovement;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        LoginPanel.SetActive(true);
    }

    public void saveData()
    {

        string name = nameInputField.text;
        string mail = mailInputField.text;

        PlayerPrefs.SetString("name", name);
        PlayerPrefs.SetString("mail", mail);

        Debug.Log("Data saved: " + name + ", " + mail);
        LoginPanel.SetActive(false);
        HudPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerMovement.UnblockCamera();
    }
}
