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
    [SerializeField] private GeneralManager generalManager;
    public float MaxHealth; //Vida maxima 
    public float CurrentHealth; //Vida actual
    public Image healthImage; // Imagen del HUD
    private Vector3 PlayerSpawnPosition;
    private Quaternion PlayerSpawnRotation;
    
   

    /*-------- Void Start && Void Update ---------*/
    void Start()
    {
        // Inicializamos la vida al maximo
        MaxHealth = 100f;
        CurrentHealth = MaxHealth;
        PlayerSpawnPosition = transform.position;
        PlayerSpawnRotation = transform.rotation; 
    }

    public void DamagePlayer(float damage)
    {
        //Programa para recibir damage
        if (CurrentHealth > 0)
        {
            CurrentHealth -= damage;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
        }
        else
        {
            KillPlayer();
        }

        LifeImageFillAmount();
        Debug.Log("Te han dañado" + CurrentHealth);

    }

    public void HealPlayer(float heal)
    {
        //Programa para sanar
        if(CurrentHealth > 0)
        {
            if(CurrentHealth + heal <= MaxHealth)
            {
                CurrentHealth += heal;
                CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
            }
            else 
            {
                CurrentHealth = 100f;
            }
            LifeImageFillAmount();
            Debug.Log("TE ha curado hasta " + CurrentHealth + "% de vida");
            
        }
        else
        {
            KillPlayer();
        }
    }

    public void KillPlayer()
    {      
        transform.position = PlayerSpawnPosition;
        transform.rotation = PlayerSpawnRotation;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.None;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        CurrentHealth = 100f;
        LifeImageFillAmount();
        generalManager.decreaseScore(200);

    }

    public void LifeImageFillAmount()
    {
            healthImage.fillAmount = CurrentHealth / MaxHealth;
    }

}
