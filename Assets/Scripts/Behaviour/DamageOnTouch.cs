using UnityEngine;
using UnityEngine.UI;

public class DamageOnTouch : MonoBehaviour
{
    public int customDamage;
    public LifeSystem LifeSystem;

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "Player")
        {
            if (LifeSystem.CurrentHealth - customDamage > 0)
            {
                LifeSystem.DamagePlayer(customDamage);
                LifeSystem.LifeImageFillAmount();
            }
            else
            {
                LifeSystem.KillPlayer();
            }
                

            
        }
    }
}
