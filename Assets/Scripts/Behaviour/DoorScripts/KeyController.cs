using TMPro;
using UnityEngine;

public class KeyController : MonoBehaviour
{
    private AudioSource sfxAudioSource;

    [SerializeField] private AudioClip audioClip;

    private GeneralManager generalManager;
    private RaycastController raycast;

    private bool take;
    private Outline outline;

    void Start()
    {
        generalManager = FindFirstObjectByType<GeneralManager>();
        raycast = FindFirstObjectByType<RaycastController>();
        outline = GetComponent<Outline>();

        outline.enabled = true;

        sfxAudioSource = generalManager.sfxSource;
    }

    private void Update()
    {
        outline.OutlineColor = new Color(0, 0, 0, 0);

        if (raycast.GetHitObjectName() == "PrisonGate Key(Clone)")
        {
            outline.OutlineColor = new Color(1, 1, 1, 1);

            generalManager.SetInteractionText("Click to grab the key");

            if (Input.GetMouseButtonDown(0))
            {
                SaveOnInventory();

                // ✅ NUEVO SISTEMA
                DataManager.Instance.SetFirstKeyTime(generalManager.GetTime());

                GetComponent<Renderer>().enabled = false;

                // ⚠️ Protección por si no existe "Luz"
                Transform luz = transform.Find("Luz");
                if (luz != null)
                {
                    luz.gameObject.SetActive(false);
                }

                sfxAudioSource.PlayOneShot(audioClip);

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