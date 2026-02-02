using TMPro;
using UnityEngine;
using UnityEngine.UI;

/*--------  ---------*/
/*
  Este Script es un sistema que permite al juego restar salud del jugador con diversos eventos.
  Trampas, Caidas al vacio, ataques, ...
  Tambien permite sanar al jugador con pociones, objetos especiales...
*/

public class LifeSystem : MonoBehaviour
{
    private GeneralManager generalManager;
    public float maxHealth; //Vida maxima 
    public float currentHealth; //Vida actual
    public Image healthImage; // Imagen del HUD
    private Vector3 playerSpawnPosition;
    private Quaternion playerSpawnRotation;
    public TextMeshProUGUI interactionText;
    private AudioSource playerAudio;
    [SerializeField] private AudioClip playerDamage;
    [SerializeField] private AudioClip playerDie;


    /*-------- Void Start && Void Update ---------*/
    void Start()
    {
        // Inicializamos la vida al maximo
        generalManager = FindFirstObjectByType<GeneralManager>();
        interactionText = generalManager.interactionText;
        maxHealth = 100f;
        currentHealth = maxHealth;
        healthImage = generalManager.healthBar;
        playerSpawnPosition = transform.position;
        playerSpawnRotation = transform.rotation;
        playerAudio = GetComponent<AudioSource>();
    }

    public void DamagePlayer(float damage)
    {
        //Programa para recibir damage
        if (currentHealth > 0)
        {
            currentHealth -= damage;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            playerAudio.PlayOneShot(playerDamage);
        }
        else
        {
            KillPlayer();
        }

        LifeImageFillAmount();
        Debug.Log("Te han dañado" + currentHealth);
    }

    public void HealPlayer(float heal)
    {
        //Programa para sanar
        if (currentHealth > 0)
        {
            if (currentHealth + heal <= maxHealth)
            {
                currentHealth += heal;
                currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            }
            else
            {
                currentHealth = 100f;
            }

            LifeImageFillAmount();
            Debug.Log("Te ha curado hasta " + currentHealth + "% de vida");
        }
        else
        {
            KillPlayer();
        }
    }

    public void KillPlayer()
    {
        transform.position = playerSpawnPosition;
        transform.rotation = playerSpawnRotation;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.None;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        currentHealth = 100f;
        LifeImageFillAmount();
        if (generalManager != null)
        {
            generalManager.DecreaseScore(200);
            generalManager.EnableDecreaseText(200);
            generalManager.DisableDecreaseTextDelayed(2f);
            generalManager.EnableDeathPanel();
            playerAudio.PlayOneShot(playerDie);
        }
    }

    public void LifeImageFillAmount()
    {
        healthImage.fillAmount = currentHealth / maxHealth;
    }
}