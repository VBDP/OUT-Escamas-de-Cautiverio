using System;
using UnityEngine;
using UnityEngine.UI;

public class RunaPickup : MonoBehaviour
{
    private RaycastController raycast;
    private Inventory inventario;
    private GeneralManager generalManager;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject hitObject;
    [SerializeField] private Transform tpPoint;
    [SerializeField] private Transform playerTpDestination; // Destino del jugador
    [SerializeField] private GameObject blockLeft;
    [SerializeField] private GameObject blockRight;

    [Header("UI & Transitions")]
    [SerializeField] private CanvasGroup fadeOverlay;
    [SerializeField] private float fadeDuration = 1.0f;

    [SerializeField] private Image space1;
    [SerializeField] private Image space2;

    [SerializeField] private Sprite othillaSprite;
    [SerializeField] private Sprite jeraSprite;

    private AudioSource sfx;
    [SerializeField] private AudioClip runePickup;

    private bool isTeleporting = false;

    void Start()
    {
        generalManager = FindFirstObjectByType<GeneralManager>();
        raycast = FindFirstObjectByType<RaycastController>();
        inventario = FindFirstObjectByType<Inventory>();

        sfx = generalManager.sfxSource;

        // Si hay un overlay de fade, nos aseguramos de que esté invisible al inicio
        if (fadeOverlay != null)
        {
            fadeOverlay.alpha = 0f;
            fadeOverlay.blocksRaycasts = false;
        }
    }

    private void Update()
    {
        if (isTeleporting) return;

        if (raycast.GetHitObjectName() == hitObject.name)
        {
            if (Input.GetMouseButtonDown(0))
            {
                PickUpRune();
            }
        }
    }

    private void PickUpRune()
    {
        if (hitObject.name == "Jera")
        {
            sfx.PlayOneShot(runePickup);
            inventario.Jera = true;
            DataManager.Instance.SetJeraTime(generalManager.GetTime());
            blockRight.SetActive(true);

            if (space1.sprite == null)
            {
                space1.sprite = jeraSprite;
                space1.gameObject.SetActive(true);
            }
            else
            {
                space2.sprite = jeraSprite;
                space2.gameObject.SetActive(true);
            }
        }
        else if (hitObject.name == "Othilla")
        {
            sfx.PlayOneShot(runePickup);
            inventario.Othilla = true;
            DataManager.Instance.SetOthillaTime(generalManager.GetTime());
            blockLeft.SetActive(true);

            if (space1.sprite == null)
            {
                space1.sprite = othillaSprite;
                space1.gameObject.SetActive(true);
            }
            else
            {
                space2.sprite = othillaSprite;
                space2.gameObject.SetActive(true);
            }
        }

        // Mover la runa fuera del mapa (lógica original)
        transform.position = new Vector3(
            tpPoint.position.x,
            tpPoint.position.y,
            tpPoint.position.z + 1000
        );

        // Iniciar secuencia de teletransporte del jugador
        if (playerTpDestination != null)
        {
            StartCoroutine(TeleportSequence());
        }
    }

    private System.Collections.IEnumerator TeleportSequence()
    {
        isTeleporting = true;

        // 1. Fade Out (Hacia negro)
        if (fadeOverlay != null)
        {
            float elapsed = 0;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                fadeOverlay.alpha = Mathf.Clamp01(elapsed / fadeDuration);
                yield return null;
            }
        }

        // 2. Teletransporte
        if (player != null && playerTpDestination != null)
        {
            // Desactivar temporalmente Rigidbody si existe para evitar conflictos de física
            Rigidbody rb = player.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            player.transform.position = playerTpDestination.position;
            player.transform.rotation = playerTpDestination.rotation;
        }

        // Breve espera en negro
        yield return new WaitForSeconds(0.2f);

        // 3. Fade In (Vuelve la imagen)
        if (fadeOverlay != null)
        {
            float elapsed = 0;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                fadeOverlay.alpha = 1f - Mathf.Clamp01(elapsed / fadeDuration);
                yield return null;
            }
        }

        isTeleporting = false;
    }


    public void ResetJera()
    {
        inventario.Jera = false;

        if (space1.sprite == jeraSprite)
        {
            space1.sprite = null;
            space1.gameObject.SetActive(false);
        }
        else if (space2.sprite == jeraSprite)
        {
            space2.sprite = null;
            space2.gameObject.SetActive(false);
        }
    }

    public void ResetOthilla()
    {
        inventario.Othilla = false;

        if (space1.sprite == othillaSprite)
        {
            space1.sprite = null;
            space1.gameObject.SetActive(false);
        }
        else if (space2.sprite == othillaSprite)
        {
            space2.sprite = null;
            space2.gameObject.SetActive(false);
        }
    }
}