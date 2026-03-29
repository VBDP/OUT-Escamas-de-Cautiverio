using System;
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
    String actualScene;
    private GeneralManager generalManager;
    public float maxHealth; //Vida maxima 
    public float currentHealth; //Vida actual
    public Image healthImage; // Imagen del HUD
    private Vector3 playerSpawnPosition;
    private Quaternion playerSpawnRotation;
    public TextMeshProUGUI interactionText;
    private AudioSource sfxSource;
    [SerializeField] private AudioClip playerDamage;
    [SerializeField] private AudioClip playerDie;
    private int deaths = 0;


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
        sfxSource = generalManager.sfxSource;
        actualScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        deaths = 0;
    }

    public void DamagePlayer(float damage)
    {
        // Reducir vida
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        
        // Sonido de daño
        if (sfxSource != null && playerDamage != null && currentHealth > 0)
        {
            sfxSource.PlayOneShot(playerDamage);
        }

        // Si la vida llega a 0, matar al jugador
        if (currentHealth <= 0)
        {
            KillPlayer();
        }

        LifeImageFillAmount();
        Debug.Log("Vida actual: " + currentHealth);
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
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.None;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
        currentHealth = 100f;
        LifeImageFillAmount();
        if (generalManager != null)
        {
            deaths += 1;
            generalManager.DecreaseScore(200);
            generalManager.EnableDecreaseText(200);
            generalManager.DisableDecreaseTextDelayed(2f);
            generalManager.EnableDeathPanel();
            
            if (sfxSource != null && playerDie != null)
            {
                sfxSource.PlayOneShot(playerDie);
            }
        }
    }

    public void LifeImageFillAmount()
    {
        if (healthImage != null)
        {
            healthImage.fillAmount = currentHealth / maxHealth;
        }
    }

    public int GetDeaths()
    {
        return deaths;
    }
}