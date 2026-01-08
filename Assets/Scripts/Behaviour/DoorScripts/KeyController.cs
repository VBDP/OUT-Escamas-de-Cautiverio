using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyController : MonoBehaviour
{
    [SerializeField] private PrisonGate prison;

    [SerializeField] private string whatDoorOpens;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;
    private RaycastController raycast;
    private bool take;
    private TextMeshProUGUI interactionText;
    private Outline outline;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        raycast = FindObjectOfType<RaycastController>();
        prison = FindObjectOfType<PrisonGate>();
        outline = GetComponent<Outline>();
    }

    private void Update()
    {
        prison.ClearText();
        if (raycast.GetHitObjectName() == "PrisonGate Key(Clone)")
        {
            prison.interactionTextForKey();
            if (Input.GetMouseButtonDown(0))
            {
                SaveOnInventory();
                GetComponent<Renderer>().enabled = false;
                transform.Find("Luz").gameObject.SetActive(false);

                audioSource.PlayOneShot(audioClip);

                Destroy(gameObject, 1f);
            }
        }
    }

    private void SaveOnInventory()
    {
        take = true;
    }

    public bool GetKey()
    {
        return take;
    }


}
