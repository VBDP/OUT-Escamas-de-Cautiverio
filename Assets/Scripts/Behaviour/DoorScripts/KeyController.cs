using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;
    private GeneralManager generalManager;
    private RaycastController raycast;
    private bool take;
    private Outline outline;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        raycast = FindObjectOfType<RaycastController>();
        outline = GetComponent<Outline>();
        outline.enabled = true;
        audioSource = GetComponent<AudioSource>();   
        generalManager = FindObjectOfType<GeneralManager>();
        
    }

    private void Update()
    {
        outline.OutlineColor = new Color(0,0,0,0);

        if (raycast.GetHitObjectName() == "PrisonGate Key(Clone)")
        {
            outline.OutlineColor = new Color(1,1,1,1);
            generalManager.SetInteractionText("Click to grab the key");
            if (Input.GetMouseButtonDown(0))
            {
                SaveOnInventory();
                GetComponent<Renderer>().enabled = false;
                transform.Find("Luz").gameObject.SetActive(false);
                AudioSource.PlayClipAtPoint(audioClip, transform.position);
                Destroy(gameObject);
            }
        }
    }

    private void SaveOnInventory()
    {
        take = true;
        generalManager.EnableKeyImage();
    }

    public bool GetKey()
    {
        return take;
    }


}
