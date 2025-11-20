using UnityEngine;
using UnityEngine.UI;

// Este Script es un sistema que permite al juego restar salud del jugador con diversos eventos.
// Trampas, Caidas al vacío, ataques, ...
// También permite sanar al jugador con pociones, objetos especiales...

public class LifeSystem : MonoBehaviour
{
    public float MaxHealth; //Vida maxima 
    public float CurrentHealth; //Vida actual
    public Image LifeImage; // Imagen del HUD
    private Vector3 PlayerSpawn;
    // ------------------------------------------------------------------------
    void Start()
    {
        // Inicializamos la vida al maximo
        MaxHealth = 100f;
        CurrentHealth = MaxHealth;
        PlayerSpawn = transform.position;
    }
    // ------------------------------------ ------------------------------------
    public void DamagePlayer(float damage)
    {
        //Programa para recibir daño
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
    // ------------------------------------------------------------------------
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
            Debug.Log("Te han curado" + CurrentHealth);
            
        }
        else
        {
            KillPlayer();
        }
    }
    // ------------------------------------------------------------------------
    public void KillPlayer()
    {      
        transform.position = PlayerSpawn;
        CurrentHealth = 100f;
        LifeImageFillAmount();
    }
    // ------------------------------------------------------------------------
    public void LifeImageFillAmount()
    {
        if (LifeImage != null && CurrentHealth > 0f)
        {
            LifeImage.fillAmount = CurrentHealth / MaxHealth;
        }
    }
    // ------------------------------------------------------------------------
}
